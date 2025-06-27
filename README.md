# Claims and Covers API

This project implements a RESTful API for managing **Claims** and **Covers** in an insurance domain.  
It provides endpoints for creating, retrieving, deleting, and computing insurance-related data such as claims and cover premiums.

---

## Overview

The API consists of two main controllers:

- **ClaimsController**: Manages claim operations.
- **CoversController**: Manages cover operations including premium computation.

---

## Technologies

- .NET 9 (or latest)
- ASP.NET Core Web API
- Dependency Injection for service layers
- Asynchronous programming with `async/await`

---

## Controllers and Endpoints

### ClaimsController

Manages CRUD operations for claims.

| HTTP Method | Route           | Description            | Request Body    | Response                            |
|-------------|-----------------|------------------------|-----------------|-----------------------------------|
| GET         | `/claims`       | Retrieve all claims     | None            | List of `ClaimResponse`            |
| GET         | `/claims/{id}`  | Retrieve claim by ID    | None            | `ClaimResponse` or `null`          |
| POST        | `/claims`       | Create a new claim      | `ClaimRequest`  | Created `ClaimResponse` or errors  |
| DELETE      | `/claims/{id}`  | Delete claim by ID      | None            | `204 No Content` or `404 Not Found`|

---

### CoversController

Manages CRUD operations for covers and premium computation.

| HTTP Method | Route                 | Description                | Request Body / Params                       | Response                             |
|-------------|-----------------------|----------------------------|--------------------------------------------|------------------------------------|
| POST        | `/covers/compute`     | Compute premium for a cover| Query params: `startDate`, `endDate`, `coverType` | Computed premium or errors          |
| GET         | `/covers`             | Retrieve all covers         | None                                       | List of `CoverResponse`             |
| GET         | `/covers/{id}`        | Retrieve cover by ID        | None                                       | `CoverResponse` or `null`           |
| POST        | `/covers`             | Create a new cover          | `CoverRequest`                             | Created `CoverResponse` or errors   |
| DELETE      | `/covers/{id}`        | Delete cover by ID          | None                                       | `204 No Content` or `404 Not Found`|

---

## Models (Summary)

- **ClaimRequest**: Data required to create a claim.
- **ClaimResponse**: Data returned for a claim.
- **CoverRequest**: Data required to create a cover.
- **CoverResponse**: Data returned for a cover.
- **ComputePremiumResult**: Result of premium computation including success flag and errors.
- **CoverType**: Enum representing different cover types.

---

## Services

Business logic is handled by the following service interfaces:

- `IClaimService`: Claim-related operations.
- `ICoverService`: Cover-related operations, including premium computation.