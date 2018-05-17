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

        bool result = User.Unique("Jim");
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
    public void GetConnections_GetToAndFrom_ListOfUsers()
    {
        User userOne = new User("Jim", "5421");
        userOne.Save();

        User userTwo = new User("Eva", "1234");
        userTwo.Save();

        User userThree = new User("Verna", "7890");
        userThree.Save();

        User userFour = new User("Jane", "7890");
        userFour.Save();


        Message messageOne = new Message("Hi Jim", userTwo.GetId(), userOne.GetId());
        messageOne.Save();
        Message messageTwo = new Message("Hi Eva", userOne.GetId(),userThree.GetId());
        messageTwo.Save();

        Message messageThree = new Message("Hi Jim", userThree.GetId(), userOne.GetId());
        messageThree.Save();

        Message messageFour = new Message("Hi Jim", userFour.GetId(), userOne.GetId());
        messageFour.Save();

        List<User> result = userOne.GetConnections();

        foreach (var user in result)
        {
            Console.WriteLine(user.GetName());
        }
        List<User> test = new List<User> {userTwo, userFour, userThree};
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

        List<Message> allMessagesNotSeenByJim = userOne.GetNotSeen(userTwo.GetId());
        Console.WriteLine("Jim got " + allMessagesNotSeenByJim.Count + " new messages");
        List<Message> test = new List<Message>{secondMessage};
        Console.WriteLine(test.Count);

        //Assert
        CollectionAssert.AreEqual(test, allMessagesNotSeenByJim);

    }

    [TestMethod]
    public void Search_GettingUsersWhichMatchSearch_UsersList()
    {
        //Arrange, Act
        User userOne = new User("Jim", "1234");
        userOne.Save();
        User userTwo = new User("Eva", "4321");
        userTwo.Save();
        User userThree = new User("NewJim", "1234");
        userThree.Save();

        List<User> result = User.Search("jim");
        foreach (var user in result)
        {
          Console.WriteLine(user.GetName());
        }
        List<User> test = new List<User>{userOne, userThree};
        //Assert
        CollectionAssert.AreEqual(test, result);

    }

    [TestMethod]
    public void Edit_UpdateUserNameAndPassword_False()
    {
        User userOne = new User("Jim", "1234");
        userOne.Save();
        User userTwo = new User("Eva", "4321");
        userTwo.Save();

        bool result = userOne.Edit("Eva", "35435345");

        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void Edit_NotEditIfFalse_NameNotChanged()
    {
        User userOne = new User("Jim", "1234");
        userOne.Save();

        userOne.Edit("Eva", "35435345");
        string result = userOne.GetName();
        Console.WriteLine(userOne.GetName());
        string test = "Eva";

        Assert.AreEqual(test, result);
    }

    [TestMethod]
    public void Edit_UpdateUserNameAndPassword_True()
    {
        User userOne = new User("Jim", "1234");
        userOne.Save();
        User userTwo = new User("Eva", "4321");
        userTwo.Save();

        bool result = userOne.Edit("John", "35435345");
        Console.WriteLine(userOne.GetName());
        Console.WriteLine(userOne.GetPassword());

        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Delete_DeleteUserAndHisMessages_True()
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

        userOne.Delete();
        List<Message> userTwoMessages = Message.GetAll(userTwo.GetId(), userOne.GetId());
        List<Message> test = new List<Message>{};

        CollectionAssert.AreEqual(test, userTwoMessages);
    }
  }
}
