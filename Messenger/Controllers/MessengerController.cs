using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MessengerApp.Models;

namespace MessengerApp.Controllers
{
  public class MessagesController : Controller
  {
    [HttpGet("/box/{id}/{connectionId}")]
    public ActionResult Messenger(int id, int connectionId)
    {
        User thisUser = MessengerApp.Models.User.Find(id);
        User connectedUser = MessengerApp.Models.User.Find(connectionId);
        List<Message> notSeen = thisUser.GetNotSeen(connectedUser.GetId());
        thisUser.ChangeToSeen(notSeen);
        List<Message> messages = Message.GetAll(id,connectionId);
        Dictionary<string, object> model = new Dictionary<string,object>();
        model.Add("thisUser", thisUser);
        model.Add("connectedUser", connectedUser);
        model.Add("messages", messages);

        return View("DialogBox", model);
    }

    [HttpGet("/dialog/{id}/{connectionId}")]
    public ActionResult MessengerPage(int id, int connectionId)
    {
        User thisUser = MessengerApp.Models.User.Find(id);
        User connectedUser = MessengerApp.Models.User.Find(connectionId);
        Dictionary<string, object> model = new Dictionary<string,object>();
        model.Add("thisUser", thisUser);
        model.Add("connectedUser", connectedUser);
        return View("DialogPage", model);
    }

    [HttpPost("/dialog/{id}/{connectionId}")]
    public ActionResult SendMessage(int id, int connectionId)
    {
        Message newMessage = new Message(Request.Form["message"], id, connectionId);
        newMessage.Save();
        User thisUser = MessengerApp.Models.User.Find(id);
        User connectedUser = MessengerApp.Models.User.Find(connectionId);
        Dictionary<string, object> model = new Dictionary<string,object>();
        model.Add("thisUser", thisUser);
        model.Add("connectedUser", connectedUser);
        return View("DialogPage", model);
    }

    [HttpGet("/dialog/{id}/{connectionId}/deleteConnection")]
    public ActionResult DeleteConversation(int id, int connectionId)
    {
        User thisUser = MessengerApp.Models.User.Find(id);
        User connectedUser = MessengerApp.Models.User.Find(connectionId);
        Message.DeleteConversation(id, connectionId);
        Dictionary<string, object> model = new Dictionary<string,object>();
        model.Add("thisUser", thisUser);
        model.Add("connectedUser", connectedUser);
        return View("DialogPage", model);
    }

    [HttpGet("/dialog/{messageId}/{id}/{connectionId}")]
    public ActionResult DeleteMessage(int messageId, int id, int connectionId)
    {
        Message unwantedMessage = Message.Find(messageId);
        unwantedMessage.Delete();

        return RedirectToAction("Messenger", new {id = id, connectionId = connectionId});
    }
  }
}
