﻿using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;

//https://learn.microsoft.com/en-us/azure/cosmos-db/mongodb/quickstart-dotnet

//1. New instance of CosmosClient class
//string connectionString = @"mongodb://mymongodbaccount:rEhKZAX15zCV6AMGLhcVkdYGngNIHiWa7M8z5cf9MgSSvgEWilqBSNM4FwOwFLRGypHKJslCWn8mACDbZVQTHw==@mymongodbaccount.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@mymongodbaccount@";

string connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION");

MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

var MongoDBclient = new MongoClient(settings);

//2. Database reference with creation if it does not already exist
var db = MongoDBclient.GetDatabase("adventure");

//3. Container reference with creation if it does not alredy exist
var _products = db.GetCollection<Product>("products");

//4. Create new object and upsert (create or replace) to container
_products.InsertOne(new Product(
    Guid.NewGuid().ToString(),
    "gear-surf-surfboards",
    "Yamba Surfboard", 
    12, 
    false
));

//5. Read a single item from container
var product = (await _products.FindAsync(p => p.Name.Contains("Yamba"))).FirstOrDefault();
Console.WriteLine("Single product:");
Console.WriteLine(product.Name);

//6. Read multiple items from container
_products.InsertOne(new Product(
    Guid.NewGuid().ToString(),
    "gear-surf-surfboards",
    "Sand Surfboard",
    4,
    false
));

var products = _products.AsQueryable().Where(p => p.Category == "gear-surf-surfboards");

Console.WriteLine("Multiple products:");
foreach (var prod in products)
{
    Console.WriteLine(prod.Name);
}

//7. We define the "Product" class 

public record Product(
    string Id,
    string Category,
    string Name,
    int Quantity,
    bool Sale
);