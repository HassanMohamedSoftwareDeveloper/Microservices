# Microservice Messaging
## Issues with Synchronous Communication
+ Latency 

## Limits of RPC
+ This shows the biggest drawback of RPC with microservices; processes must wait for a response, that is, synchronous communication.
+ Handling the number of simultaneous calls.
    For example, if more calls are coming in than the microservice can handle, the caller will see either severe latency or no contact at all. 
    This forces the caller to use retry policies or the circuit breaker pattern.
+ Scaling microservices horizontally requires a load balancer. The load balancers must also be performant not to add any latency.
    And just because another instance of a microservice exists does not mean it is instantly registered in the load balancer. So, there is an effort to register the microservice instance in the load balancer
+ There is the other issue of adding other microservices to a business process.
    If the caller is only aware of one microservice, then it must be altered to know about the other microservices.
    This means more code changes to each caller that must be aware of the others. 
    And how is versioning to be handled? There are even more code changes to handle adapting to a different version.

## Messaging
> Using messaging in the microservices architecture allows independent pieces to communicate without knowing the location of each other.

### Reasons to Use Messaging
#### Loosely Coupled
> By using messaging, the sender and the microservices (message consumers) are loosely coupled.

> The sender of a message does not need to know anything about the microservices receiving the message. 

> This means that microservices do not need to know the endpoints of others. 

> It allows for the swapping and scaling of microservices without any code changes.

> This provides autonomy for the microservices.

> With autonomy, the microservices can evolve independently.

> This allows for microservices to only be tied together where they fit to fulfill business processes and not at the network layer.

#### Buffering
> Message brokers utilize queues that provide a buffer of the messages.

> During times of issues, the undeliverable messages are stored.

> Once the consumers are available again, the messages are delivered.

> This buffering also retains sequencing.

> The messages are delivered in the order they were sent.

> Maintaining the message order is known as First In, First Out (FIFO).

#### Scaling
> The ability to scale a microservice architecture is paramount. 

> For any production applications or distributed processing, you will have multiple instances of your microservices.

> This is not just for high availability but allows messages to be processed with higher throughput.

#### Independent Processing
> Messaging allows a microservice architecture solution to change over time. 

> You can have a solution to generate orders and manage the shipping of the products. 

> Later, you can add other microservices to perform additional business processes by having them become message consumers. 

> The ability to add microservices to business processes as consumers allows them to be independent information processors.

### Message Types
#### Query
#### Command
#### Event

### Message Routing
> For messages to go from a publisher to the consumers, there must be a system to handle the messages and the routing.

#### Broker-less
> A broker-less system, like ZeroMQ, sends messages directly from the publisher to the consumer.

> This requires each microservice to have the broker-less engine installed.

> It also requires each endpoint to know how to reach others. 

<mark> As you scale your microservices, it quickly becomes harder to manage the endpoint list</mark>.
> Because there is no central message system, it can have lower latency than a brokered system.

> This also causes a temporal coupling of the publisher to the consumer.

<mark>This means that the consumer must be live and ready to handle the traffic.</mark>
<mark>One way to handle the chance of a consumer not being available is to use a distributor.</mark>

> A <code>distributor</code> is a load balancer to share the load when you have multiple instances of a microservice.

> The distributor also handles when a consumer is unavailable, sending a message to another instance of your microservice.

#### Brokered
> A brokered system like ActiveMQ, Kafka, and RabbitMQ provides a centralized set of queues that hold messages until they are consumed.

> Because the messages are stored and then sent to consumers, it provides a loosely coupled architecture. 

> The storing of messages until they are consumed is not a high latency task.

> It simply means the publisher does not have to store the messages but can rely on the broker.

<mark> An advantage of using a broker is that if a message fails in the processing by a consumer, the message stays in queue until it can pick it up again or another process is ready to process it.</mark>

### Consumption Models
> There are multiple models for receiving messages you will see with multiple consumers. You will use one or both models.

#### Competing Consumers
> Various business processes need to have a message processed by only one consumer.

![Invoice microservice instances as competing consumers!](/Images/7.png "Invoice microservice instances as competing consumers")

> You should have multiple instances of a microservice for scaling, availability, and distribution of load. 

> A microservice is designed to subscribe to a specific queue and message type.

> But when there are multiple instances of that microservice, you have a competing consumer scenario.

> When a message is sent, only one of the microservice instances receives the message. 

> If that instance of the microservice that has the message fails to process it, the broker will attempt to send it to another instance for processing.

#### Independent Consumers
> There are times when other microservices must also consume the message. 

> This means that these microservices do not compete with other consumers.

> They receive a copy of the message no matter which competing consumer processes the message.

