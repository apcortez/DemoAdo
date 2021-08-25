using Concessionario.Entities;
using Concessionario.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concessionario.ThirdMethodRepository
{
    class BusTMRepository : IBusDbManager
    {
        const string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;" +
                                              "Initial Catalog = Magazzino3;" +
                                              "Integrated Security = true;";
        const string _discriminator = "Bus";
        public void Delete(Bus bus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "delete from Vehicle where Id = @id";
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
                command.CommandText = "select * from Vehicle where Discriminator = @type";
                command.Parameters.AddWithValue("@type", _discriminator);
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
                command.CommandText = "Select * from Vehicle where Id = @id";
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
                command.CommandText = "Insert into Vehicle values (@brand, @model, @year, @supply, @doors, @seats, @type)";
                command.Parameters.AddWithValue("@brand", bus.Brand);
                command.Parameters.AddWithValue("@model", bus.Model);
                command.Parameters.AddWithValue("@year", DBNull.Value);
                command.Parameters.AddWithValue("@supply", DBNull.Value);
                command.Parameters.AddWithValue("@doors", DBNull.Value);
                command.Parameters.AddWithValue("@seats", bus.SeatsNumber);
                command.Parameters.AddWithValue("@type", _discriminator);

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
                command.CommandText = "update Vehicle " +
                                      "set Brand = @brand, Model = @model, ProductionYear = @year, Supply = @supply, DoorsNumber = @doors, SeatsNumber = @seats, Discriminator = @type " +
                                      "where Id = @id";
                command.Parameters.AddWithValue("@brand", bus.Brand);
                command.Parameters.AddWithValue("@model", bus.Model);
                command.Parameters.AddWithValue("@year", DBNull.Value);
                command.Parameters.AddWithValue("@supply", DBNull.Value);
                command.Parameters.AddWithValue("@doors", DBNull.Value);
                command.Parameters.AddWithValue("@seats", bus.SeatsNumber);
                command.Parameters.AddWithValue("@type", _discriminator);

                command.ExecuteNonQuery();
            }
        }
    }
}
