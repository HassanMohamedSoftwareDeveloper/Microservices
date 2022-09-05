# Rest Vs gRPC
## REST
> <code>Roy Fielding</code> created Representational State Transfer (REST) in 2000 to transfer data using stateless connections.

> Provides a form of state over the stateless protocol HTTP.

> Uses the development of HTTP 1.1 and Uniform Resources Identifiers (URI).

> Is best suited for text-based payloads and not binary payloads.

> Most data is in memory, it must be transformed, or “serialized,” to a textual representation like JSON or XML before being sent to an endpoint.

> Data is then deserialized back to a binary form stored in memory when received.

> Has operations called HTTP verbs.

+ <code>GET</code> – used for the retrieval of data.
+ <code>POST</code> – used for the creation of data.
+ <code>PUT</code> – used for updating data.
+ <code>DELETE</code> – used for the deletion of data.

> The REST standard considers GET, PUT, and DELETE is <code>idempotent</code>.

> <code>Idempotent actions</code> mean that sending the same message multiple times has the same effect as sending once.

> For <code>developers</code>, this means we need to pay careful attention to our APIs.

## gRPC
> <code>Google</code> created gRPC for faster communication with distributed systems by using a <code>binary protocol</code> called <code>Protocol Buffers</code>.

> Like REST, gRPC is language agnostic, so it can be used where the microservice and caller are using different programming languages.

> Unlike REST, gRPC is type specific and uses Protocol Buffers to serialize and deserialize data.

> You must know the type definitions at design time for both parties to understand how to manage the data.



## There are some considerations when choosing which transport mechanism to use for microservices 
1.  gRPC uses the HTTP/2 protocol, which helps with lower latency.
    > However, you may need to verify various pieces are compatible with HTTP/2.

1. If you know the microservices are public facing, either on the Internet or on a network deemed public, <code>REST is the most likely chosen option</code>.
    > It is the most versatile(تنوعاً) and easiest to use.
1. If the calls are from internal code such as a monolith or another microservice, you may consider gRPC.
1. Using gRPC will help <code>lower latency</code> and is worth the <code>extra time</code> to set up. 