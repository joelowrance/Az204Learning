**The namespace is created in ServiceBusSender**

Create the topic 

`az servicebus topic create -n MyTopic --namespace-name sb19010530 -g az204`

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
"createdAt": "2021-05-31T14:45:31.627000+00:00",
"defaultMessageTimeToLive": "10675199 days, 2:48:05.477581",
"duplicateDetectionHistoryTimeWindow": "0:10:00",
"enableBatchedOperations": true,
"enableExpress": false,
"enablePartitioning": false,
"id": "/subscriptions/03d0ff60-6017-4624-af46-3ff7412ec533/resourceGroups/az204/providers/Microsoft.ServiceBus/namespaces/sb19010530/topics/MyTopic",
"location": "East US",
"maxSizeInMegabytes": 1024,
"name": "MyTopic",
"requiresDuplicateDetection": false,
"resourceGroup": "az204",
"sizeInBytes": 0,
"status": "Active",
"subscriptionCount": 0,
"supportOrdering": true,
"type": "Microsoft.ServiceBus/Namespaces/Topics",
"updatedAt": "2021-05-31T14:45:32.663000+00:00"
}
````
create a subscription

`az servicebus topic subscription create -n Sub1 --namespace-name sb19010530 -g az204 --topic-name MyTopic --max-delivery-count 5`

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
"createdAt": "2021-05-31T14:47:39.091086+00:00",
"deadLetteringOnFilterEvaluationExceptions": true,
"deadLetteringOnMessageExpiration": false,
"defaultMessageTimeToLive": "10675199 days, 2:48:05.477581",
"duplicateDetectionHistoryTimeWindow": null,
"enableBatchedOperations": true,
"forwardDeadLetteredMessagesTo": null,
"forwardTo": null,
"id": "/subscriptions/03d0ff60-6017-4624-af46-3ff7412ec533/resourceGroups/az204/providers/Microsoft.ServiceBus/namespaces/sb19010530/topics/MyTopic/subscriptions/Sub1",
"location": "East US",
"lockDuration": "0:01:00",
"maxDeliveryCount": 5,
"messageCount": 0,
"name": "Sub1",
"requiresSession": false,
"resourceGroup": "az204",
"status": "Active",
"type": "Microsoft.ServiceBus/Namespaces/Topics/Subscriptions",
"updatedAt": "2021-05-31T14:47:39.091086+00:00"
}
````