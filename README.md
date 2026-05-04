# F# Giraffe Template

A minimal starter template for building medium-sized web applications with F# and Giraffe. This template provides a clean foundation for rapid development while remaining lightweight and extensible.

## Features

- **Backend**: F# with Giraffe web framework
- **Frontend**: Razor pages with Tailwind CSS
- **Serialization**: Thoth.Json for JSON handling

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Architecture

### Backend (FsharpGiraffeTemplate.Api)

The API project uses Giraffe for handling HTTP requests with a functional approach:

- **Giraffe**: Functional web framework for F#
- **Thoth.Json**: Type-safe JSON serialization

### Frontend (FsharpGiraffeTemplate.Website)

The website project combines server-side rendering with modern styling:

- **Razor Pages**: Server-side templating with F#
- **Tailwind CSS**: Utility-first CSS framework
- **Static Assets**: Served from wwwroot directory

## What's not in that template

This template is intentionally minimal. Consider adding these features as your project grows:

### Database Integration
- **npgsql** - PostgreSQL driver
- **SQLite** - Lightweight database
- **SqlHydra** - Type-safe SQL

### Logging
- **Serilog** - Structured logging
- **Microsoft.Extensions.Logging** - Built-in logging

### Testing
- **Expecto** - F# testing framework
- **Hedgehog** - Property-based testing
- **FSCheck** - QuickCheck for F#

### Architecture
- **FsharpGiraffeTemplate.App** - Shared DTOs and business logic
- **Domain-Driven Design** - Rich domain models
- **CQRS** - Command Query Responsibility Segregation
