# DfE NCS Course Mock API

A .NET 10 Azure Functions app that serves mock course and T Level data for the National Careers Service (NCS), for use in local development and integration testing.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools v4](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [Postman](https://www.postman.com/downloads/)

## Running Locally

Navigate to the function project and start the host:

    cd DfE.NCS.Course.Mock.Api/DfE.NCS.Course.Mock.Function
    func start

API base URL: `http://localhost:7071/api`

---

## Postman

All Postman files are in the `postman/` folder:

    postman/
      collections/
        CourseDetailsMockAPI.postman_collection.json
      environments/
        dfe-ncs-local.postman_environment.json
        dfe-ncs-dev.postman_environment.json
      globals/
        workspace.postman_globals.json

**To import into Postman:**

1. Open Postman and click **Import** (top-left)
1. Select `Connect Local Git Repo`
1. Select Git folder
1. Postman should automatically detect the collection, environment, and globals files. Select all and click **Import**.


---

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/courses/list` | Paginated list of courses |
| GET | `/api/courses/updates` | Courses updated since a cutoff date |
| GET | `/api/t-levels/list` | Paginated list of T Levels |
| GET | `/api/t-levels/updates` | T Levels updated since a cutoff date |

### Query Parameters

**`/api/courses/list`** and **`/api/t-levels/list`**

| Parameter | Default | Description |
|-----------|---------|-------------|
| `pageNumber` | `1` | Page number (1-based) |
| `pageSize` | `10` | Results per page |
| `totalCount` | `100` | Total records to generate |

**`/api/courses/updates`** and **`/api/t-levels/updates`**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `cutOffDate` | Yes | ISO 8601 date — returns records updated on or after this date |
| `pageNumber` | No | Page number, default `1` |
| `pageSize` | No | Results per page, default `10` |
| `totalCount` | No | Total records to generate, default `100` |
