using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using climatobservations.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace climatobservations.Repositories
{
    internal class DbRepository
    {

        private string _connectionString;


        public DbRepository()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<DbRepository>()
                         .Build();

            _connectionString = config.GetConnectionString("dbConn");
        }


        public void AddObserver(Observer observer) 
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                StringBuilder sql = new StringBuilder("insert into observer ");
                sql.AppendLine("(firstname, lastname) ");
                sql.AppendLine("values (@firstname,@lastname) ");

                using var command = new NpgsqlCommand(sql.ToString(), conn);
                command.Parameters.AddWithValue("firstname", observer.Firstname);
                command.Parameters.AddWithValue("lastname", observer.Lastname);
                command.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState != "23505") // Felkoden 23505: användare redan finns pga sammansatt nyckel. Ingeting händer om firstname och lastname redan finns i tabellen. 
                {
                    throw new Exception("Allvarligt fel", ex);  
                } 
            }
        }

        public Observation GetObservationByObserverId(Observer observer) // Metod som hämtar observationens id från observer id
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            StringBuilder sql = new StringBuilder("select ");
            sql.AppendLine("id, date, observer_id, geolocation_id ");
            sql.AppendLine("from observation ");
            sql.AppendLine("where observer_id =@observer_id "); 

            using var command = new NpgsqlCommand(sql.ToString(), conn); 
            command.Parameters.AddWithValue("observer_id", observer.Id); 

            Observation? observation = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    observation = new Observation()
                    {
                        Id = reader.GetInt32(0),
                        Date = (DateTime)reader["date"],
                        Observer_Id = (int)reader["observer_id"],
                        Geolocation_Id = (int)reader["geolocation_id"]

                    };

                }
            }
            return observation;
        }

        public List<Observer> GetObserver() 
        {           
                var observers = new List<Observer>(); // Skapa lista 
                using var conn = new NpgsqlConnection(_connectionString); // Skapar databasobjekt
                conn.Open();                                              // Kopplar upp mot databas
                Observer observer = null;
                using var cmd = new NpgsqlCommand();
                cmd.CommandText = "select * from observer"; // Tar in samtliga observatörer, hade kunnat effektiviserats genom hämta via id eller namn
                cmd.Connection = conn;


            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    observer = new Observer()
                    {
                        Id = reader.GetInt32(0),
                        Firstname = (string?)reader["firstname"],
                        Lastname = reader["lastname"] == DBNull.Value ? null : (string?)reader["lastname"] // Sista stycket efter == Om värdet är null så kraschar det inte 

                    };
                    observers.Add(observer); // Lägger till observers i lista
                }
                return observers;
            }

         }

        public void RemoveObserver(Observer observer)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                StringBuilder sql = new StringBuilder("delete from observer ");
                sql.AppendLine("where firstname =@firstname AND  lastname =@lastname");

                using var command = new NpgsqlCommand(sql.ToString(), conn);
                command.Parameters.AddWithValue("firstname", observer.Firstname);
                command.Parameters.AddWithValue("lastname", observer.Lastname);
                command.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23503") // Kan inte ta bort en observatör som gjort observationer 
                {
                    throw ex;
                }
                else
                {
                    throw new Exception("Allvarligt fel", ex);
                }
            }
        }



        public void AddMeasurement(Observation observation, Category category, decimal value)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                StringBuilder sql = new StringBuilder("insert into measurement ");
                sql.AppendLine("(value, observation_id, category_id) ");
                sql.AppendLine("values (@value, @observation_id, @category_id) ");

                using var command = new NpgsqlCommand(sql.ToString(), conn);
                command.Parameters.AddWithValue("value", value);
                command.Parameters.AddWithValue("observation_id", observation.Id);
                command.Parameters.AddWithValue("category_id", category.Id);

                command.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23505") // Uppdaterar measurement då en observatör endast kan ha en observation (datum).  
                {
                    using var conn = new NpgsqlConnection(_connectionString);
                    conn.Open();

                    StringBuilder sql = new StringBuilder("update measurement ");
                    sql.AppendLine("set value = @value ");
                    sql.AppendLine("where observation_id = @observation_id and category_id = @category_id ");

                    using var command = new NpgsqlCommand(sql.ToString(), conn);
                    command.Parameters.AddWithValue("value", value);
                    command.Parameters.AddWithValue("observation_id", observation.Id);
                    command.Parameters.AddWithValue("category_id", category.Id);

                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Allvarligt fel", ex);
                }
            }


        }


        public List<Measurement> GetMeasurementByObservationId(Observation observation)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            StringBuilder sql = new StringBuilder("select ");
            sql.AppendLine("id, value, observation_id, category_id ");
            sql.AppendLine("from measurement ");
            sql.AppendLine("where observation_id =@observation_id ");

            using var command = new NpgsqlCommand(sql.ToString(), conn);
            command.Parameters.AddWithValue("observation_id", observation.Id);

            List<Measurement> measurements = new List<Measurement>();
            Measurement measurement = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    measurement = new Measurement()
                    {
                        Id = reader.GetInt32(0),
                        Value = (float)reader["value"],
                        Observation_Id = (int)reader["observation_id"],
                        Category_Id = (int)reader["category_id"]
                    };
                    
                    measurements.Add(measurement);
                }
            }
            return measurements; 
        }


        public void AddObservation(Observer selectedObserver, DateTime date) 
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            StringBuilder sql = new StringBuilder("insert into observation ");
            sql.AppendLine("(date, observer_id, geolocation_id) ");
            sql.AppendLine("values (@date ,@observer_id, 4) "); // Geolocation Id är alltid densamma "4" Hagaparken.

            using var command = new NpgsqlCommand(sql.ToString(), conn);
            command.Parameters.AddWithValue("date", date);
            command.Parameters.AddWithValue("observer_id", selectedObserver.Id);

            command.ExecuteNonQuery();
        }



        public List<Category> GetCategory() // Listmetod
        {
            {
                var categories = new List<Category>(); // Skapa lista 
                using var conn = new NpgsqlConnection(_connectionString); // Skapar databasobjekt
                conn.Open();                                              // Kopplar upp mot databas
                Category category = null;
                using var cmd = new NpgsqlCommand();
                cmd.CommandText = "select * from category";
                cmd.Connection = conn;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        category = new Category()
                        {
                            Id = reader.GetInt32(0),
                            Name = (string)reader["name"],
                            Basecategory_Id = reader["basecategory_id"] == DBNull.Value ? null : (int)reader["basecategory_id"] // Sista stycket efter == Om värdet är null så kraschar det inte 

                        };
                        categories.Add(category); // Lägger till observers i lista
                    }
                    return categories;
                }

            }

        }
    }
}
