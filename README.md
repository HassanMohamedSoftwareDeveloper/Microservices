# Microservice Architecture
The microservice architecture breaks software into smaller pieces that can be independently deployed, scaled, and replaced.

## Benefits
### 1. Team Autonomy
+ Development teams can have autonomy from other teams. 
    > Allows teams to develop and deploy at a pace different than others.
+ Using different languages, allowing flexibility in fitting the tool to the task at hand.
+ Each team only hold the responsibility for their services.
    > Focus on their code without the need to know details of code in other areas.

### 2. Service Autonomy
+ Separating concerns at the service layer.
    > The “Single Responsibility Principle” applies here as well.
    
    > No microservice should have more than one reason to change.
+ Evolve independently.
    > Because having a microservice dedicated to specific business processes.
+ Loose coupling between microservices.
    > Upgrading microservices is easier and has less impact on other services.
    
    > Allows for features and business processes to evolve at different paces
+ Individual resiliency and availability needs.
+ Deployment
    > Release separately using (CI/CD).
    
    > Allows frequent releases with minimal, if any, impact on other services. 
    
    > Separate deployment frequency and complexity than with monolithic applications.
    
    > **Zero downtime**,Can configure a deployment strategy to bring up an updated version before taking down existing services.

### 3. Scalability
+ Allows for the number of instances of services to differentiate between other services and a monolithic application.
+ Lets multiple instances reside on the same server or across multiple servers, which aids in fault isolation.
+ Leverage servers of diverse sizes.
    > One microservice may need more CPU than RAM, while others require more in-memory processing capabilities.
+ The diversity of programming languages
    > Assuming the monolith runs .NET Framework, you can write microservices in other programming languages. If these languages can run on Linux, then you have the potential of saving money due to the operating system license cost.

### 4. Fault Isolation
Handling failures without them taking down an entire system.
> Several things can cause failure:
+ Coding or data issues
+ Extreme CPU and RAM utilization
+ Network
+ Server hardware
+ Downstream systems
> With a microservice architecture, services with any of the preceding conditions will not take down other parts of the system.

### 5. Data Autonomy
+ Having data isolated per microservice allows independent changes to occur with minimal impact on others.
+ Encourages quicker time to production for the business. 
+ With separate databases, you also get the benefit of using differing data store technologies.
+ Provides an opportunity for some data to be in a relational database like SQL Server while others are in non-relational databases like MongoDB.

## Challenges to Consider
> Migrating to a microservice architecture is not pain-free and is more complex than monoliths. 

> Developing microservices requires a new way of thinking about the existing architecture, such as the cost of development time and infrastructure changes to networks and servers.

> How microservices communicate, handling failures, dependencies, and data consistency.

> More than code to consider.

+ Accessibility
+ Obtaining configuration information
+ Messaging
+ Service discovery

## Microservice Patterns
Every microservice architecture has challenges such as accessibility, obtaining configuration information, messaging, and service discovery.
There are common solutions to these challenges called patterns. 
Various patterns exist to help solve these challenges and make the architecture solid.

### 1. API Gateway/BFF
+ <code>API Gateway pattern</code> provides a single endpoint for client applications to the microservices assigned to it.
+ <code>API Gateway pattern</code> Provides functionality such as routing to microservices, authentication, and load balancing.
![Single API Gateway access point!](/Images/1.png "Single API Gateway access point")

#### Problems / Chalenges 
<code>API Gateway pattern</code> may cause a problem :
The number of client applications may increase. 
The demands from those client applications may grow. 

#### Solving 
Separation should be done to split client applications apart by using <code>multiple API Gateways</code>.

<code>BFF (Backends for Frontends)</code> design pattern helps with this segregation.
    > There are multiple endpoints, but they are designated based on the types of clients being served.

![Designated API Gateway endpoints!](/Images/2.png "Designated API Gateway endpoints")

> Mobile clients usually do not get/need all the content compared to a full website.

