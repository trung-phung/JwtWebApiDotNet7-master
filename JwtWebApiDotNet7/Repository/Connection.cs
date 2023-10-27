// using System.Data.SqlClient;
// using MySqlConnector;
// namespace JwtWebApiDotNet7.Repository;


// public class Connection
// {
//     public  Task? ConnectAsync()
//     {
//         var builder = new MySqlConnectionStringBuilder
//         {
//             Server = "localhost:3306",
//             UserID = "root",
//             Password = "a",
//             Database = "PROJECT3_BOUQUET",
//         };
//         using var connection = new MySqlConnection(builder.ConnectionString);
//         connection.OpenAsync();
//         using var command = connection.CreateCommand();
//         command.CommandText = @"SELECT * FROM BOUQUET";
//         using var reader =  command.ExecuteReader();
//         while (reader.Read())
//         {
//             var id = reader.GetInt32("Bouquet_ID");
//             var date = reader.GetString("Name");
//             var price = reader.GetInt32("Price");
//         }
//         return null;
//     }
// }