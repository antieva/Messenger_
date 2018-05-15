using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
// using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class UsersController : Controller
  {
    [HttpGet("/{id}/details")]
    public ActionResult UsersDetails(int id)
    {
      return View();
    }

    [HttpGet("/users/success")]
    public ActionResult NewAccount()
    {
      return View();
    }

    // [HttpPost("/users/search")]
    // public ActionResult Search()
    // {
    //   List<User> searchedUsers = User.Find(Request.Form["searchUsers"]);
    //   return View("UserList", searchedUsers);
    // }
  }
}
