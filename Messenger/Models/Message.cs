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

        // public void SetDate(string newDate)
        // {
        //     _dueDate = newDate;
        // }

        // public void AddFlight(Flight newFlight)
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"INSERT INTO cities_flights (flight_id, city_id) VALUES (@FlightId, @CityId);";
        //
        //     MySqlParameter flight_id = new MySqlParameter();
        //     flight_id.ParameterName = "@FlightId";
        //     flight_id.Value = newFlight.GetId();
        //     cmd.Parameters.Add(flight_id);
        //
        //     MySqlParameter city_id = new MySqlParameter();
        //     city_id.ParameterName = "@CityId";
        //     city_id.Value = _id;
        //     cmd.Parameters.Add(city_id);
        //
        //     cmd.ExecuteNonQuery();
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        // }
        //
        // public List<Flight> GetFlights()
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"SELECT flight_id FROM cities_flights WHERE city_id = @cityId;";
        //
        //     MySqlParameter cityIdParameter = new MySqlParameter();
        //     cityIdParameter.ParameterName = "@cityId";
        //     cityIdParameter.Value = _id;
        //     cmd.Parameters.Add(cityIdParameter);
        //
        //     var rdr = cmd.ExecuteReader() as MySqlDataReader;
        //
        //     List<int> flightIds = new List<int> {};
        //     while(rdr.Read())
        //     {
        //         int flightId = rdr.GetInt32(0);
        //         flightIds.Add(flightId);
        //     }
        //     rdr.Dispose();
        //
        //     List<Flight> flights = new List<Flight> {};
        //     foreach (int flightId in flightIds)
        //     {
        //         var flightQuery = conn.CreateCommand() as MySqlCommand;
        //         flightQuery.CommandText = @"SELECT * FROM flights WHERE id = @FlightId;";
        //
        //         MySqlParameter flightIdParameter = new MySqlParameter();
        //         flightIdParameter.ParameterName = "@FlightId";
        //         flightIdParameter.Value = flightId;
        //         flightQuery.Parameters.Add(flightIdParameter);
        //
        //         var flightQueryRdr = flightQuery.ExecuteReader() as MySqlDataReader;
        //         while(flightQueryRdr.Read())
        //         {
        //             int thisFlightId = flightQueryRdr.GetInt32(0);
        //             string flightName = flightQueryRdr.GetString(1);
        //             int flightDepartureTime = flightQueryRdr.GetInt32(2);
        //             string flightDeparture = flightQueryRdr.GetString(3);
        //             string flightArrival = flightQueryRdr.GetString(4);
        //             string flightStatus = flightQueryRdr.GetString(5);
        //             Flight foundFlight = new Flight(flightName, flightDepartureTime, flightDeparture, flightArrival, flightStatus, thisFlightId);
        //             flights.Add(foundFlight);
        //         }
        //         flightQueryRdr.Dispose();
        //     }
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        //     return flights;
        // }

        // We've removed the GetCategoryId() method entirely.

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

            // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
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

        // public void UpdateDescription(string newDescription)
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"UPDATE cities SET description = @newDescription WHERE id = @searchId;";
        //
        //     MySqlParameter searchId = new MySqlParameter();
        //     searchId.ParameterName = "@searchId";
        //     searchId.Value = _id;
        //     cmd.Parameters.Add(searchId);
        //
        //     MySqlParameter description = new MySqlParameter();
        //     description.ParameterName = "@newDescription";
        //     description.Value = newDescription;
        //     cmd.Parameters.Add(description);
        //
        //     cmd.ExecuteNonQuery();
        //     _description = newDescription;
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        //
        // }
      //
      //   public static void Delete(int id)
      // {
      //   MySqlConnection conn = DB.Connection();
      //   conn.Open();
      //   var cmd = conn.CreateCommand() as MySqlCommand;
      //   cmd.CommandText = @"DELETE FROM cities_flights WHERE flight_id = @CityId;";
      //
      //   MySqlParameter cityIdParameter = new MySqlParameter();
      //   cityIdParameter.ParameterName = "@CityId";
      //   cityIdParameter.Value = id;
      //   cmd.Parameters.Add(cityIdParameter);
      //
      //   cmd.ExecuteNonQuery();
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
            cmd.CommandText = @"DELETE FROM message;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
      //   public void Edit(string newDescription)
      // {
      //     MySqlConnection conn = DB.Connection();
      //     conn.Open();
      //     var cmd = conn.CreateCommand() as MySqlCommand;
      //     cmd.CommandText = @"UPDATE cities SET description = @newDescription WHERE id = @searchId;";
      //
      //     MySqlParameter searchId = new MySqlParameter();
      //     searchId.ParameterName = "@searchId";
      //     searchId.Value = _id;
      //     cmd.Parameters.Add(searchId);
      //
      //     MySqlParameter description = new MySqlParameter();
      //     description.ParameterName = "@newDescription";
      //     description.Value = newDescription;
      //     cmd.Parameters.Add(description);
      //
      //     cmd.ExecuteNonQuery();
      //     _description = newDescription;
      //
      //     conn.Close();
      //     if (conn != null)
      //     {
      //         conn.Dispose();
      //     }
      // }
    }
}
