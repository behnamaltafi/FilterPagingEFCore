# Dynamic Filtering, Sorting, and Paging Extension Method

This repository provides a C# extension method designed to facilitate dynamic filtering, sorting, and paging on `IQueryable` data sources. It offers a flexible solution for handling these common operations in a generic and reusable manner.

## Features

- **Dynamic Filtering**: Filter data based on dynamic filter parameters provided.
- **Dynamic Sorting**: Sort data based on dynamic sorting parameters.
- **Paging**: Retrieve paginated results from the data source.

## Installation

You can install the package via NuGet. Run the following command in the Package Manager Console:

```sh
 Install-Package FilterPagingEFCore
```
## Usage
Example


```csharp
// Usage example
var filteredData = yourQueryableData.FilterPaging(yourPagingParam);
```
## Parameters

data: An IQueryable<T> data source to be filtered, sorted, and paged.
pagingFilter: A PagingParam object containing filter, sort, and paging parameters.

## Requirements

.NET Framework or .NET Core environment.
Compatible with C# projects.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, please feel free to open an issue or create a pull request.
