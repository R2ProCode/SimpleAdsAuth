using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAdsAuth.Data
{
    public class SimpleAdRepository
    {
        private readonly string _connectionString;

        public SimpleAdRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SimpleAd> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            //can do join
            command.CommandText = @"SELECT *, 
                                   (SELECT Name FROM Users WHERE Id = sa.UserId) 
                                    AS UserName FROM SimpleAds sa
                                    ORDER BY DateCreated DESC";
            connection.Open();

            var simpleAds = new List<SimpleAd>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                simpleAds.Add(new SimpleAd
                {
                    Id = (int)reader["Id"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["UserName"]
                });
            }

            return simpleAds;
        }
        public void AddAd(SimpleAd ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO SimpleAds (PhoneNumber, Description, DateCreated, UserId)
                                    VALUES (@phoneNumber, @description, @dateCreated, @userId);";
            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            command.Parameters.AddWithValue("@description", ad.Description);
            command.Parameters.AddWithValue("dateCreated", DateTime.Now);
            command.Parameters.AddWithValue("@userId", ad.UserId);
            connection.Open();

            command.ExecuteNonQuery();
        }

        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM SimpleAds WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<SimpleAd> GetAdsForUser(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT *, 
                                   (SELECT Name FROM Users WHERE Id = sa.UserId) 
                                    AS UserName FROM SimpleAds sa WHERE UserId = @userId
                                    ORDER BY DateCreated DESC";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();

            var reader = command.ExecuteReader();
            
            var simpleAds = new List<SimpleAd>();
            while (reader.Read())
            {
                simpleAds.Add(new SimpleAd
                {
                    Id = (int)reader["Id"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["UserName"]
                });
            }

            return simpleAds;
        }

        public List<int> GetAdIdsForUser(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id FROM SimpleAds WHERE UserId = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();

            var adIds = new List<int>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                adIds.Add((int)reader["Id"]);
            }

            return adIds;
        }
    }
}