> Using the <code>BFF</code> pattern with <code>API Gateways</code> allows for that separation of handling the differences.

#### There are precautions (احتياطات) when using an API Gateway pattern.
+ Must be maintained so there is not too much coupling
+ Should not be more responsible than necessary.
+ There may be a point at which multiple API Gateways are created, and microservices split between them.This would help with another precaution where the API Gateway can be a bottleneck and may add to any latency issues.

### 2. External Configuration Store
> With the ability to have many microservices instances, it would be impractical  for each instance to have its own configuration files. Updating information across all running instances would be overwhelming.

> Using the <code>External Configuration Store pattern</code> provides a common area to store configuration information.
This means there is one source of the configuration values.

> The configuration information could be stored in a data store such as SQL Server or Azure Cosmos DB. 

> Environment-specific settings could be stored in different Configuration Stores allowing the same code to work in Dev vs. Staging or Production.

#### A challenge here
> Is knowing when to get the settings.

> The code can either get all the settings at startup or as needed.

## Messaging
> As microservices are designed to fit various business needs, their communication methods must also be considered. 

> With monolithic applications, methods simply call other methods without the need to worry about where that method resides.

> With distributed computing, those methods are on other servers.

> <code>Interprocess communication (IPC)</code> mechanisms are used to communicate with microservices since they are over a <code>network</code>.

### There are three main aspects of communication with microservices
+ Business processes and use cases help determine the layout of messaging needs.
     > determine the “why” there is communication with microservices.
     
     > The “what” is the data format.
+ Transport mechanisms 
    > Used to transfer message content between processes.

    > This covers the “how” messages are sent to endpoints.

#### Business Process Communication
There are multiple ways of communicating between business processes in a microservices architecture. 
> The simplest but least versatile method is using <code>synchronous</code> calls.

> The three other ways are <code>asynchronous</code> and provide various <code>message delivery</code> methods.

##### 1. RPC (Remote Procedure Call)
The synchronous, “direct” way is for when a request needs an immediate response.

+ It is direct and synchronous, from the client to the service.
+ Using RPC should be limited in use. 
+ These have a high potential of adding unnecessary latency for the client.
+ Should only be used when the processing inside the microservice is small.

##### 2. Fire-and-Forget
Is <code>asynchronous</code> call.

+ The client does not care if the microservice can complete the request.
    > An example of this style is logging.

##### 3. Callback
Is <code>asynchronous</code> call.
+ The microservice calls back to the client, notifying when it is done processing.
+ The business process continues after sending the request. 
+ The request contains information for how the microservice is to send the response. 
+ This requires the client to open ports to receive the calls and passing the  address and port number in the request.
+ With many calls occurring, there is a need to match the response to the request. 
+ When passing a correlation ID in the request message and the microservice persisting that information to the response, the client can use the response for further processing.

<code>This notification is called a “domain event.”</code>

##### 4. Pub/Sub (Publish/Subscribe)
Is <code>asynchronous</code> call.
+ This is a way of listening on a message bus for messages about work to process.
+ The sender publishes a message for all the listeners to react on their own based on the message. 
###### A persistent-based Pub/Sub model :
    > Only one instance of a listener works on the message.

#### Message Format
+ The format of the data in the messages allows your communication to be cross language and technology independent.
+ There are simply two main formats for messages:
    1. text
    2. binary
+ The human-readable text-based messages are the simplest to create but have their burdens. 
    + These formats allow for the transportation to include metadata. 
    + With small to medium size messages, JSON and XML are the most used formats.
    + But, as the message size increases, the extra information can increase latency.
+ Utilizing a format such as Google’s Protocol Buffers or Avro by Apache ,the messages are sent as a binary stream. 
    + These are efficient with medium to large messages because there is a CPU cost to convert content to binary.
    + Smaller messages may see some latency.

#### Transport 
+ Transportation mechanisms are responsible for the delivery of messages to/from the client and microservices. 
+ There are multiple protocols 
    1. HTTP
    2. TCP
    3. gRPC
    4. AMQP (Advanced Message Queuing Protocol )

