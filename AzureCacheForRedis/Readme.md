````
az redis create 
    -g az204 
    -l eastus  
    --name cache16170530 
    --sku Basic 
    --vm-size c0 
    --enable-non-ssl-port
````

Takes a few minutes
````
{
    "accessKeys": null,
    "enableNonSslPort": true,
    "hostName": "cache16170530.redis.cache.windows.net",
    "id": "/subscriptions/03d0ff60-6017-4624-af46-3ff7412ec533/resourceGroups/az204/providers/Microsoft.Cache/Redis/cache16170530",
    "instances": [
        {
            "isMaster": false,
            "nonSslPort": 13000,
            "shardId": null,
            "sslPort": 15000,
            "zone": null
        }
    ],
    "linkedServers": [],
    "location": "East US",
    "minimumTlsVersion": null,
    "name": "cache16170530",
    "port": 6379,
    "provisioningState": "Creating",
        "redisConfiguration": {
        "maxclients": "256",
        "maxfragmentationmemory-reserved": "12",
        "maxmemory-delta": "2",
        "maxmemory-reserved": "2"
    },
    "redisVersion": "4.0.14",
    "replicasPerMaster": null,
    "resourceGroup": "az204",
    "shardCount": null,
    "sku": {
        "capacity": 0,
        "family": "C",
        "name": "Basic"
    },
    "sslPort": 6380,
    "staticIp": null,
    "subnetId": null,
    "tags": {},
    "tenantSettings": {},
    "type": "Microsoft.Cache/Redis",
    "zones": null
}
````

Creates a instance with DNS of `cache16170530.redis.cache.windows.net`

---

Connection string is stored in user secrets

`dotnet user-secrets init`

`dotnet user-sercrets set CacheConnection "cache16170530.redis.cache.windows.net:6380,abortConnect=false,ssl=true,allowAdmin=true
,password=something"`

---
Clean up:
`az redis delete -n cache16170530 -g az204`
