using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MessengerApp.Models;
using System;

namespace MessengerApp.Models
{
  public class User
  {
    private string _name;
    private string _password;
    private int _id;

    public User(string userName, string userPassword, int user_id = 0)
    {
      _name = userName;
      _password = userPassword;
      _id = user_id;

    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetPassword()
    {
      return _password;
    }

    public void SetId(int newId)
    {
      _id = newId;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    // public void SetList(List<City> cities)
    // {
    //   _cities = cities;
    // }

    public override bool Equals(System.Object otherUser)
    {
      if (!(otherUser is User))
      {
        return false;
      }
      else
      {
        User newUser = (User) otherUser;
        bool idEquality = this.GetId() == newUser.GetId();
        bool nameEquality = this.GetName() == newUser.GetName();
        bool passwordEquality = this.GetPassword() == newUser.GetPassword();
        return (idEquality && nameEquality && passwordEquality);
      }
    }

    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
    }


    public static bool IsUnique(string loginName)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM users WHERE name = (@checkingName);";

        MySqlParameter checkingName = new MySqlParameter();
        checkingName.ParameterName = "@checkingName";
        checkingName.Value = loginName;
        cmd.Parameters.Add(checkingName);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        string userName = "";

        while(rdr.Read())
        {
            userName = rdr.GetString(1);
            if (userName == loginName)
            {
                return false;
            }
        }

        return true;
    }


    public List<User> GetConnections()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users
      JOIN message WHERE message.toUserId = users.id
      AND message.fromUserId = @fromUserId;";

      MySqlParameter checkingFromUserId = new MySqlParameter();
      checkingFromUserId.ParameterName = "@fromUserId";
      checkingFromUserId.Value = _id;
      cmd.Parameters.Add(checkingFromUserId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int userId = 0;
      string userName = "";
      string userPassword = "";
      List<User> connections = new List<User>{};

      while(rdr.Read())
      {
        userId = rdr.GetInt32(0);
        userName = rdr.GetString(1);
        userPassword = rdr.GetString(2);
        User newUser = new User(userName, userPassword, userId);
        if (!connections.Contains(newUser))
          {
            connections.Add(newUser);
          }
      }



      MySqlConnection conn2 = DB.Connection();
      conn2.Open();
      var cmd2 = conn2.CreateCommand() as MySqlCommand; // We need to start here, this SQL statment doesn't work.
      cmd2.CommandText = @"SELECT * FROM users
      JOIN message WHERE message.fromUserId = users.id
      AND message.toUserId = @toUserId;";

      MySqlParameter checkingToUserId = new MySqlParameter();
      checkingToUserId.ParameterName = "@toUserId";
      checkingToUserId.Value = _id;
      cmd2.Parameters.Add(checkingToUserId);
      var rdr2 = cmd2.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        userId = rdr2.GetInt32(0);
        userName = rdr2.GetString(1);
        userPassword = rdr2.GetString(2);
        User newUser = new User(userName, userPassword, userId);
        if (!connections.Contains(newUser))
          {
            connections.Add(newUser);
          }
      }

      conn2.Close();
      if (conn2 != null)
      {
          conn2.Dispose();
      }
      return connections;

    }






    public static User DoesExist(string loginName, string loginPassword)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM users WHERE name = (@checkingName) AND password = (@checkingPassword);";

        MySqlParameter checkingName = new MySqlParameter();
        checkingName.ParameterName = "@checkingName";
        checkingName.Value = loginName;
        cmd.Parameters.Add(checkingName);

        MySqlParameter checkingPassword = new MySqlParameter();
        checkingPassword.ParameterName = "@checkingPassword";
        checkingPassword.Value = loginPassword;
        cmd.Parameters.Add(checkingPassword);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int userId = 0;
        string userName = "";
        string userPassword = "";

        while(rdr.Read())
        {
          userId = rdr.GetInt32(0);
          userName = rdr.GetString(1);
          userPassword = rdr.GetString(2);
        }

        User newUser = new User(userName, userPassword, userId);

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        if (newUser.GetId() == 0)
        {
          return null;
        } else {
          return newUser;
        }
      }


    // public List<City> GetCities()
    //       {
    //           MySqlConnection conn = DB.Connection();
    //           conn.Open();
    //           MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    //           cmd.CommandText = @"SELECT cities.* FROM flights
    //               JOIN cities_flights ON (flights.id = cities_flights.flight_id)
    //               JOIN cities ON (cities_flights.city_id = cities.id)
    //               WHERE flights.id = @FlightId;";
    //
    //           MySqlParameter flightIdParameter = new MySqlParameter();
    //           flightIdParameter.ParameterName = "@FlightId";
    //           flightIdParameter.Value = _id;
    //           cmd.Parameters.Add(flightIdParameter);
    //
    //           MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //           List<City> cities = new List<City>{};
    //
    //           while(rdr.Read())
    //           {
    //             int cityId = rdr.GetInt32(0);
    //             string cityDescription = rdr.GetString(1);
    //             City newCity = new City(cityDescription, cityId);
    //             cities.Add(newCity);
    //           }
    //           conn.Close();
    //           if (conn != null)
    //           {
    //               conn.Dispose();
    //           }
    //           return cities;
    //       }

    // public void AddCity(City newCity)
    //     {
    //         MySqlConnection conn = DB.Connection();
    //         conn.Open();
    //         var cmd = conn.CreateCommand() as MySqlCommand;
    //         cmd.CommandText = @"INSERT INTO cities_flights (flight_id, city_id) VALUES (@FlightId, @CityId);";
    //
    //         MySqlParameter flight_id = new MySqlParameter();
    //         flight_id.ParameterName = "@FlightId";
    //         flight_id.Value = _id;
    //         cmd.Parameters.Add(flight_id);
    //
    //         MySqlParameter city_id = new MySqlParameter();
    //         city_id.ParameterName = "@CityId";
    //         city_id.Value = newCity.GetId();
    //         cmd.Parameters.Add(city_id);
    //
    //         cmd.ExecuteNonQuery();
    //         conn.Close();
    //         if (conn != null)
    //         {
    //             conn.Dispose();
    //         }
    //     }




    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users (name, password) VALUES (@name, @password);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);


      MySqlParameter password = new MySqlParameter();
      password.ParameterName = "@password";
      password.Value = this._password;
      cmd.Parameters.Add(password);
      // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    // Will get all users
    public static List<User> GetAll()
    {
      List<User> allUsers = new List<User> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string userName = rdr.GetString(1);
        string userPassword = rdr.GetString(2);

        User newUser = new User(userName, userPassword, userId);
        newUser.SetId(userId);
        allUsers.Add(newUser);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allUsers;
    }

    public static User Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int userId = 0;
      string userName = "";
      string userPassword = "";
      // We remove the line setting a itemCategoryId value here.

      while(rdr.Read())
      {
        userId = rdr.GetInt32(0);
        userName = rdr.GetString(1);
        userPassword = rdr.GetString(2);

      }

      // Constructor below no longer includes a itemCategoryId parameter:
      User newUser = new User(userName, userPassword, userId);
    //  newCategory.SetDate(ItemDueDate);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }

      return newUser;
    }

    // public static void Delete(int id)
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   MySqlCommand cmd = new MySqlCommand("DELETE FROM users WHERE id = @UserId; DELETE FROM cities_flights WHERE flight_id = @FlightId;", conn);
    //   MySqlParameter userIdParameter = new MySqlParameter();
    //   userIdParameter.ParameterName = "@UserId";
    //   userIdParameter.Value = id;
    //
    //   cmd.Parameters.Add(userIdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM users;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
