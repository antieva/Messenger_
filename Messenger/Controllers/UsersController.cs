using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class UsersController : Controller
  {
    // [HttpGet("/users/details")]
    // public ActionResult UsersDetails()
    // {
    //   return View();
    // }


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

    [HttpPost ("/users/details")]
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
        List<User> thisUserConnections = newUser.GetConnectionsFrom();
        thisUserConnections = newUser.GetConnectionsTo(thisUserConnections);
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("user", newUser);
        model.Add("connections", thisUserConnections);

        return View(model);
    }

    [HttpPost("/users/update")]
    public ActionResult UpdateAccount()
    {
      return View("UpdateAccount");
    }

    // [HttpPost("/users/search")]
    // public ActionResult Search()
    // {
    //   List<User> searchedUsers = User.Find(Request.Form["searchUsers"]);
    //   return View("UsersList", searchedUsers);
    // }

  }
}
