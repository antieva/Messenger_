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

    // [TestMethod]
    // public void GetAll_DatabaseEmptyAtFirst_0()
    // {
    //   //Arrange, Act
    //   int result = Message.GetAll(0,1).Count;
    //
    //   //Assert
    //   Assert.AreEqual(0, result);
    // }

    [TestMethod]
    public void GetAll_Database_true()
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

      Console.WriteLine("User 1 id:");
      Console.WriteLine(userOne.GetId());

      Console.WriteLine("User 2 id:");
      Console.WriteLine(userTwo.GetId());

      List<Message> allMessagesFromJim = Message.GetAll(userOne.GetId(), userTwo.GetId());
      Console.WriteLine(allMessagesFromJim.Count);
      List<Message> test = new List<Message>{firstMessage};
      Console.WriteLine(test.Count);


      //Assert
      CollectionAssert.AreEqual(test, allMessagesFromJim);
    }

  
    // [TestMethod]
    // public void AddDonor_AddsDonorToBloodBank_DonorList()
    // {
    //   //Arrange
    //   BloodBank testBloodBank = new BloodBank("Puget Sound", "Seattle", "7777777");
    //   testBloodBank.Save();
    //
    //   Donor testDonor = new Donor("Tim", "3333333", "08/11/93", "IIIRh+", "Healthy");
    //   testDonor.Save();
    //
    //   //Act
    //   testBloodBank.AddDonor(testDonor);
    //
    //   List<Donor> result = testBloodBank.GetDonors();
    //   Console.WriteLine(result.Count);
    //   List<Donor> testList = new List<Donor>{testDonor};
    //   Console.WriteLine(testList.Count);
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testList, result);
    // }
    //
    // [TestMethod]
    // public void Delete_DeletesBloodBankAssociationsFromDatabase_BloodBankList()
    // {
    //   //Arrange
    //   BloodBank testBloodBank = new BloodBank("Puget Sound", "Seattle", "7777777");
    //   testBloodBank.Save();
    //
    //   Donor testDonor = new Donor("Tim", "3333333", "08/11/93", "IIIRh+", "Healthy");
    //   testDonor.Save();
    //
    //   //Act
    //   testBloodBank.AddDonor(testDonor);
    //   testBloodBank.Delete();
    //
    //   List<BloodBank> resultBloodBankDonors = testDonor.GetBloodBanks();
    //   List<BloodBank> testBloodBankDonors = new List<BloodBank> {};
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testBloodBankDonors , resultBloodBankDonors);
    // }
  }
}
