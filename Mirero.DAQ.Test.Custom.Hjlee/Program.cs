// See https://aka.ms/new-console-template for more information

using Mirero.DAQ.Test.Custom.Hjlee;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt").CreateLogger();

new WorkflowTest().Test();