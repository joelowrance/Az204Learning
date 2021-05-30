Namespace needs to exist first 

` az servicebus namespace create -g az204 -l eastus -n sb19010530 --sku Standard`

````
{
    "createdAt": "2021-05-30T23:03:37.110000+00:00",
    "id": "/subscriptions/03d0ff60-6017-4624-af46-3ff7412ec533/resourceGroups/az204/providers/Microsoft.ServiceBus/namespaces/sb19010530",
    "location": "East US",
    "metricId": "03d0ff60-6017-4624-af46-3ff7412ec533:sb19010530",
    "name": "sb19010530",
    "provisioningState": "Succeeded",
    "resourceGroup": "az204",
    "serviceBusEndpoint": "https://sb19010530.servicebus.windows.net:443/",
    "sku": {
        "capacity": null,
        "name": "Standard",
        "tier": "Standard"
    },
    "tags": {},
    "type": "Microsoft.ServiceBus/Namespaces",
    "updatedAt": "2021-05-30T23:04:18.867000+00:00"
}
````

Then can add a queue

`az servicebus queue create -g az204 --namespace-name sb19010530 --name mainqueue`

````
{
  "accessedAt": "0001-01-01T00:00:00",
  "autoDeleteOnIdle": "10675199 days, 2:48:05.477581",
  "countDetails": {
    "activeMessageCount": 0,
    "deadLetterMessageCount": 0,
    "scheduledMessageCount": 0,
    "transferDeadLetterMessageCount": 0,
    "transferMessageCount": 0
  },
  "createdAt": "2021-05-30T23:08:09.707000+00:00",
  "deadLetteringOnMessageExpiration": false,
  "defaultMessageTimeToLive": "10675199 days, 2:48:05.477581",
  "duplicateDetectionHistoryTimeWindow": "0:10:00",
  "enableBatchedOperations": true,
  "enableExpress": false,
  "enablePartitioning": false,
  "forwardDeadLetteredMessagesTo": null,
  "forwardTo": null,
  "id": "/subscriptions/03d0ff60-6017-4624-af46-3ff7412ec533/resourceGroups/az204/providers/Microsoft.ServiceBus/namespaces/sb19010530/queues/mainqueue",
  "location": "East US",
  "lockDuration": "0:01:00",
  "maxDeliveryCount": 10,
  "maxSizeInMegabytes": 1024,
  "messageCount": 0,
  "name": "mainqueue",
  "requiresDuplicateDetection": false,
  "requiresSession": false,
  "resourceGroup": "az204",
  "sizeInBytes": 0,
  "status": "Active",
  "type": "Microsoft.ServiceBus/Namespaces/Queues",
  "updatedAt": "2021-05-30T23:08:09.783000+00:00"
}
````

On the queue, Shared Access Policy, create new policy.
Will create a connection string like this.

`Endpoint=sb://sb19010530.servicebus.windows.net/;SharedAccessKeyName=Send;SharedAccessKey=yay;EntityPath=mainqueue`

this is stored in the secrets file (dotnet user-secrets init)

Clean up:

`az servicebus namespace delete -g az204 -n sb19010530`



