using EmployeeManagement.Data.Entities;
using Microsoft.Azure.Cosmos.Table;

namespace EmployeeManagement.Data.Operations;

public class Operations
{
    public async Task TriggerOperations()
    {
        const string tableName = "Employee";

        CloudTable cloudTable = await Common.CreateTableAsync(tableName);

        try
        {
            // await AddDataOperations(cloudTable);
            // await UpdateDataOperations(cloudTable);
            // RetrieveAllDataOperation(cloudTable);
            // RetrieveSingleDataOperationV1(cloudTable, "Consulting",  "lauri-saar-rk");
            // await RetrieveSingleDataOperationV2(cloudTable, "Consulting",  "lauri-saar-rk");
           await DeleteOperation(cloudTable, await RetrieveSingleDataOperationV2(cloudTable, "Consulting",  "lauri-saar-rk"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;  
        }
    }

    private static void RetrieveAllDataOperation(CloudTable cloudTable)
    {
        IEnumerable<EmployeeEntity>? allDataQuery = cloudTable.ExecuteQuery(new TableQuery<EmployeeEntity>());

        foreach (EmployeeEntity entity in allDataQuery)
        {
            Console.WriteLine($"RowKey: {entity.RowKey}, PartitionKey: {entity.PartitionKey}, FullName: {entity.FullName}");
        }
    }
    
    private static void RetrieveSingleDataOperationV1(CloudTable cloudTable, string partKey, string rowKey)
    {
        EmployeeEntity? singleDataQuery = cloudTable
            .CreateQuery<EmployeeEntity>()
            .Where(e => e.PartitionKey == partKey && e.RowKey ==rowKey)
            .Select(x=>x)
            .Single();

            Console.WriteLine($"V1: RowKey: {singleDataQuery.RowKey}, PartitionKey: {singleDataQuery.PartitionKey}, FullName: {singleDataQuery.FullName}");
    }
    
    private static async Task<EmployeeEntity?> RetrieveSingleDataOperationV2(CloudTable cloudTable, string partKey, string rowKey)
    {
        TableOperation retSingleOp = TableOperation.Retrieve<EmployeeEntity>(partKey, rowKey);
        TableResult retSingleResult = await cloudTable.ExecuteAsync(retSingleOp);

        if (retSingleResult.Result is EmployeeEntity entity)
        {
            Console.WriteLine($"V2: RowKey: {entity.RowKey}, PartitionKey: {entity.PartitionKey}, FullName: {entity.FullName}");
            return entity;
        }
        else
        {
            Console.WriteLine("Not found.");
            return null;
        }
    }

    public static async Task AddDataOperations(CloudTable cloudTable)
    {
        EmployeeEntity newEmployeeEntity = new("Consulting")
        {
            FullName = "Lauri Saar",
            Email = "lauri.saar@outlook.com",
            RowKey = "lauri-saar-rk"
        };
        
        TableOperation addOp = TableOperation.InsertOrMerge(newEmployeeEntity);
        TableResult addResult = await cloudTable.ExecuteAsync(addOp);
        EmployeeEntity addResponse = addResult.Result as EmployeeEntity;

        if (addResult.RequestCharge.HasValue)
        {
            Console.WriteLine($@"addResponse => {addResponse.PartitionKey}, {addResponse.RowKey}, {addResponse.FullName} ");
        }
    }
    
    private static async Task UpdateDataOperations(CloudTable cloudTable)
    {
        EmployeeEntity newEmployeeEntity = new("Consulting")
        {
            FullName = "Lauri Saar New",
            Email = "lauri.saar@outlook.com",
            RowKey = "lauri-saar-rk-new"
        };
        
        TableOperation addOp = TableOperation.InsertOrMerge(newEmployeeEntity);
        TableResult addResult = await cloudTable.ExecuteAsync(addOp);
        EmployeeEntity addResponse = addResult.Result as EmployeeEntity;

       if (addResult.RequestCharge.HasValue)
        {
            Console.WriteLine($@"addResponse => {addResponse.PartitionKey}, {addResponse.RowKey}, {addResponse.FullName} ");
        }
        
        newEmployeeEntity.FullName = "Updated Full Name";
        
        TableOperation updOp = TableOperation.InsertOrMerge(newEmployeeEntity);
        TableResult updResult = await cloudTable.ExecuteAsync(updOp);
        EmployeeEntity updResponse = updResult.Result as EmployeeEntity;
        
        if (updResult.RequestCharge.HasValue)
        {
            Console.WriteLine($@"updResponse => {updResponse.PartitionKey}, {updResponse.RowKey}, {updResponse.FullName} ");
        }
    }
    
    private static async Task DeleteOperation(CloudTable cloudTable, EmployeeEntity entity)
    {
        TableOperation delOp = TableOperation.Delete(entity );
        TableResult delResult = await cloudTable.ExecuteAsync(delOp);

        if (delResult.RequestCharge is double reqCrg)
        {
            Console.WriteLine($"Delete succeeds, req.crg: ${reqCrg}");
        }
        else
        {
            Console.WriteLine("Delete failed");
        }
    }
}