# CQRS Patterns Lab

A practical .NET 8 project demonstrating different CQRS architectures, from a simple monolith to distributed microservices.

The goal of this repository is to understand **CQRS, Eventual Consistency, Outbox Pattern, and asynchronous processing** by implementing them step by step.

---

## 🏗️ Architecture

The project is divided into multiple phases:

```text
CQRS-Patterns-Lab
│
├── 01-SimpleMonolith
├── 02-MonolithSeparateDB
├── 03-Microservices
│
├── docs
│   ├── architecture.md
│   ├── cqrs.md
│   ├── outbox.md
│   └── consistency.md
│
└── docker-compose.yml
```

---

# Phase 1 — Simple Monolith

### Architecture

```text
                ASP.NET Core API
                       │
                    MediatR
                  /        \
             Command       Query
                │             │
                └──────┬──────┘
                       │
                  Single DB
```

### Characteristics

* Single application
* Single database
* CQRS using MediatR
* Separate Commands and Queries
* Write operations modify the domain
* Read operations use optimized queries
* `AsNoTracking()` for read operations
* Projection to DTOs

### Concepts demonstrated

* CQRS
* MediatR
* Command / Query separation
* Domain modeling
* Read optimization
* Clean Architecture concepts

---

# Phase 2 — Monolith with Separate Read/Write Databases

The second phase separates the write model from the read model.

### Architecture

```text
                    ASP.NET Core API
                           │
                        MediatR
                      /        \
                 Command       Query
                    │             │
                    ▼             ▼
                Write DB       Read DB
                    │
                    ▼
                 Outbox
                    │
                    ▼
            Background Worker
                    │
                    ▼
                 Read DB
```

### Write Side

The command modifies the Write Database.

The Order and its corresponding Outbox message are committed in the same database transaction.

```text
Create Order
     │
     ▼
Save Order
     │
     ▼
Generate Order.Id
     │
     ▼
Create Outbox Message
     │
     ▼
Commit Transaction
```

This guarantees that the Order and its event are persisted atomically.

---

## Outbox Pattern

The Outbox Pattern is used to reliably store events that need to be processed asynchronously.

Example:

```text
Orders
--------------------------------
Id
CustomerId
TotalAmount
Status
```

```text
OutboxMessages
--------------------------------
Id
Type
Payload
OccurredOnUtc
ProcessedOnUtc
Error
AggregateId
```

An `OrderCreatedEvent` is serialized and stored inside the Outbox.

The event is only marked as processed after the Read Database has been successfully updated.

---

## Eventual Consistency

The Write Database and Read Database are not updated at exactly the same time.

```text
Write DB
   │
   │ Commit
   ▼
Outbox
   │
   │ asynchronous processing
   ▼
Read DB
```

Therefore, the Read Database can temporarily lag behind the Write Database.

This is an example of **Eventual Consistency**.

---

## Idempotent Processing

Outbox processing follows an **at-least-once** processing model.

A message can potentially be processed more than once.

The read-model projection therefore checks whether the Order already exists before inserting it.

```text
Outbox Event
     │
     ▼
Does Order exist?
   /       \
 Yes        No
  │          │
Skip       Insert
```

This prevents duplicate records in the Read Database.

---

# Technologies

* .NET 8
* ASP.NET Core Web API
* C#
* Entity Framework Core 8
* SQL Server
* MediatR
* CQRS
* Outbox Pattern
* BackgroundService
* Clean Architecture principles

---

# Key Concepts

This repository focuses on understanding:

* CQRS
* Command / Query separation
* Write Models
* Read Models
* Separate Databases
* Eventual Consistency
* Outbox Pattern
* Background Processing
* At-Least-Once Processing
* Idempotency
* Transactions
* Clean Architecture
* Scaling considerations

---

# Why These Different Phases?

The project intentionally evolves from a simple architecture into a distributed architecture.

```text
Phase 1
Simple Monolith
      │
      ▼
Phase 2
Separate Read / Write DB
      │
      ▼
Phase 3
Microservices + Message Broker
```

The goal is not to introduce complexity from the beginning, but to understand **why and when each architectural pattern becomes useful**.

---

# Current Progress

* [x] Phase 1 — Simple Monolith
* [x] Phase 2 — Separate Read / Write Databases
* [ ] Phase 3 — Microservices
* [ ] RabbitMQ
* [ ] Distributed Event Processing
* [ ] Independent Service Scaling
* [ ] Docker Compose
* [ ] Failure and Retry Scenarios

---

# Learning Goal

This project is built as a practical laboratory for understanding distributed-system concepts through implementation rather than theory alone.

The main question behind the project is:

> **How does a system evolve from a simple monolith into a scalable distributed architecture, and what problems appear at each step?**

---

## License

This project is for educational purposes.
