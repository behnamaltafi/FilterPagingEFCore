Dynamic Filtering, Sorting, and Paging Extension Method

This repository contains a C# extension method designed to facilitate dynamic filtering, sorting, and paging on IQueryable data sources. It provides a flexible solution for handling these common operations in a generic and reusable manner.
Features

    Dynamic Filtering: Filter data based on dynamic filter parameters provided.
    Dynamic Sorting: Sort data based on dynamic sorting parameters.
    Paging: Retrieve paginated results from the data source.

Usage
Installation

The extension method is written in C# and can be used in any .NET project. To use it, follow these steps:

    Copy the FilterPaging.cs file into your project.
    Ensure that the necessary dependencies are included.

Example

csharp

// Import the namespace where the extension method is defined
using YourNamespace;

// Usage example
var filteredData = yourQueryableData.FilterPaging(yourPagingParam);

Parameters

    data: An IQueryable<T> data source to be filtered, sorted, and paged.
    pagingFilter: A PagingParam object containing filter, sort, and paging parameters.

Requirements

    .NET Framework or .NET Core environment.
    Compatible with C# projects.

Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, please feel free to open an issue or create a pull request.
License

This project is licensed under the MIT License.
