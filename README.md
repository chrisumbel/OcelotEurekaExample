OcelotEureakExample
===================
[Ocelot](https://github.com/ThreeMammals/Ocelot) is a .NET Core API gateway 
similar to Zuul. 

This project is an example of using the Ocelot API Gateway, 
[Pivotal Cloud Foundry](https://pivotal.io/platform), and the 
[Eureka Service Registry](https://github.com/Netflix/eureka) via Pivotal's 
[Service Discovery Tile](https://docs.pivotal.io/spring-cloud-services/1-2/service-registry/).

This solution contains two projects: The *Gateway* project which is the 
[Ocelot](https://github.com/ThreeMammals/Ocelot)
gateway and *BackingService* which is a simple WebAPI webserive that registers
with eurka such that it can be fronted by Ocelot.  

Configuring Ocelot
---------------------------

Ocelot's configuration is defined in `ocelot.json` in the *Gateway* project,
as directed in *Gateway*'s Program.cs (`.AddJsonFile("ocelot.json")`). 

Note that this maps its root `/` to the backing service's `/api/values`
endpoint. Further it binds the gateway's Route to a service named 
`backingService` which is expected to be regiestered with Eureka.


The `GlobalConfiguration.ServiceDiscoveryProvider` directive tells Ocelot to
use Eureka for service discovery, but other providers are supported as well
such as Consul.

```
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/api/values",
      "UpstreamPathTemplate": "/",
      "UpstreamHttpMethod": [
        "Get"
      ],
      "DownstreamScheme": "https",
      "ServiceName": "backingService",
      "LoadBalancerOptions": {
        "Type": "LeastConnection"
      }
    }
  ],
  "GlobalConfiguration": {
    "RequestIdKey": "OcRequestId",
    "AdministrationPath": "/administration",
    "ServiceDiscoveryProvider": {
      "Type": "Eureka"
    }
  }
}
```

Buildind and running the example
================================

Building
-----

Build and publish the backing service in OcelotEureakExample/BackingService
```
dotnet publish -r ubuntu.14.04-x64 -o publish
```

Build and publish the gateway in OcelotEureakExample/Gateway
```
dotnet publish -r ubuntu.14.04-x64 -o publish
```

Create Discovery Service in Cloud Foundry
-----------------------------------------

```
cf create-service p-service-registry standard ocelot-service-registry
```

Push to PCF
-----------
First push the backing service from OcelotEureakExample/BackingService:
```
cf push
```

Then push the gateway from OcelotEureakExample/Gateway
```
cf push

```

Finally, go to the Gateway's URL and you should see the backing services 
response proxied out of the gateway:

 ```
 [
"value1",
"value2"
]
``` 