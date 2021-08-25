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
    class MotocycleTMRepository : IMotocycleDbManager
    {
        const string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;" +
                                              "Initial Catalog = Magazzino3;" +
                                              "Integrated Security = true;";
        const string _discriminator = "Motocycle";
        public void Delete(Motocycle motocycle)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "delete from Vehicle where Id = @id";
                command.Parameters.AddWithValue("@id", motocycle.Id);
                command.ExecuteNonQuery();

            }
        }

        public List<Motocycle> Fetch()
        {
            List<Motocycle> motocycles = new List<Motocycle>();
            using(SqlConnection connection = new SqlConnection(connectionString))
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
                    var year = (int)reader["ProductionYear"];
                    var id = (int)reader["Id"];

                    Motocycle motocycle = new Motocycle(brand, model, year, id);
                    motocycles.Add(motocycle);
                }
            }
            return motocycles;
        }

        public Motocycle GetById(int? id)
        {
            Motocycle motocycle = new Motocycle();
            using(SqlConnection connection = new SqlConnection(connectionString))
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
                    var year = (int)reader["ProductionYear"];

                    motocycle = new Motocycle(brand, model, year, id);
            
                }
            }
            return motocycle;
        }

        public void Insert(Motocycle motocycle)
        {
           using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Insert into Vehicle values (@brand, @model, @year, @supply, @doors, @seats, @type)";
                command.Parameters.AddWithValue("@brand", motocycle.Brand);
                command.Parameters.AddWithValue("@model", motocycle.Model);
                command.Parameters.AddWithValue("@year", motocycle.ProductionYear);
                command.Parameters.AddWithValue("@supply", DBNull.Value);
                command.Parameters.AddWithValue("@doors", DBNull.Value);
                command.Parameters.AddWithValue("@seats", DBNull.Value);
                command.Parameters.AddWithValue("@type", _discriminator);

                command.ExecuteNonQuery();

            }
        }

        public void Update(Motocycle motocycle)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "update Vehicle " +
                                      "set Brand = @brand, Model = @model, ProductionYear = @year, Supply = @supply, DoorsNumber = @doors, SeatsNumber = @seats, Discriminator = @type " +
                                      "where Id = @id";
                command.Parameters.AddWithValue("@brand", motocycle.Brand);
                command.Parameters.AddWithValue("@model", motocycle.Model);
                command.Parameters.AddWithValue("@year", motocycle.ProductionYear);
                command.Parameters.AddWithValue("@supply", DBNull.Value);
                command.Parameters.AddWithValue("@doors", DBNull.Value);
                command.Parameters.AddWithValue("@seats", DBNull.Value);
                command.Parameters.AddWithValue("@type", _discriminator);

                command.ExecuteNonQuery();
            }
        }
    }
}