![Independent consumers!](/Images/8.png "Independent consumers")

> These independent consumers process the messages for their specific business processing needs.

<mark>In the previos image, a message is sent from the Order Microservice. The message needs to be received by one instance of the Payment Microservice and one instance of the Shipping Microservice. 
Here the Payment and Shipping microservices are not competing with each other. Instead, they are independent consumers. 
Also, note that each instance of a Payment Microservice is a
competing consumer to itself. The same is true for the Shipping Microservice instances. </mark>

> Having independent consumers allows for a Wire Tap pattern 

<mark>An example of this has a front-end component that sends a command to the Order Microservice to revise an order. In RabbitMQ, you can have an exchange that is bound to another exchange. 
This allows copies of messages to be sent to another consumer, even though the exchange is set up with direct topology</mark>

![Wire Tap pattern!](/Images/8.png "Wire Tap pattern")

### Delivery Guarantees

#### At Most Once
> This delivery guarantee is for when your system can tolerate the cases when messages fail to be processed.

> An example is when receiving temperature readings every second. 
If the broker delivers the message, but the consumer fails to process it, the message is not resent. 
The broker considers it delivered regardless of the consumer's ability to complete any processing of the message.

#### At Least Once
> The “at least once” delivery guarantee is where you will spend the most effort designing the consumers to handle messages in an idempotent manner.

> If a message fails to be given to a consumer, it is retried.

> If a consumer receives a message but fails during the process, the broker will resend that message to another consumer instance.

> Although most broker systems have a flag that is set when it sends a message again, you are safer to assume the message is a duplicate anyway and process accordingly.

#### Once and Only Once
> Another name for this delivery guarantee is “Exactly Once,” which is the hardest to accomplish.

> Many argue it does not exist. (الاغلبيه يجادل انه غير موجود) 

> When you need a message to be processed only once, how do you know it was only processed once? How do you know the message is not a duplicate? Essentially, how do you know the intended state has not already succeeded?

> The point here is that guaranteeing a message was only delivered and processed once requires state management involving the consumers and the broker system.

> This addition of state management may be far more trouble than designing for the other delivery guarantees.

### Message Ordering
> we use messaging systems that process messages as efficiently as possible. But this puts the onus on developers to build an architecture that can tolerate the reception of out-of-order messages.

#### An example scenario
> A message sent to an Order Microservice to create a new order.
> Another message to a different instance of the same microservice is sent with an update to the order. 
> During the processing of the first message, an error occurs, and the message is not acknowledged.
> The broker then sends that message to the next microservice instance that is available.
> Meanwhile, the message with the update to the order is being processed.
> The microservice code must decide how to update an order that does not exist yet.



### Building the Messaging Microservices
> We will create three projects:

+ Invoice Microservice.
+ Payment Microservice.
+ Test client.

> The test client will act as a front-end service with information for the Invoice Microservice to create an invoice.

> Then the Invoice Microservice will publish a message about the newly created invoice.

> The Payment Microservice and the test client will both receive the message that the invoice was created.

> For the test client, it is confirmation the invoice was created (Display the invoice number).

> The Payment Microservice serves as a quick example of a downstream microservice that reacts to the creation of the invoice.

#### Running RabbitMQ
> You have some options when running RabbitMQ.

+ run on a server, on your computer.
    > go to https://rabbitmq.com/download.html.
+ In a Docker container.
    > To install Docker Desktop, go to https://docker.com/products/docker-desktop.

    > Go to a command prompt and enter
    ``` CLI
        docker run -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```
    > If you prefer to run the RabbitMQ instance detached from the console:
    ``` CLI
        docker run -d 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```
    > Look at the RabbitMQ Management site by going to http://localhost:15672 
    The default username and password for RabbitMQ are guest and guest.

> We will be running MassTransit on top of RabbitMQ. 

> MassTransit provides a layer of abstraction and makes coding easier. It can run on top of RabbitMQ, Azure Service Bus, ActiveMQ, and others.

### Drawbacks of Messaging
+ Messaging solutions require effort and time to understand the many pieces that must be decided. 
+ Expect to create several proofs of concept to try out the many designs. 
    > You will need to judge each design based on complexity, ease of implementation, and manageability variations.
+ You have the infrastructure to create and manage.
+ The messaging product, RabbitMQ, for example, must run on a server someplace. Then for high availability, you must create a cluster on multiple servers. With the additional servers, you have more infrastructure to maintain.
+ Troubleshooting is also much harder.
+ Since messages can end up in a Dead Letter Queue, you may have to replay that message or decide to delete it. 
+ Decide if the timeout setting for the DLQ is sufficient.
+ You will also need to verify the messaging system of choice is fully functioning.