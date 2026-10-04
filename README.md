# CommBank Server

An ASP.NET Core banking application learning fork of [fencer-so/CommBank-Server](https://github.com/fencer-so/CommBank-Server).
The source contains account, transaction, savings-goal, tag, and user services backed by MongoDB.

## Local setup

The project targets .NET 6. Its startup code reads a local `Secrets.json` file and a `ConnectionStrings:CommBank` value.
Configure a disposable MongoDB database before running the server. Startup also runs seed logic.

```sh
dotnet test Server.sln
```

The application entry point is `CommBank-Server/Program.cs`.

## Scope

This is a fork for learning. Upstream code is not presented as original work.
Authentication and access controls need review before any deployment or use with real records.

