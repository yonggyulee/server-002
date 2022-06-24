// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis;
using Mirero.DAQ.Test.Custom.Sykim;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt").CreateLogger();
Log.Information("Starting up");

//AccountTest.Test();

