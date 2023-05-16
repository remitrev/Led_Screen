using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Led_Screen.MVVM.Models
{
    public class MainModel
    {
        private string connectionString = "Server=localhost;Database=led_screen;Uid=visualstudio;Pwd=visualstudio;";
        public List<Message> AllMessages { get; protected set; }

        public MainModel()
        {
            AllMessages = GetFilteredAndOrderMessages(null, "lastUse");
            Debug.Print("test");
        }

        public void CreateOrUpdateMessages(List<string> contents, string tag)
        {
            foreach(var content in contents)
            {
                if(content != "")
                {
                    CreateOrUpdateMessage(content, tag);
                }                
            }
        }

        public void ApplyFilterOrOrderOnList(string filter, string order)
        {
            AllMessages.Clear();
            AllMessages = GetFilteredAndOrderMessages(filter, order);
        }

        #region Private methods
        private void CreateOrUpdateMessage(string content, string tag)
        {
            if(AllMessages.Exists(mess => mess.Tag.Equals(tag) && mess.Content.Equals(content))){
                var message = AllMessages.Find(mess => mess.Tag.Equals(tag) && mess.Content.Equals(content));
                AllMessages.Remove(message);
                message.UpdateLastUpdate(DateTime.Now);
                UpdateMessageInDb(message);
                AllMessages.Add(message);
            } else
            {
                var message = new Message(Guid.NewGuid(), content, tag, DateTime.Now, DateTime.Now);
                AddMessageInDB(message);
                AllMessages.Add(message);
            }
        }
        private List<Message> GetMessagesInDB()
        {
            List<Message> messages = new List<Message>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Debug.Print("Connexion ok");
                string query = "SELECT * FROM message";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid("id");
                        string content = reader.GetString("content");
                        string tag = reader.GetString("tag");
                        DateTime createdDate = reader.GetDateTime("createdDate");
                        DateTime lastUsedDate = reader.GetDateTime("lastUsedDate");

                        Message message = new Message(id, content, tag, createdDate, lastUsedDate);
                        messages.Add(message);
                    }
                }
                connection.Close();
            }
            return messages;
        }

        private List<Message> GetFilteredAndOrderMessages(string filter, string order)
        {
            var messages = GetMessagesInDB();
            if (filter != null && filter != "")
            {
                messages = messages.FindAll(mess => mess.Tag.Contains(filter));
            }
            if (order != null)
            {
                if (order == "lastUse")
                {
                    messages = messages.OrderByDescending(mess => mess.LastUsedDate).ToList();
                }
                else if (order == "create")
                {
                    messages = messages.OrderByDescending(mess => mess.CreatedDate).ToList();
                }
            }

            return messages;
        }
        private void AddMessageInDB(Message message)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Debug.Print("Connexion ok");
                string query = "INSERT INTO message (id, content, tag, createdDate, lastUsedDate) " +
                    "VALUES (@id, @content, @tag, @createdDate, @lastUsedDate)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", message.Id);
                command.Parameters.AddWithValue("@content", message.Content);
                command.Parameters.AddWithValue("@tag", message.Tag);
                command.Parameters.AddWithValue("@createdDate", message.CreatedDate);
                command.Parameters.AddWithValue("@lastUsedDate", message.LastUsedDate);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void UpdateMessageInDb(Message message)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Debug.Print("Connexion ok");
                string query = "UPDATE message SET content = @content, tag = @tag, createdDate = @createdDate, lastUsedDate = @lastUsedDate WHERE id = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@content", message.Content);
                command.Parameters.AddWithValue("@tag", message.Tag);
                command.Parameters.AddWithValue("@createdDate", message.CreatedDate);
                command.Parameters.AddWithValue("@lastUsedDate", message.LastUsedDate);
                command.Parameters.AddWithValue("@id", message.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        #endregion
    }
}
