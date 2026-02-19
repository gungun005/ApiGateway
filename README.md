API GATEWAY
Single entry point for all clients in a microservices architecture
The gateway routes the request internally =>HOW?
<br></br>
? WHY DO WE USE API GATEWAY?
1-Centralizes entry point
client talks to one URL

2-Authorization and Authentication
instead of implementing JWT Validation in every service
Gateway validates token once
Forward requests to internal services.

3-Load Balancing
distribute traffic among multiple instance of service

4-Rate Limiting
Prevents abuse

5-Request Aggregation
Combine multiple service response into one
e.g.- GET/user-dashboard
GATEWAY calls=>
User service
Order service
Recommendation Service

Then combine data and returns single response

6-Logging and Monitoring
Central place to log all requests

<br></br>
? WHEN SHOULD WE USE IT

you have multiple microservices
clients are web + mobile + 3rd party
need centralized security

CORE CONCEPTS OF API GATEWAY

ROUTING-process of mapping
Incoming request(external URL)-Correct internal microservices

<br></br>
Why we need routing?

In microservices
-each service runs on different port
-different servers
-different containers
-possibly different clusters

Without Routing=>Client must know internal architecture breaks abstraction!

<br></br>
How Routing works internally?

When request comes=>GET/api/users/5
Gateway does=>

STEP 1-PATH MATCHING-

It checks routing table
Basically a pattern matching engine
Internally-
.Path is tokenized
.Compared against templates
.Parameters extracted

e.g.=>/api/user/5
Matches=>/api/user/{id}
extracts=>id=5

STEP 2- REQUEST TRANSFORMATION
Gateway creates new Http request
GET https:// userservice:5001/internal/users/5
This is done using internal HTTP client


IMPORTANT ADVANCED CONCEPTS IN ROUTING

1-PATH REWRITING

The external API path seen by client is different from internal service path
e.g.
client calls-
GET/api/v1/users /10
Gateway calls-
Get/internal/user-service/profile/10

Why do we need path rewriting?
Exposing internal structure can leak
-internal naming
-architectural design
-versioning logic
Gateway allows clean public API'S

How path rewriting works internally?

When request arrives=>GET/

2-VERSIONING STRATEGY
e.g.=>
client-/api/v2/products
Gateway internally routes- product-service/api/products
We can keep internal API same and change only Gateway mapping
This reduces breaking change

<br></br>
How path rewriting works internally?

When request arrives=>
GET/api/users/10
Gateway-
1.split path into segments
2.match against route template
/api/users/{id}
3.extracts variables
id=10
4.substitutes into downstream templates
/internal/users/{id}
This is simple string token replacement + regex pattern matching!

2#-HEADER TRANSFORMATION-
Headers are extremely powerful 

<br></br>
Why headers manipulation is needed?
Headers carry=>
authentication tokens
correlation ids
user metadata
API version
Rate limit info

1-ADD HEADERS-After JWT Validation
Authorization: Bearer<token>
Gateway extracts user ID and injects
X-User-Id:123
X-User-Role: Admin
Downstream service trust these headers
This reduces -
-Token passing every service
-Repeated signature verification

2-REMOVE HEADERS
Security Reason-
Client sends-
X-Internal-Admin: true
Gateway removes it before forwarding
Prevents header spoofing!

3-MODIFY HEADERS
E.g.=>HOST: api.myapp.com
Internally service expects
Host-User service
Gateway modifies before forwarding

How headers processing works internally?
 
When a request hits API gateway it has 3 stages:

1-Parsing Stage

TCP Connection established
HTTP request is parsed 
Headers are stored in an internal structure usually case insensitive dictionary
Headers are now accessible via middleware

2-Middleware Access

It can Read, Add, Remove and Update headers

3-Downstream Request Creation

Gateway creates a new -HTTP Request Message
It copies allowed headers
It excludes restricted ones
It adds transformed headers
Then sends it via HTTP Client
 
HOP BY HOP HEADERS

Used only for single transport connection
These must not be forwarded
E.g. 
connection
keep alive
transfer-encoding
upgrade
proxy-Authenticate
proxy-Authorization

