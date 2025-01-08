# Order Processing System

## Overview

This is a simple order processing system utilizing message queue technology (MQTT + MQTTNet), consisting of two microservices: a publisher and a subscriber console application.

The publisher submits orders in the following format:

```
{
	"Id": "93c47201-2d5e-44d7-a242-f7800ad3d009",
	"CustomerName": "John Doe",
	"Product": "Laptop",
	"Status": "Pending",
	"CreatedAt": "2024-12-24T12:00:00.0000000Z"
}
```

While notifying the user about submitting the order.

```
Order submitted!
Order ID: 93c47201-2d5e-44d7-a242-f7800ad3d009
```

When the subscriber application receives the order, it takes between 1 and 3 seconds to process it.

```
Order received!
Processing order: Id = 93c47201-2d5e-44d7-a242-f7800ad3d009, Customer = John Doe, Product = Laptop
Order processed: Id = 93c47201-2d5e-44d7-a242-f7800ad3d009, Status = Processed
```

## Setup & Usage

There are two ways to use the applications, each requiring a different level of work setting up the system.

To gain control over the underlying logic, open the solution files of both microservices (**OrderSubmissionService** and **OrderProcessingService**) in Visual Studio, then build the solution. Afterwards, navigate to the folder [path/to/microservice/MicroserviceName/MicroserviceName], and run the **OrderSubmissionService** with:

```
dotnet run -- order "CustomerName" "Product"
```

And the **OrderProcessingService** with simply:

```
dotnet run
```

If the source code and the unit tests are not needed, it is enough to just download the *publish* folder, navigate to the specific microservice folders inside, and run the applications with:

```
OrderSubmissionService order "CustomerName" "Product"
```

```
OrderProcessingService
```

If there are less than 3 arguments when running the **OrderSubmissionService**, or the first argument is not "order", there will be a warning message:

```
Usage: dotnet run -- order "CustomerName" "Product"
```

This format is valid only if running a built solution, not when running the service via the executable file.

In the case of **OrderProcessingService**, it is enough to run it by double-clicking the executable file, since it doesn't take any arguments.

Make sure to wait until the **OrderProcessingService** has confirmed that it subscribed to the message queue topic before sending orders.

The system uses a public MQTT broker, so it does not need to be set up.