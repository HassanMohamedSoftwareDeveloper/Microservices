# The Business
> Our hypothetical client company Hyp-Log is a shipping logistics middleman coordinator for hotshot deliveries.

> Hotshot deliveries are point-to-point transfers of goods or materials that require a level of expedience that may not be attainable by larger carrier firms.

> Hyp-Log’s value to its customers derives from its ability to source the best possible price given the type, size, and weight of a given shipment. In essence, Hyp-Log took all their rate spreadsheets and the analysis that staff were performing manually and built a “rate engine” to perform those same calculations with greater speed, higher accuracy, and better overall customer experience.

> A previous employee of Hyp-Log created a custom application for them. 

> Although the application has helped in many ways, a few business processes remain as manual effort.

> With growing demands, Hyp-Log decides to have a company make the programming changes necessary.

> There are several challenges when it comes to managing shipments without automation.

> Let’s consider an owner/operator in a carrier company.

> This person is not only the owner of the company, responsible for all the back-office administrative tasks, but this person is also responsible for quoting shipments and delivering said shipments.

> That is an awful lot of hats to wear for one individual.

> Here is where the monolithic carrier portal shines, and the application removes the burden of quoting each shipment.

> The rating engine uses carrierprovided rate sheets, fleet availability, and capabilities to generate quotes for customer shipments automatically, thus removing the burden of a quotation from the carrier.

> After a few years of using spreadsheets and calling carriers for quote information, they decided to have an application made to handle their processes.

> An employee created an application to facilitate as much of the work as possible.

> That application became affectionately known as “Shipment Parcel Logistics Administration – Thing,” aka “SPLAT.” Naming things is hard.

> SPLAT has three main user types: customers, carriers, and administrators.

> It provides a way for customers to submit load requests, pick a carrier based on a list of quotes, and provide payment.

> Carriers can manage their fleet and base costs per mile plus extra costs based on particular needs.

> Some loads require a trailer, are hazardous material, or even refrigerated.

# Domain-Driven Design
Eric Evans is the founder of DDD and author of Domain-Driven Design: Tackling Complexity in the Heart of Software.
## Domain 
> DDD is a way of developing applications with an intended focus on a domain.

> The domain is the realm for which you created the application.

+ In the case of Hyp-Log, their domain is hotshot load management.
+ Other domains may exist in the company, like Human Resources and Insurance, but they are not relative to why Hyp-Log exists as a company.
+ They are ancillary domains for a company to function, but they do not help Hyp-Log stand out among competitors.

## Subdomains
> Digging deeper into a domain are subdomains.

> A subdomain is a grouping of related business processes.

### For Example 
You may have a group of processes for Accounts Payable, Accounts Receivable, and Payroll in an accounting domain. 
The business processes related to each other for generating and processing invoices belong to the Accounts Receivable subdomain.

> This grouping of focus is known as the <code>Problem Space</code>.

> The code that provides the functionality in a subdomain should also exist in groups separate from others.

> The groups of code that provide functionality for their subdomain are called a <code>bounded context</code>. 

> The <code>bounded context</code> exists in what is known as the <code>Solution Space</code>.

> When we decide what code should become a microservice, you will see how the subdomain type weighs in.

> There are three subdomain types: Core, Supportive, and Generic.
	
1. <code>Core</code> – Each core subdomain in an application contains one or more bounded contexts that are critical to the company.
2. <code>Supportive</code> – The supportive subdomains are not deemed critical but contain code that is supportive of the business.
3. <code>Generic</code>– Lastly, generic subdomains are those that are replaceable with off-the-shelf solutions.

## Ubiquitous Language
> One of the most important artifacts of DDD.

> A collection of phrases and terms that helps everyone involved to have a clear and concise understanding of business processes.

## Bounded Contexts
> A collection of codes that implements business processes in a subdomain distinctly different from other processes.

### For Example 
> Consider an application at a manufacturing plant.

> The code for inventory management of purchased parts is different than inventory management of manufactured parts.

> Because the business processes differed between purchased parts and manufactured parts, the code providing business functionality is also different.

> By using ubiquitous language here, the term “part” is distinguished to the various purposes to become “purchased part” vs. “manufactured part.” Thus, there is a bounded context, determined by the language around the term “part.”

### Identifying bounded contexts 
> Can be done by a couple of different tactics.

1. <code>Language</code>is an indicator of a boundary, making a bounded context.
1. <code>Functionality</code>that uses specific language in the namespaces, class names, and method help identify a bounded context and its purpose.

## Aggregates and Aggregate Roots
> There can be multiple classes in a bounded context.

> There is a relationship between the classes based on dependency.

> This dependency may be due to inheritance or composition.

> Each group of these classes is an Aggregate and the toplevel class in an Aggregate Root.

> There is a rule, provided by Eric Evans, that states that classes in an aggregate may call on each other or an aggregate root of another aggregate even if it is in another bounded context. 
The idea is that no class in a bounded context may leverage a class in another bounded context without going through the root of an aggregate. 
This rule is an architectural form of encapsulation. It prevents building dependencies that quickly become fragile.

![Example of aggregates!](/Images/6.png "Example of aggregates")

Should any class in the benefits aggregate be allowed to call the employee object in the payroll aggregate and change its state?  
<code>No</code>


