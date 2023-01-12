// See https://aka.ms/new-console-template for more information

using EmployeeManagement.Data;
using EmployeeManagement.Data.Operations;

Console.WriteLine("Hello, World!");

// Common.CreateTableAsync("Employee").GetAwaiter().GetResult();

Operations dataOps = new Operations();
await dataOps.TriggerOperations();

Console.ReadKey();

