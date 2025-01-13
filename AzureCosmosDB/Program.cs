using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.Write("hello world");
    }

    /// <summary>
    /// Database operations for a single db instance on a db granular level. 
    /// </summary>
    /// <returns></returns>
    private static async Task databaseOperations()
    {
        string endpoint = "";
        string authKey = "";
        //Init cosmos client. 
        CosmosClient client = new CosmosClient(endpoint, authKey);

        Database database = await client.CreateDatabaseIfNotExistsAsync(id: "testDatabase");
        //Read a database by ID. 
        DatabaseResponse readResponse = await database.ReadAsync();

        //delete a database. 
        await database.DeleteAsync();
    }

    private async Task containerDatabaseOperations()
    {
        string endpoint = "";
        string authKey = "";
        //Init cosmos client. 
        CosmosClient client = new CosmosClient(endpoint, authKey);

        Database database = await client.CreateDatabaseIfNotExistsAsync(id: "testDatabase");

        // Init the databse. 
        string containerID = "";
        string partitionKey = "";
        int throughput = 400;
        ContainerResponse simplerContainer = await database.CreateContainerIfNotExistsAsync(
            id: containerID,
            partitionKeyPath: partitionKey,
            throughput: throughput);

        // Get the container. 
        Container container = database.GetContainer(containerID);
        ContainerProperties containerProperties = await container.ReadContainerAsync();

        // Perform creates. 
        SalesOrder order = new SalesOrder { Name = "Test Order" };
        ItemResponse<SalesOrder> createResponse = await container.CreateItemAsync(order, new PartitionKey(order.AccountNumber));


        // Perform Reads. 
        string id = "[orderID]";
        string accountNumber = order.AccountNumber;
        ItemResponse<SalesOrder> readResponse = await container.ReadItemAsync<SalesOrder>(id, new PartitionKey(accountNumber));

        // Query an item. 
        QueryDefinition query = new QueryDefinition(
        "select * from sales s where s.AccountNumber = @AccountParam ").WithParameter("@AccountParam", "mainAccount");

        FeedIterator<SalesOrder> resultSet = container.GetItemQueryIterator<SalesOrder>(
            query,
            requestOptions: new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey("mainAccount"),
                MaxItemCount = 1
            });
        // Delete a container. 
        await database.GetContainer(containerID).DeleteContainerAsync();
    }

    private class SalesOrder
    {
        public string Name { get; set; } = "default";
        public string AccountNumber { get; set; } = "mainAccount";
    }
}