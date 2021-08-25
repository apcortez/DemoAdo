﻿using Concessionario.Entities;
using Concessionario.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concessionario.SecondMethodSqlRepository
{
    class BusSMRepository : IBusDbManager
    {
        const string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;" +
                                              "Initial Catalog = Magazzino;" +
                                              "Integrated Security = true;";
        public void Delete(Bus bus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Delete from Bus where Id = @id";
                command.Parameters.AddWithValue("@id", bus.Id);

                command.ExecuteNonQuery();
            }
        }

        public List<Bus> Fetch()
        {
            List<Bus> buses = new List<Bus>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;

                command.CommandText = "select * from Bus";
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var brand = (string)reader["Brand"];
                    var model = (string)reader["Model"];
                    var seats = (int)reader["SeatsNumber"];
                    var id = (int)reader["Id"];

                    Bus bus = new Bus(brand, model, seats, id);
                    buses.Add(bus);
                }
            }
            return buses;
        }

        public Bus GetById(int? id)
        {
            Bus bus = new Bus();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Select * from Bus where Id = @id";
                command.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var brand = (string)reader["Brand"];
                    var model = (string)reader["Model"];
                    var seats = (int)reader["SeatsNumber"];

                    bus = new Bus(brand, model, seats, id);
                }
            }
            return bus;
        }

        public void Insert(Bus bus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Insert into Bus values(@brand, @model, @seats)";
                command.Parameters.AddWithValue("@brand", bus.Brand);
                command.Parameters.AddWithValue("@model", bus.Model);
                command.Parameters.AddWithValue("@seats", bus.SeatsNumber);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Bus bus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "update Bus set Brand = @brand, Model = @model, SeatsNumber = @seats where Id = @id";
                command.Parameters.AddWithValue("@brand", bus.Brand);
                command.Parameters.AddWithValue("@model", bus.Model);
                command.Parameters.AddWithValue("@year", bus.SeatsNumber);
                command.Parameters.AddWithValue("@id", bus.Id);

                command.ExecuteNonQuery();
            }
        }
    }
}
