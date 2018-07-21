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
          string name = Request.Form["userName"];
          if (MessengerApp.Models.User.Unique(name.ToLower()))
          {
              if (Request.Form["userPassword1"] == Request.Form["userPassword2"])
              {
                User newUser = new User(name.ToLower(), Request.Form["userPassword1"]);
                newUser.Save();
                return View("NewUser", newUser);
              } else {
                string errorMessage = "Sorry, your pasword does not match";
                return View("ErrorMessage", errorMessage);
              }
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
          List<User> thisUserConnections = currentUser.GetConnections();
          Dictionary<string, object> model = new Dictionary<string, object>();
          model.Add("user", currentUser);
          model.Add("connections", thisUserConnections);
          model.Add("search", searchedUsers);

          return View("UsersDetails", model);
      }
      [HttpPost("/users/details")]
      public ActionResult UsersDetails()
      {
          User newUser = MessengerApp.Models.User.DoesExist(Request.Form["name"],Request.Form["password"]);
          if (newUser == null)
          {
              string errorMessage = "Sorry, but your login name or password is not correct.";
              return View("ErrorMessage", errorMessage);
          }
          int id = newUser.GetId();
//           List<User> searchedUsers = MessengerApp.Models.User.Search(Request.Form["searchUser"]);
//           List<User> thisUserConnections = newUser.GetConnections();
//           Dictionary<string, object> model = new Dictionary<string, object>();
//           model.Add("user", newUser);
//           model.Add("connections", thisUserConnections);
//           model.Add("search", searchedUsers);

          return RedirectToAction("UserProfile", new {id = id});
      }

      [HttpGet ("/users/details/{id}")]
      public ActionResult BackToDetails(int id)
      {
          User thisUser = MessengerApp.Models.User.Find(id);
          List<User> searchedUsers = new List<User>();
          List<User> thisUserConnections = thisUser.GetConnections();
          Dictionary<string, object> model = new Dictionary<string, object>();
          model.Add("user", thisUser);
          model.Add("connections", thisUserConnections);
          model.Add("search", searchedUsers);

          return View("UsersDetails", model);
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
        User newUser = MessengerApp.Models.User.Find(id);
        newUser.Delete();
        return View("DeletedAccount");
      }

      

      [HttpPost("/users/details/{id}")]
      public ActionResult SearchUser(int id)
      {
          User newUser = MessengerApp.Models.User.Find(id);
          List<User> thisUserConnections = newUser.GetConnections();
          List<User> searchedUsers = MessengerApp.Models.User.Search(Request.Form["searchUser"]);
          Dictionary<string, object> model = new Dictionary<string, object>();
          model.Add("user", newUser);
          model.Add("connections", thisUserConnections);
          model.Add("search", searchedUsers);
          return View("UsersDetails", model);
      }
  }
}
