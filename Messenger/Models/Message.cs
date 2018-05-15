using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MessengerApp.Models;
using System;

namespace MessengerApp.Models
{
  public class Message
    {
        private int _id;
        private string _text;
        private bool _seen;
        private int _fromUserId;
        private int _toUserId;

        // We no longer declare _categoryId here

        public Message(string userText, int fromUserId, int toUserId, int id = 0,  bool userSeen = false)
        {
            _text = userText;
            _seen = userSeen;
            _fromUserId = fromUserId;
            _toUserId = toUserId;
            _id = id;
           // categoryId is removed from the constructor
        }

        public override bool Equals(System.Object otherMessage)
        {
          if (!(otherMessage is Message))
          {
            return false;
          }
          else
          {
             Message newMessage = (Message) otherMessage;
             bool idEquality = this.GetId() == newMessage.GetId();
             bool seenEquality = this.GetSeen() == newMessage.GetSeen();
             bool textEquality = this.GetText() == newMessage.GetText();
             bool fromUserIdEquality = this.GetFromUserId() == newMessage.GetFromUserId();
             bool toUserIdEquality = this.GetToUserId() == newMessage.GetToUserId();
             return (idEquality && seenEquality && textEquality && fromUserIdEquality && toUserIdEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetText().GetHashCode();
        }

        public string GetText()
        {
            return _text;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetFromUserId()
        {
          return _fromUserId;
        }

        public int GetToUserId()
        {
          return _toUserId;
        }

        public bool GetSeen()
        {
            return _seen;
        }

        public void SetSeen(bool maybeSeen)
        {
            _seen = maybeSeen;
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO message (text, fromUserId, toUserId, seen) VALUES (@text, @fromUserId, @toUserId, @seen);";

            MySqlParameter text = new MySqlParameter();
            text.ParameterName = "@text";
            text.Value = this._text;
            cmd.Parameters.Add(text);

            MySqlParameter fromUserId = new MySqlParameter();
            fromUserId.ParameterName = "@fromUserId";
            fromUserId.Value = this._fromUserId;
            cmd.Parameters.Add(fromUserId);

            MySqlParameter toUserId = new MySqlParameter();
            toUserId.ParameterName = "@toUserId";
            toUserId.Value = this._toUserId;
            cmd.Parameters.Add(toUserId);

            MySqlParameter seen = new MySqlParameter();
            seen.ParameterName = "@seen";
            seen.Value = this._seen;
            cmd.Parameters.Add(seen);


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
            cmd.CommandText = @"DELETE FROM message WHERE id = @thisId;";

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

        public static List<Message> GetAll(int userId1, int userId2)
        {
            List<Message> allMessages = new List<Message> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM message WHERE fromUserId = @searchId1 AND toUserId = @searchId2;";

            MySqlParameter searchId1 = new MySqlParameter();
            searchId1.ParameterName = "@searchId1";
            searchId1.Value = userId1;
            cmd.Parameters.Add(searchId1);

            MySqlParameter searchId2 = new MySqlParameter();
            searchId2.ParameterName = "@searchId2";
            searchId2.Value = userId2;
            cmd.Parameters.Add(searchId2);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int messageId = rdr.GetInt32(0);
              string messageText = rdr.GetString(1);
              int fromUserId = rdr.GetInt32(2);
              int toUserId = rdr.GetInt32(3);
              bool messageSeen = rdr.GetBoolean(4);
              Message newMessage = new Message(messageText, fromUserId, toUserId, messageId, messageSeen);
              allMessages.Add(newMessage);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allMessages;
        }

        public static Message Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM message WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int messageId = 0;
            string messageText = "";
            int userId1 = 0;
            int userId2 = 0;
            bool seen = false;

            while(rdr.Read())
            {
               messageId = rdr.GetInt32(0);
               messageText = rdr.GetString(1);
               userId1 = rdr.GetInt32(2);
               userId2 = rdr.GetInt32(3);
               seen = rdr.GetBoolean(4);
            }
            Message newMessage = new Message(messageText, userId1, userId2, messageId, seen);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newMessage;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM message;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
