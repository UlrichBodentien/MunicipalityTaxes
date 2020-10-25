# MuncipalityTaxes

## Setting up the project

### Producer
Note: Normally, I would never invlude connection strings in the source code. For the sake of simplicity, I left the connection string, so it would be as easy as possible to test

#### Running from visual studio
Set as Start project and start. You can also set both the Producer and the Consumer as startup projects at once, in the solution's properties, to debug both simultaniously

#### Installing as a service
The producer project can be installed as a service.<br/>
Install it by running install-as-service.ps1.</br>
The API will run on http://localhost:5000 </br>
To uninstall run uninstall.ps1

### Consumer
To run the consumer service, just set it as the startup project.<br/>
The CSV file for data import is embedded in the source code, but can be modified for testing purposes.</br>
Make sure the ApiUrl found in Program.cs points to the correct port.<br/>
If running from Visual Studio the url is https://localhost:44320 <br/>
If running as a service the url is http://localhost:5000