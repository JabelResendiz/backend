#tener instalado la herramienta de dotnet ef globalmente para ejecutar los comandos
# dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialMigration99090 --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet ef database update --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet run --project ./Finlay.PharmaVigilance.API/