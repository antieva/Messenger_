using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MessengerApp.Models;
using System;

namespace MessengerApp.Tests
{
  [TestClass]
  public class UserTest : IDisposable
  {
    public UserTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=messenger_test;";
    }
    public void Dispose()
    {
      User.DeleteAll();
      Message.DeleteAll();
    }

    [TestMethod]
    public void Find_TwoUser_Ids()
    {
        //Arrange
        User userOne = new User("Eva", "123");
        userOne.Save();

        User testUser = userOne;
        User userResult = User.Find(userOne.GetId());
        Console.WriteLine("The user result is:" + userResult.GetPassword());
        //Assert
        Assert.AreEqual(testUser, userResult);
    }

    [TestMethod]
    public void IsUnique_userName_true()
    {
      User userOne = new User("Eva", "123");
      userOne.Save();

      bool result = User.IsUnique("Jim");
      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void DoesExist_userProfile_User()
    {
      User userOne = new User("Jim", "5421");
      userOne.Save();


      User result = User.DoesExist("Jim", "5421");
      if (result == null)
      {
          Console.WriteLine("The result is null");
      }

      Assert.AreEqual(userOne, result);
    }

    [TestMethod]
    public void GetConnectionsFrom_CheckByThisUserId_ListOfUsers()
    {
      User userOne = new User("Jim", "5421");
      userOne.Save();

      User userTwo = new User("Eva", "1234");
      userTwo.Save();

      User userThree = new User("Verna", "7890");
      userThree.Save();


      Message messageOne = new Message("Hi", userOne.GetId(), userTwo.GetId());
      messageOne.Save();
      Message messageTwo = new Message("Hi", userOne.GetId(), userThree.GetId());
      messageTwo.Save();

      Message messageThree = new Message("Hi", userOne.GetId(), userTwo.GetId());
      messageThree.Save();

      List<User> result = userOne.GetConnectionsFrom();
      //Console.WriteLine("Jim's connected to " + result[0].GetName() + " and " + result[1].GetName());
      //Console.WriteLine(result.Count);
      // foreach (var user in result)
      // {
      //     Console.WriteLine(user.GetName());
      // }
      List<User> test = new List<User> {userTwo, userThree};

      CollectionAssert.AreEqual(test, result);
    }

    [TestMethod]
    public void GetConnectionsTo_GetTo_ListOfUsers()
    {
      User userOne = new User("Jim", "5421");
      userOne.Save();

      User userTwo = new User("Eva", "1234");
      userTwo.Save();

      User userThree = new User("Verna", "7890");
      userThree.Save();


      Message messageOne = new Message("Hi", userTwo.GetId(), userOne.GetId());
      messageOne.Save();
      Message messageTwo = new Message("Hi", userThree.GetId(),userOne.GetId());
      messageTwo.Save();

      Message messageThree = new Message("Hi", userTwo.GetId(), userOne.GetId());
      messageThree.Save();

      List<User> result = userOne.GetConnectionsFrom();

      result = userOne.GetConnectionsTo(result);
      //Console.WriteLine("Jim's connected to " + result[0].GetName() + " and " + result[1].GetName());

      foreach (var user in result)
      {
          Console.WriteLine(user.GetName());
      }
      List<User> test = new List<User> {userTwo, userThree};
      Console.WriteLine("List: " + result.Count);

      CollectionAssert.AreEqual(test, result);
    }

    [TestMethod]
    public void GetConnectionsToandFrom_GetToAndFrom_ListOfUsers()
    {
      User userOne = new User("Jim", "5421");
      userOne.Save();

      User userTwo = new User("Eva", "1234");
      userTwo.Save();

      User userThree = new User("Verna", "7890");
      userThree.Save();


      Message messageOne = new Message("Hi", userTwo.GetId(), userOne.GetId());
      messageOne.Save();
      Message messageTwo = new Message("Hi", userOne.GetId(),userThree.GetId());
      messageTwo.Save();

      Message messageThree = new Message("Hi", userTwo.GetId(), userOne.GetId());
      messageThree.Save();

      List<User> result = userOne.GetConnectionsFrom();

      result = userOne.GetConnectionsTo(result);
      //Console.WriteLine("Jim's connected to " + result[0].GetName() + " and " + result[1].GetName());

      foreach (var user in result)
      {
          Console.WriteLine(user.GetName());
      }
      List<User> test = new List<User> {userThree, userTwo};
      Console.WriteLine("List: " + result.Count);

      CollectionAssert.AreEqual(test, result);
    }

    [TestMethod]
    public void GetNotSeen_GettingMessagesNotSeen_MessageList()
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


      Message thirdMessage = new Message("Hey stranger!", userTwo.GetId(), userOne.GetId(), 0, true);
      thirdMessage.Save();
      //thirdMessage.SetSeen(true);

      List<Message> allMessagesNotSeenByJim = userOne.GetNotSeen(userTwo.GetId());
      Console.WriteLine("Jim got " + allMessagesNotSeenByJim.Count + " new messages");
      List<Message> test = new List<Message>{secondMessage};
      Console.WriteLine(test.Count);

      //Assert
      CollectionAssert.AreEqual(test, allMessagesNotSeenByJim);

    }
  }
}
