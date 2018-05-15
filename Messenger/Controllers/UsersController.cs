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
  }
}
