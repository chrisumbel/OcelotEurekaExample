OcelotEureakExample
===================

This project is an example of using the Ocelot API Gateway, 
Pivotal Cloud Foundry, and the 
[Eureka Service Registry](https://github.com/Netflix/eureka) via Pivotal's 
[Service Discovery Tile](https://docs.pivotal.io/spring-cloud-services/1-2/service-registry/).

This solution contains two projects: The *Gateway* project which is the Ocelot
gateway and *BackingService* which is a simple WebAPI webserive that registers
with eurka such that it can be fronted by Ocelot.  

Ocelot 
------
[Ocelot](https://github.com/ThreeMammals/Ocelot) is a .NET Core API gateway 
similar to Zuul. 

Build
-----

Build and publish the gateway OcelotEureakExample/BackingService
```
dotnet publish -r ubuntu.14.04-x64 -o publish
```

Build and publish the gateway OcelotEureakExample/Gateway
```
dotnet publish -r ubuntu.14.04-x64 -o publish
```

Create Discovery Service
------------------------

```
cf create-service p-service-registry standard ocelot-service-registry
```

Push to PCF
----
First push tbe backing service from OcelotEureakExample/BackingService:
```
cf push
```

Then push tbe backing service from OcelotEureakExample/Gateway
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

Note that the gateway maps its root "/" to the backing service's /api/values
endpoint.