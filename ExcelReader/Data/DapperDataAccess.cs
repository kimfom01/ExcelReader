using System.Data;
using Dapper;
using ExcelReader.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ExcelReader.Data;

public class DapperDataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private string _connectionString;

    public DapperDataAccess(IConfiguration config)
    {
        _config = config;
        InitializeConnectionString();
    }

    private void InitializeConnectionString()
    {
        _connectionString = _config.GetConnectionString("ExcelDb");
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        var products = await connection.QueryAsync<Product>("dbo.Products_Select");

        return products.ToList();
    }

    public async Task AddProducts(List<Product> products)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync("dbo.Products_Insert @ProductId @ProductName @Supplier @ProductCost", products);
    }

    public void SetupDatabase()
    {
        var databaseConnection = _config.GetConnectionString("DatabaseConnection");
        var file = new FileInfo(_config.GetSection("SetupScript").Value!);
        var script = file.OpenText().ReadToEnd();

        using var connection = new SqlConnection(databaseConnection);

        var serverConnection = new ServerConnection(connection);

        var server = new Server(serverConnection);

        server.ConnectionContext.ExecuteNonQuery(script);
    }
}
