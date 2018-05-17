using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class UsersController : Controller
  {


    [HttpPost("/users/new")]
    public ActionResult CreateAccount()
    {
        Console.WriteLine(User);
        if (MessengerApp.Models.User.Unique(Request.Form["userName"]))
        {
            User newUser = new User(Request.Form["userName"],Request.Form["userPassword"]);
            newUser.Save();
            return View("NewUser", newUser);
        } else {
            string errorMessage = "Sorry, but your login name is not unique.";
            return View("ErrorMessage", errorMessage);
        }
    }
    [HttpGet("/users/details")]
    public ActionResult UsersUpdated()
    {
      return View("UsersDetails");
    }
    [HttpGet("/users/{id}")]
    public ActionResult UserProfile(int id)
    {
      User currentUser = MessengerApp.Models.User.Find(id);
      List<User> searchedUsers = MessengerApp.Models.User.Search(currentUser.GetName());
      List<User> thisUserConnections = currentUser.GetConnectionsFrom();
      thisUserConnections = currentUser.GetConnectionsTo(thisUserConnections);
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("user", currentUser);
      model.Add("connections", thisUserConnections);
      model.Add("search", searchedUsers);

      return View("UsersDetails", model);
    }
    [HttpPost("/users/details")]
    public ActionResult UsersDetails()
    {
        Console.WriteLine("User name: " + Request.Form["name"]);
        Console.WriteLine("User password: " + Request.Form["password"]);
        User newUser = MessengerApp.Models.User.DoesExist(Request.Form["name"],Request.Form["password"]);
        Console.WriteLine("User name: " + newUser);
        if (newUser == null)
        {
            string errorMessage = "Sorry, but your login name or password is not correct.";
            return View("ErrorMessage", errorMessage);
        }
        List<User> searchedUsers = MessengerApp.Models.User.Search(Request.Form["searchUser"]);
        List<User> thisUserConnections = newUser.GetConnectionsFrom();
        thisUserConnections = newUser.GetConnectionsTo(thisUserConnections);
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("user", newUser);
        model.Add("connections", thisUserConnections);
        model.Add("search", searchedUsers);

        return View(model);
    }

    [HttpGet("/users/{id}/update")]
    public ActionResult UpdateAccount(int id)
    {
      User thisUser = MessengerApp.Models.User.Find(id);
      Console.Write(" ");
      return View(thisUser);
    }
    [HttpPost("/users/{id}/update")]
    public ActionResult UpdateAccountForm(int id)
    {
      Console.Write(" This is the start ");
      User thisUser = MessengerApp.Models.User.Find(id);
      thisUser.Edit(Request.Form["name"], Request.Form["password"]);
      Console.Write(" Hello ");
      Console.WriteLine(" This new user is: " + thisUser);
      return RedirectToAction("UserProfile", new {id = id});
    }
    [HttpPost("/users/{id}/delete")]
    public ActionResult DeleteAccount(int id)
    {
      MessengerApp.Models.User.Delete(id);
      return RedirectToAction("Index", "HomeController");
    }

    [HttpPost("/users/search/{id}")]
    public ActionResult SearchUser(int id)
    {
      User newUser = MessengerApp.Models.User.Find(id);
      List<User> thisUserConnections = newUser.GetConnectionsFrom();
      thisUserConnections = newUser.GetConnectionsTo(thisUserConnections);
      List<User> searchedUsers = MessengerApp.Models.User.Search(Request.Form["searchUser"]);
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("user", newUser);
      model.Add("connections", thisUserConnections);
      model.Add("search", searchedUsers);
      return View("UsersDetails", model);
    }
  }
}
