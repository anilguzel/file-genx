# DistributedFileStorage

This repository contains a simple example application that enables splitting files into chunks for storage and later reconstruction. The goal is to build a multi-layered architecture aligned with Clean Architecture principles, allowing storage providers to be easily interchangeable.

## Layers and Technologies

- **Core**: Contains fundamental models, interfaces, and service definitions.
- **Infrastructure**: Handles file chunking, storage providers, and file reconstruction.
- **Persistence**: Uses EF Core to store metadata in a SQLite database.
- **App**: Console application with dependency injection configuration.

The project uses **.NET 8**, **Entity Framework Core**, and **Serilog**.  
Metadata is stored in a persistent SQLite database named `chunks.db`, and file data is written to the `chunks` folder by default.

## Key Features

- `DynamicFileChunker`: Splits files into chunks of variable size depending on file size.
- `FileSystemStorageProvider` and `DatabaseStorageProvider`: Offer different storage backends.
- `FileUploadService`: Saves chunks to the selected provider and creates metadata.
- `FileRebuilder`: Reconstructs the original file from stored chunks and verifies it using a checksum.
- **Serilog** is used for logging important steps to the console.

## Setup

To build and run the project, [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required.

```bash
dotnet build
dotnet run --project src/DistributedFileStorage.App
