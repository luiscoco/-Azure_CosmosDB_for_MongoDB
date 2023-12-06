# Azure CosmosDB for MongoDB

https://learn.microsoft.com/en-us/azure/cosmos-db/mongodb/quickstart-dotnet

![image](https://github.com/luiscoco/-Azure_CosmosDB_for_MongoDB/assets/32194879/ddb3991f-8002-48c5-aa2e-3b5d449cfa27)

# 1. Prerequisites

## 1.1. Install .NET 8 SDK 

For installing .NET 8 navigate to the URL: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer

To check the installation was successful run the command:

```
dotnet --list-sdks
```

## 1.2. Install the Azure CLI or Azure PowerShell

Navigate to the following URL to install Azure CLI: https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli

Navigate to the following URL to install Azure PowerShell: https://learn.microsoft.com/en-us/powershell/azure/install-azure-powershell?view=azps-11.1.0

Check the installation was succesful running the commands:

```
az --version 
```

```
Get-Module -ListAvailable AzureRM
```

# 2. Create an Azure CosmosDB account

```csharp
﻿using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;

//1. Obtaine Azure credentials
TokenCredential cred = new DefaultAzureCredential();

//2. Azure Authentication
ArmClient client = new ArmClient(cred);

//3. Set your Azure subscription number
string subscriptionId = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

//4. Set the ResourceGroup name where to create the new Azure CosmosDB account
//It is mandatory to create this ResourceGroup before creating the ComosDB account
string resourceGroupName = "rg1";

//5. Create the ResourceGroup Identifier
ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);

//6. Get the ResourceGroup
ResourceGroupResource resourceGroupResource = client.GetResourceGroupResource(resourceGroupResourceId);

//7. Get the collection of this CosmosDBAccountResource
CosmosDBAccountCollection collection = resourceGroupResource.GetCosmosDBAccounts();

//8. Set the data input for creating the CosmosDB account: accountName, account location, etc
string accountName = "newcosmosdbwithazuresdk";
CosmosDBAccountCreateOrUpdateContent content = new CosmosDBAccountCreateOrUpdateContent(new AzureLocation("westeurope"), new CosmosDBAccountLocation[]
{
new CosmosDBAccountLocation()
{
LocationName = new AzureLocation("westeurope"),
FailoverPriority = 0,
IsZoneRedundant = false,
}
})
{
    CreateMode = CosmosDBAccountCreateMode.Default,
};

//9. Create/Update a CosmosDB account
ArmOperation<CosmosDBAccountResource> lro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, accountName, content);

//10. Get the ComosDB Account Resource
CosmosDBAccountResource result = lro.Value;

//11. Get the CosmosDB Account Data
CosmosDBAccountData resourceData = result.Data;

//12. We print the new CosmosDB account Id
Console.WriteLine($"Succeeded on id: {resourceData.Id}");
```


# 3. Create a MongoDB



