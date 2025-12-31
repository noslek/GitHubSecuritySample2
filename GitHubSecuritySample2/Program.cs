using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

class Program
{
    private static string _apiKey = "sk_live_abc123secret";

    static void Main(string[] args)
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();  // User-controlled input

        Console.Write("Enter password: ");
        string password = Console.ReadLine();  // User-controlled input

        SearchUser(username, password);
        HashData("test");

        var app = WebApplication.Create(args);

        app.MapGet("/download", (string filename) =>
        {
            // BAD: Path traversal - user controls file path
            var path = Path.Combine("/var/www/files", filename);
            return Results.File(System.IO.File.ReadAllBytes(path));
        });

        app.Run();
    }

    static void SearchUser(string username, string pw)
    {
        string connectionString = "Server=localhost;Database=test;";
        string query = "SELECT * FROM Users WHERE Username = '" + username + "' and Password = '" + pw + "'";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(query, connection);
    }

    static byte[] HashData(string input)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
}