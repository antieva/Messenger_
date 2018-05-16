using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class MessagesController : Controller
  {
    [HttpGet("/messenger/box")]
    public ActionResult Messenger()
    {
      return View("DialogBox");
    }

    [HttpGet("/messenger")]
    public ActionResult MessengerPage()
    {
      return View("DialogPage");
    }
  }
}
