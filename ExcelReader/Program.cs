using Microsoft.Extensions.Configuration;

IConfigurationBuilder builder = new ConfigurationBuilder();

var currentDirectory = Directory.GetCurrentDirectory();
builder.SetBasePath(currentDirectory);
builder.AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();

var ePPlus = config.GetSection("EPPlus");
var excelPackage = ePPlus.GetSection("ExcelPackage");
var licenseContext = excelPackage.GetSection("LicenseContext");

Console.WriteLine($"{licenseContext.Value}");