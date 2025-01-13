using System;
using Azure.Storage.Blobs;
using Azure.Identity;
using System.Reflection.Metadata;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string accountName = "defaultAccountName";
        getBlobServiceClient(accountName);
    }

    /// <summary>
    /// Create a BlobServiceClient object. 
    /// </summary>
    /// <param name="accountName"></param>
    /// <returns></returns>
    private static BlobServiceClient getBlobServiceClient(string accountName)
    {
        BlobServiceClient client = new(
            new Uri($"https://{accountName}.blob.core.windows.net"),
            new DefaultAzureCredential());

        return client;
    }

    /// <summary>
    /// Create container client based on the <paramref name="serviceClient"/>
    /// </summary>
    /// <param name="serviceClient"></param>
    /// <param name="containerName"></param>
    /// <returns></returns>
    private static BlobContainerClient getBlobClientContainer(BlobServiceClient serviceClient, string containerName)
    {
        BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(containerName);
        return containerClient;
    }

    /// <summary>
    /// Create a BlobContainerClient object directly without using BlobServiceClient.
    /// </summary>
    /// <param name="accountName"></param>
    /// <param name="containerName"></param>
    /// <returns></returns>
    private static BlobContainerClient getBlobContainerWithCreds(string accountName, string containerName)
    {
        BlobContainerClient client = new(
            new Uri($"https://{accountName}.blob.core.windows.net/{containerName}"),
            new DefaultAzureCredential(),
            new BlobClientOptions());
        return client;
    }

    /// <summary>
    /// A BlobClient object allows you to interact with a specific blob resource.
    /// </summary>
    /// <param name="serviceClient"></param>
    /// <param name="containerName"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    private static BlobClient getBlobClient(BlobServiceClient serviceClient, string containerName, string blobName)
    {
        BlobClient blobClient = serviceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return blobClient;
    }
}