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
      List<Message> messages = Message.GetAll(id,connectionId);
      List<Message> messagesToThisUser = Message.GetAll(connectionId,id);
      messages.AddRange(messagesToThisUser);
      Message tmp = new Message("",0,0);
      for (int i = 0; i < messages.Count; i++)
      {
          for (int j = i + 1; j < messages.Count; j++)
          {
              if (messages[i].GetId() > messages[j].GetId())
              {
                  tmp = messages[i];
                  messages.Remove(messages[i]);
                  messages.Insert(j, tmp);
              }
          }
      }
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
  }
}
