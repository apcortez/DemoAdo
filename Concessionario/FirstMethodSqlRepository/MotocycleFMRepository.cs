using Concessionario.Entities;
using Concessionario.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concessionario.FirstMethodSqlRepository
{
    class MotocycleFMRepository : IMotocycleDbManager
    {
        static VehicleFMRepository vehicleRepository = new VehicleFMRepository();


        const string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;"+
                                          "Initial Catalog = Magazzino;"+
                                          "Integrated Security = true;";

        public void Delete(Motocycle motocycle)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int idVehicleToDelete = GetIdVehicle(motocycle.Id);

                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Connection = connection;

                command.CommandText = "delete from Motocycle where Id = @id";
                command.Parameters.AddWithValue("@id", motocycle.Id);

                command.ExecuteNonQuery();

                vehicleRepository.DeleteById(idVehicleToDelete);

            }
        }

        private int GetIdVehicle(int? id)
        {
            int idVehicleToDelete = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;

                command.CommandText = "select * from Motocycle where Id = @id";
                command.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    idVehicleToDelete = (int)reader["IdVehicle"];
                }

            }
            return idVehicleToDelete;
        }

        public List<Motocycle> Fetch()
        {
            List<Motocycle> motocycles = new List<Motocycle>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Connection = connection;
                command.CommandText = "select Vehicle.Brand, Vehicle.Model, Motocycle.Id, Motocycle.ProductionYear from Vehicle join Motocycle on Vehicle.Id = Motocycle.IdVehicle";

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    var brand = reader["Brand"];
                    var model = reader["Model"];
                    var year = (int)reader["ProductionYear"];
                    var id = (int)reader["Id"];

                    Motocycle motocycle = new Motocycle((string)brand, (string)model, year, id);

                    motocycles.Add(motocycle);
                }
            }
            return motocycles;
        }

        public Motocycle GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Motocycle motocycle)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Vehicle vehicle = new Vehicle(motocycle.Brand, motocycle.Model, null);
                vehicleRepository.Insert(vehicle);
                int idVehicle = vehicleRepository.GetId(vehicle);

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "insert into Motocycle values (@year, @idVehicle)";
                command.Parameters.AddWithValue("@year", motocycle.ProductionYear);
                command.Parameters.AddWithValue("@idVehicle", idVehicle);

                command.ExecuteNonQuery();  
            }
        }

        public void Update(Motocycle motocycle)
        {
            throw new NotImplementedException();
        }
    }
}
