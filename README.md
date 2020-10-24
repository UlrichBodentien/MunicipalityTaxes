# MuncipalityTaxes

## Running the project

### Producer
The producer project, requires a database<br/>
If you have a standard local MSSQLEXPRESS database instance running, the connectionstring works out of the box. Otherwise update it to match your local database, it is found in appsettings.json.<br/>
Set the core project as Default project in the Package Manager console, and run "Update-database" to run the migration

### Consumer
To run the consumer service, just set it as the startup project.<br/>
The CSV file for data import is embedded in the source code, but can be modified for testing purposes.