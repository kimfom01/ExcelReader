using ExcelReader.ConsoleTableViewer;
using ExcelReader.Controller;
using ExcelReader.Data;
using ExcelReader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationBuilder builder = new ConfigurationBuilder();

var currentDirectory = Directory.GetCurrentDirectory();
builder.SetBasePath(currentDirectory);
builder.AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();

// var ePPlus = config.GetSection("EPPlus");
// var excelPackage = ePPlus.GetSection("ExcelPackage");
// var licenseContext = excelPackage.GetSection("LicenseContext");

// Console.WriteLine($"{licenseContext.Value}");

var services = new ServiceCollection();
services.AddSingleton(config);
services.AddTransient<IDataAccess, DapperDataAccess>();
services.AddTransient<IReaderService, EPPlusReaderService>();
services.AddTransient<ITableVisualization, ConsoleTableBuilderVisualization>();
services.AddTransient<IReaderAppController, ExcelReaderAppController>();

var serviceProvider = services.BuildServiceProvider();
var startup = serviceProvider.GetService<IReaderAppController>();

await startup!.RunProgram();