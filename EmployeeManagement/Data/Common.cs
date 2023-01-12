using Microsoft.Azure.Cosmos.Table;

namespace EmployeeManagement.Data;

public  class Common
{   
    public static CloudTableClient GetCloudTableClient()
     {
         if (!CloudStorageAccount.TryParse(Startup.ConnectionStrings?.Default, out CloudStorageAccount clodStorageAccount))
         {
             throw new Exception("ConnStr is invalid");
         }
 
         Console.WriteLine("ConnStr is valid");
 
         return clodStorageAccount.CreateCloudTableClient();
     }
    
    public static async Task<CloudTable> CreateTableAsync(string tableName)
    {
        CloudTable cloudTable = GetCloudTableClient().GetTableReference(tableName);
        
        if (await cloudTable.CreateIfNotExistsAsync())
        {
            Console.WriteLine($"Crated table {tableName}.");
        }
        else
        {
            Console.WriteLine($"Table {tableName} already exists.");
        }

        return cloudTable;
    }

}