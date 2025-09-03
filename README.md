
# HR Authorization Proof of Concept

This repository demonstrates separating authentication (AuthN) and authorization (AuthZ) in an ASP.NET Core **.NET 6** API using [Casbin](https://casbin.org/).

## Architecture

* **Authentication (AuthN):**
  * JWT Bearer tokens are validated using the configured `Authority` and `Audience` in `Program.cs`.
  * Successful validation establishes the caller's identity (`sub`).
* **Authorization (AuthZ):**
  * [Casbin.NET](https://github.com/casbin-net) enforces domain based RBAC rules.
  * The model is defined in `CasbinModel/rbac_with_domains.conf` and policies in `CasbinPolicy/policy.csv`.
  * Endpoints consult the `IEnforcer` to decide whether a request is allowed.

## Endpoints

* `GET /authz/check?dom=&obj=&act=` – returns **204** when access is allowed and **403** otherwise.
* `GET /api/leave/{id}?dom=` – requires `(dom, api.leave, read)`.
* `POST /api/leave/{id}/approve?dom=` – requires `(dom, api.leave, approve)`.

## Running

1. Install the .NET 6 SDK.
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the API:
   ```bash
   dotnet run --project HRAuthorization.Api
   ```
4. Execute tests:
   ```bash
   dotnet test
   ```

The Casbin model and policy files can be edited to experiment with different authorization rules.
