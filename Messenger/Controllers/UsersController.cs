using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class UsersController : Controller
  {
    [HttpGet("/users/details")]
    public ActionResult UsersDetails()
    {
      return View();
    }

    [HttpPost("/users/new")]
    public ActionResult CreateAccount()
    {
      User newUser = new User(Request.Form["userName"],Request.Form["userPassword"]);
      newUser.Save();
      return View("NewAccount");
    }

    // [HttpPost("/users/search")]
    // public ActionResult Search()
    // {
    //   List<User> searchedUsers = User.Find(Request.Form["searchUsers"]);
    //   return View("UsersList", searchedUsers);
    // }
  }
}
