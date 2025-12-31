using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


class Program
{
    private static string password = "abc123";
    private static string _apiKey = "sk_live_abc123secret";

    static void Main(string[] args)
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        SearchUser(username);
        HashData("test");
    }

    static void SearchUser(string username)
    {
        // BAD: SQL injection vulnerability
        string connectionString = "Server=localhost;Database=test;";
        string query = "SELECT * FROM Users WHERE Username = '" + username + "'";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(query, connection);
        // This would execute the unsafe query
    }

    static byte[] HashData(string input)
    {
        // BAD: Weak cryptographic algorithm
        using var md5 = MD5.Create();
        return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
}