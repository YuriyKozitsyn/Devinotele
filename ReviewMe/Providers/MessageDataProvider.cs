using Newtonsoft.Json;
using Scheduler.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Scheduler.Providers
{
    public class MessageDataProvider
    {
        private string DatabaseConnection;

        public MessageDataProvider()
        {
            DatabaseConnection = ConfigurationManager.ConnectionStrings["ForReview"].ConnectionString;
        }

        public IEnumerable<Message> GetMessagesByDate(DateTime dateTime)
        {
            using (var sqlConnection =
                new SqlConnection(DatabaseConnection))
            {
                //{"DelayDateTime":"09-22-2016","Destination":"Destination","Id":1,"Source":"Source","Text":"Text","Type":5}
                using (var command = new SqlCommand("GetMessagesByDate", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@param1", dateTime));

                    sqlConnection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return ReadMessage(dr);
                        }
                    }
                    sqlConnection.Close();
                }
            }
        }

        public void DeleteMessages(IEnumerable<Message> messages)
        {
            Parallel.ForEach(messages, DeleteMessage);
        }

        private void DeleteMessage(Message message)
        {
            using (var sqlConnection =
               new SqlConnection(DatabaseConnection))
            {
                using (var command = new SqlCommand("DeleteMessageById", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                   
                })
                {
                    command.Parameters.Add(new SqlParameter("@param1", message.Id));
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }

        private Message ReadMessage(SqlDataReader dataReader)
        {
            var message = JsonConvert.DeserializeObject<Message>(dataReader.GetColumnValue<string>("Message"));

            return new Message
            {
                Id = dataReader.GetColumnValue<int>("ID"),
                DelayDateTime = dataReader.GetColumnValue<DateTime>("DelayDateTime"),
                Text = message.Text,
                Destination = message.Destination,
                Source = message.Source,
                Type = message.Type
            };
        }
    }
}