##### The drawback with synchronous messaging:
+ There is a tighter coupling between the client and services.
+ The client may know about details it should not need to care about, such as how many services are listening and their address. 
+ the client must do a DNS lookup to get an address for a service.

##### Representational State Transfer (REST) 
Is an architectural style that is quite common today when creating Web APIs and microservices.

> As a microservice architecture develops, there may be microservices calling other microservices. 

> There is an inherent risk of latency as data is serialized and deserialized at each hop. 

> An RPC technology called “gRPC Remote Procedure Call” (gRPC) is better suited for the interprocess communication.

> gRPC is a format created by Google using, by default, protocol buffers.

> Where JSON is a string of serialized information

> gRPC is a binary stream and is smaller in size and, therefore, helps cut down latency. 

> This is also useful when the payload is large, and there is a noticeable latency with JSON.


> For asynchronous calls, messages are sent using a message broker such as RabbitMQ, Redis, Azure Service Bus, Kafka, and others.

> AMQP is the primary protocol used with these message brokers.

> AMQP defines publishers and consumers. 

> Message brokers ensure the delivery of the messages from the producers to the consumers. 

> With a message broker, applications send messages to the broker for it to forward to the receiving applications.

> This provides a store-and-forward mechanism and allows for the messages to be received at a later time, such as when an application comes online.

## Testing
### Test Pyramid
+ Represents the number of tests compared to each level, speed, and reliability.
+ The <code>unit tests</code> should be small and cover basic units of business logic.
+ <code>Service tests</code> are for individual microservices.
+ <code>End-to-End</code> tests are the slowest and most unreliable as they generally depend on manual effort and the least amount of automation.

![Testing pyramid!](/Images/3.png "Testing pyramid")

### E to E
> Sometimes referred to as <code>system tests</code>, are about testing the system’s interactions that use microservices and their interaction with other services.

> May include <code>UI level tests</code> using manual effort or automated tests using products like Selenium. 

> <code>System tests</code> verify subsequent calls retrieve and update data.

![Depiction of end-to-end testing!](/Images/4.png "Depiction of end-to-end testing")

### Service
> <code>Component tests</code> are for testing a microservice apart from other services like other microservices or data stores.

> You use a <code>mock</code> or a <code>stub</code> to test microservices that depend on a data store or other microservices.

> <code>Mocks and stubs </code> are configured to return predetermined responses to the System Under Test (SUT).

> A <code>stub</code> returns responses based on how they are set up.

#### Example 
When called for saving a new order, it returns the order and other information as if it was just saved. This helps the speed of testing since it skips time-consuming, out-of-process logic and only returns however it was set up.


![Using stub for testing!](/Images/5.png "Using stub for testing")

> <code>Mocks</code> help verify dependencies are invoked.

> <code>Mocks</code> also require a setup of predetermined responses.

> They are used to help test behavior, whereas a stub helps with testing of state.

#### Exampmle
when calling a createOrder method, verify that the method to create a log message was also called.

### Unit Tests
> Good unit tests are the fastest to execute and, generally, the most reliable.

> They are at the most basic level of code.

> Unit tests should not invoke calls outside of the executing process.
         This means there should be no calls to data stores, files, or other services.

> These tests are for testing the details of business logic.

### Automation
> Using a Continuous Integration/Continuous Deployment (CI/CD) pipeline should be considered a must-have.

> They help with the automation of testing and deployment.

> It is highly recommended to use a build step in your CI/CD pipeline that is performing the unit tests.

> Integration tests are generally much longer in execution time, so many companies do not add them to the CI/CD pipeline.

> A better recommendation is to have them executed nightly, at least, or on some other schedule.

## Deploying Microservices
### Versioning
> As newer releases of microservices are deployed, versioning takes consideration.