<br></br>
Why?=Because it control TCP Connection and not Application Logic
Forwarding them can break protocol behaviour

END TO END HEADERS

These should be forwarded
E.g.
Authorization
Content-Type
Accept
User-Agent
X-User-Id

These define request semantics
<br></br>

Why careful header copying is important?
If done incorrectly
-security issues
-broken HTTP Protocol
-unexpected behaviour in downstream services
-duplicate headers causing errors

HEADER SPOOFING-

If Gateway blindly forwards it Downstream may trust it.
Solution=>
Remove sensitive headers from client
Recreate trusted headers after authentication

CLEAN MENTAL MODEL-
Parse headers
Validate security
Remove unsafe headers
Add trusted internal headers
Forward only safe headers

3#-QUERY STRING FORWARDING

E.g.=>Get/api/products>page=2&sort=prices
Query String contains-
Pagination
Filtering
Sorting
Search items

How it works internally?

Gateway=>
Parses query string
Either forward as in
Or transform it

E.g.=>external :_?page=2
Internal expects: ?page=2
Gateway rewrites query parameters

Gateway can-
Remove sensitive query params
Add internal tracking params
E.g.=>GET/products? Page=2
Gateway forwards:-
GET /products? Page =2 & source=mobile-app

Risk- 
Query injection attacks
Very long query strings-memory overhead
Complex Rewriting -processing cost

Efficient gateways=>
Avoid heavy parsing
Use optimised query string handling

4#- HTTP Method Filtering

Important for Security
Control which HTTP methods are allowed 
Gateway blocks unwanted methods before hitting service
<br></br>
	HOW IT WORKS INTERNALLY?

Incoming request=> GET/api/users/10
Gateway checks route config
"Upstream HTTP Method":["GET","POST"]
Since delete is not ALLLOWED
It returns 405-Method Not Allowed
No downstream call made

<br></br>

REVERSE PROXY-

Client reverses internal service IP
Why? 
It provides security Internal services are not exposed publicly
Not reachable from internet
It provides Abstraction-
You can change service location
Move service to new server
Client does not care

BTS=>
When client connects TCP Handshake->Gateway
Gateway-
Accepts TCP connection
Reads Http request
Opens new TCP Connection to downstream service
Forwards Request
Reads response
Sends Response back

GATEWAY ACTS AS HTTP MIDDLEWARE

<br></br>
SERVICE DISCOVERY

It allows gateway to dynamically find where source is running , how many instances exist , which instance to call

TWO TYPES
1-CLIENT SIDE DISCOVERY-Gateway asks registry =>where is user service
Registry replies which like the address =>IP

2-SERVER SIDE DISCOVERY-Load balance sits in front of services

How it works internally?
When service starts it sends heartbeat every 30 secs
I heartbeat stops -Registry removes service
Gateway queries registry -gets updated service list
<br></br>

CIRCUIT BREAKER
Problem-Payment service becomes slow
Gateway keeps forwarding requests
Each request-

Waits 30 seconds timeout
Thread gets blocked
Thread Pool exhausts
Whole system crashes

This is called Cascading Failure Solution
1->OPEN 
failure exceeds threshold
gateway stops calling service
Immediately returns error/fallback

2->CLOSED
normal operation

3->HALF-OPEN
After cooldown
Gateway sends small test required
If success-back to closed
If fail-back to open

<br></br>
HOW IT WORKS?
Gateway tracks- failure count, timeout count , error rate percentage

OAUTH FLOW
Client authenticates with Identity Provider
Gets access token
Sends token to gateway
Gateway validates token with public key


<br></br>
BTS=FOR ONE REQUEST

TCP Handshake
HTTP Parsing
Authentication middleware
Route Matching
Service Discovery Lookup
Load Balancing decision
Circuit Breaker check
Downstream HTTP Call
Response Transformation
Logging
Response sent back
API GATEWAY IS NOT JUST A ROUTER IT'S INFRASTRUCTURE BRAIN OF MICROSERVICE ARCHITECTURE
It reduces coupling, improves resilience ,central policies ,simplify clients and enables scaling!


