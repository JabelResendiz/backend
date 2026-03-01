```powershell

dotnet ef migrations add InitialMigration99090 --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet ef database update --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet run --project .\Finlay.PharmaVigilance.API\

dotnet ef migrations list --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API 

dotnet ef migrations remove --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet ef database update InitialMigration_1 --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API


dotnet ef migrations add ConfigureDepartmentCompositeKey
dotnet ef database update

dotnet run --project .\Finlay.PharmaVigilance.API\


```