> Leveraging a versioning semantic from SemVer (https://www.semver.org), there are three number segments that are used:

+ <code>Major</code> – version when you make incompatible API changes.
+ <code>Minor</code> – version when you add functionality in a backward compatible manner.
+ <code>Patch</code> – version when you make backward-compatible bug fixes.

### Containers
> Allow for executables, their dependencies, and configuration files to be packaged together.

> Although there are a few container brands available, <code>Docker</code> is the most well known.

> Deploying microservices can be done straight to servers or more likely virtual machines.

> However, you will see more benefits from running containers:
+ By using containers, you can constrain resources like CPU and RAM, so processes that consume too much CPU or have memory leaks do not kill the server they reside on.
+ They are easier to manage.
+ Controlling the number of container instances, running on certain servers, and handling upgrade/rollback and failures can all be handled by using an orchestrator.
+ Orchestrators like Docker Swarm, Service Fabric, Kubernetes, and others manage containers with all the features just mentioned but also include features like network and security.

## Pipelines
> With development processes like Continuous Integration and Continuous Deployment (CI/CD), you can leverage tools that help automate testing and staged releases.

> You can set them up to be either manually executed or triggered when code is checked into a repository.

> You can also set up Builds and Releases on a timebased schedule so, for example, hourly or nightly environments are updated.

> Microservices should be considered as their own independent application with their own deployment pipelines.

> They have the autonomy to evolve independently of monoliths and other microservices.

> They should also have their own repository.

> Having separate repositories aids in the autonomy to evolve.

## Cross-Cutting Concerns
> If a microservice became unavailable at 2 am, how would you know? When would you know? Who should know?

These crosscutting concerns (CCC) help you understand the system's health as a whole, and they are a part of it.

### Monitoring
> To say that your microservice is running well means nothing if you cannot prove it and help with capacity planning, fault diagnosing, and cost justification.

> Evaluate good monitoring solutions.

> Prometheus (https://prometheus.io) is a great option, but it is not the only good one available.

> Tools like Grafana (https://grafana.com) are for displaying captured metrics via Prometheus and other sources.

> Using Prometheus and Grafana helps you know if a server or Node (if using Kubernetes) is starving for resources like CPU and RAM. 

### Logging
> Logging information from microservices is as vital as monitoring, if not more so.

> Monitoring metrics are great, but when diagnosing fault events, exceptions, and even messages of properly working calls, logging information is an absolute must.

> A centralized logging system provides a great way to accept information that may come from multiple systems. This is known as “Log Aggregation.”

> Loggly (https://www.loggly.com) is a logging system with many connectors for a variety of systems.

### Alerting
> With data monitored and captured, development teams need to know when their microservices have issues.

> Alerting is the process of reacting to various performance metrics.

> Setting alerts based on situations like high CPU utilization or RAM starvation is simple.

### list of example metrics you should create alerts for:
+ High network bandwidth usage
+ CPU usage over a threshold for a certain amount of time
+ RAM utilization
+ Number of inbound and outbound simultaneous calls
+ Errors (exceptions, HTTP errors, etc.)
+ Number of messages in a service broker’s Dead Letter Queue (DLQ)

> Tools like Grafana, Azure Log Analytics, and RabbitMQ have their alerting features.

> An additional option is using webhooks to send information to a Slack channel.

### Testing the Architecture
Consider the following questions as a starting list of items to evaluate
+ Was the failure logged?
+ Do the log entries reflect enough detail to identify the server, microservice, etc.?
+ Is the failure due to code, network, 3rd party call, or data store? Something else?
+ Does the failure appear on the monitoring tool?
+ Was an alert generated?
+ Did the alert go to the development team responsible?
+ Did the alert contain enough information to start troubleshooting properly?
+ Are there multiple failures noticed? Should there be one or multiple?
+ Did the code retry using a “retry policy”? Should it? Was the retry adequate?
+ After resolving the failure, did the alert stop? Does logging show correct activity?
+ Are you able to produce a report of the failure that includes information from the monitoring tool, correlated log entries, alerts, and from those who responded?

