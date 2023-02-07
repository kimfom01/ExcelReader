using ExcelReader.ConsoleTableViewer;
using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Services;

namespace ExcelReader.Controller;

public class ExcelReaderAppController : IReaderAppController
{
    private readonly IDataAccess _dataAccess;
    private readonly IReaderService _readerService;
    private readonly ITableVisualization _tableVisualization;

    public ExcelReaderAppController(IDataAccess dataAccess, IReaderService readerService, ITableVisualization tableVisualization)
    {
        _dataAccess = dataAccess;
        _readerService = readerService;
        _tableVisualization = tableVisualization;
    }

    public async Task RunProgram()
    {
        _dataAccess.SetupDatabase();

        var productsFromExcel = await ReadFromExcelAsync();

        await SaveToDatabaseAsync(productsFromExcel);

        var productsFromDb = await ReadFromDatabaseAsync();

        DisplayDataOnConsole(productsFromDb);

        Console.WriteLine("Exiting app...");
        Thread.Sleep(3000);
    }

    private void DisplayDataOnConsole(List<Product> productsFromDb)
    {
        Console.WriteLine("Preparing to display data from database...");
        _tableVisualization.DisplayProductsTable(productsFromDb);
        Console.WriteLine("Done");
    }

    private async Task<List<Product>> ReadFromDatabaseAsync()
    {
        Console.WriteLine("Reading from database...");
        var productsFromDb = await _dataAccess.GetProductsAsync();
        Console.WriteLine("Done");
        return productsFromDb;
    }

    private async Task SaveToDatabaseAsync(List<Product> productsFromExcel)
    {
        Console.WriteLine("Saving data to the database...");
        await _dataAccess.AddProducts(productsFromExcel);
        Console.WriteLine("Done");
    }

    private async Task<List<Product>> ReadFromExcelAsync()
    {
        Console.WriteLine("Reading from the excel file...");
        var productsFromExcel = await _readerService.LoadProductsFromExcelAsync();
        Console.WriteLine("Done");

        return productsFromExcel;
    }
}
