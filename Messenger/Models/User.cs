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

    public void SetPassword(string newPassword)
    {
      _password = newPassword;
    }



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


    public static bool Unique(string loginName)
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


    public List<User> GetConnectionsFrom()
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
      return connections;

    }

    public List<User> GetConnectionsTo(List<User> connectionsFrom)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users
      JOIN message WHERE message.fromUserId = users.id
      AND message.toUserId = @toUserId;";

      MySqlParameter checkingToUserId = new MySqlParameter();
      checkingToUserId.ParameterName = "@toUserId";
      checkingToUserId.Value = _id;
      cmd.Parameters.Add(checkingToUserId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int userId = 0;
      string userName = "";
      string userPassword = "";

      while(rdr.Read())
      {
        userId = rdr.GetInt32(0);
        userName = rdr.GetString(1);
        userPassword = rdr.GetString(2);
        User newUser = new User(userName, userPassword, userId);
        if (!connectionsFrom.Contains(newUser))
          {
            connectionsFrom.Add(newUser);
          }
      }

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return connectionsFrom;

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



    public List<Message> GetNotSeen (int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM message WHERE fromUserId = (@fromUserId) AND toUserId = (@toUserId) AND seen = (@seen);";

            MySqlParameter fromUserId = new MySqlParameter();
            fromUserId.ParameterName = "@fromUserId";
            fromUserId.Value = id;
            cmd.Parameters.Add(fromUserId);

            MySqlParameter toUserId = new MySqlParameter();
            toUserId.ParameterName = "@toUserId";
            toUserId.Value = _id;
            cmd.Parameters.Add(toUserId);

            MySqlParameter seen = new MySqlParameter();
            seen.ParameterName = "@seen";
            seen.Value = false;
            cmd.Parameters.Add(seen);

            List<Message> allMessages = new List<Message>{};
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int messageId = rdr.GetInt32(0);
              string messageText = rdr.GetString(1);
              int fromId = rdr.GetInt32(2);
              int toId = rdr.GetInt32(3);
              bool messageSeen = rdr.GetBoolean(4);
              Message newMessage = new Message(messageText, fromId, toId, messageId, messageSeen);
              allMessages.Add(newMessage);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allMessages;
        }





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

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM users WHERE id = @thisId;";

        MySqlParameter idParameter = new MySqlParameter();
        idParameter.ParameterName = "@thisId";
        idParameter.Value = _id;
        cmd.Parameters.Add(idParameter);

        cmd.ExecuteNonQuery();

        conn.Close();
        if(conn != null)
        {
          conn.Dispose();
        }
    }

      public bool Edit(string newName, string newPassword)
      {
          // check if newName is unique first
          if (User.Unique(newName))
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"UPDATE users SET name = @newName AND password = @newPassword WHERE id = @searchId;";

              MySqlParameter searchId = new MySqlParameter();
              searchId.ParameterName = "@searchId";
              searchId.Value = _id;
              cmd.Parameters.Add(searchId);

              MySqlParameter name = new MySqlParameter();
              name.ParameterName = "@newName";
              name.Value = newName;
              cmd.Parameters.Add(name);

              MySqlParameter password = new MySqlParameter();
              password.ParameterName = "@newPassword";
              password.Value = newName;
              cmd.Parameters.Add(password);

              cmd.ExecuteNonQuery();
              this.SetName(newName);
              this.SetPassword(newPassword);

              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
              return true;
          } else {
              return false;
          }
      }

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

      return newUser;
    }

    public static List<User> Search(string userName)
    {
      List<User> allUsersFound = new List<User> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE name LIKE CONCAT('%',@userName,'%');";

      MySqlParameter userNameParameter = new MySqlParameter();
      userNameParameter.ParameterName = "@userName";
      userNameParameter.Value = userName;
      cmd.Parameters.Add(userNameParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string password = rdr.GetString(2);

        User newUser = new User(name, password, id);
        allUsersFound.Add(newUser);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allUsersFound;
    }

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
