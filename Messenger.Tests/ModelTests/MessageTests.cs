using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MessengerApp.Models;
using System;

namespace MessengerApp.Tests
{
  [TestClass]
  public class MessengerAppTest : IDisposable
  {
    public MessengerAppTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=messenger_test;";
    }
    public void Dispose()
    {
      User.DeleteAll();
      Message.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameProperties_BloodBank()
    {
      //Arrange, Act
      Message firstMessage = new Message("Hey Eva!", 0, 1);
      Message secondMessage = new Message("Hey Eva!", 0, 1);

      //Assert
      Assert.AreEqual(firstMessage, secondMessage);
    }


    [TestMethod]
    public void GetAll_Database_true()
    {
      //Arrange, Act
      User userOne = new User("Jim", "1234");
      userOne.Save();
      User userTwo = new User("Eva", "4321");
      userTwo.Save();
      User userThree = new User("Verna", "1515");
      userThree.Save();


      Message firstMessage = new Message("Hey Eva!", userOne.GetId(), userTwo.GetId());
      firstMessage.Save();
      Message secondMessage = new Message("Hey Jim!", userTwo.GetId(), userOne.GetId());
      secondMessage.Save();

      Message thirdMessage = new Message("Hey Jim!", userThree.GetId(), userOne.GetId());
      thirdMessage.Save();


      Console.WriteLine("User 1 id:");
      Console.WriteLine(userOne.GetId());

      Console.WriteLine("User 2 id:");
      Console.WriteLine(userTwo.GetId());

      List<Message> allMessages = Message.GetAll(userOne.GetId(), userTwo.GetId());
      Console.WriteLine(allMessages.Count);
      List<Message> test = new List<Message>{firstMessage, secondMessage};
      Console.WriteLine(test.Count);


      //Assert
      CollectionAssert.AreEqual(test, allMessages);
    }

    [TestMethod]
    public void DeleteConversation_DeleteAllMessagesBetweenTwoUsers_GetAllEmpty()
    {
        //Arrange, Act
        User userOne = new User("Jim", "1234");
        userOne.Save();
        User userTwo = new User("Eva", "4321");
        userTwo.Save();

        Message firstMessage = new Message("Hey Eva!", userOne.GetId(), userTwo.GetId());
        firstMessage.Save();
        Message secondMessage = new Message("Hey Jim!", userTwo.GetId(), userOne.GetId());
        secondMessage.Save();

        Message.DeleteConversation(userOne.GetId(), userTwo.GetId());
        List<Message> allMessages = Message.GetAll(userOne.GetId(), userTwo.GetId());

        Assert.AreEqual(0, allMessages.Count);
    }

  }
}
