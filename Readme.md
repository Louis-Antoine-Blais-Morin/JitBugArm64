# JitBug64

## What is it

Program reproducing what seems to be a problem in compiler/JIT when running under Arm 64 with .NET Runtime 8.0.3.

## How to reproduce

- Open JitBugArm64.sln in VisualStudio
- Compile the program in Release
- Copy content on bin\Release\net8.0 on a platform running Linux and where .NET Runtime 8.0.3 is installed
- dotnet run JitBugArm64.dll
- An exception is thrown after few seconds


