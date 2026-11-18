@echo off
REM ============================================================
REM  RepairIT - lance tous les projets de la solution
REM  Chaque projet s'ouvre dans sa propre fenetre de commande.
REM ============================================================
cd /d "%~dp0"

echo Compilation de la solution...
dotnet build RepairIT.sln
if errorlevel 1 (
    echo La compilation a echoue : corrigez les erreurs puis relancez.
    pause
    exit /b 1
)

wt -w RepairIT --title "Catalog 5101" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.CatalogService ^
 ; new-tab --title "Users 5102" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.UsersService ^
 ; new-tab --title "Repair 5103" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.RepairService ^
 ; new-tab --title "Sales 5104" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.SalesService ^
 ; new-tab --title "Customers 5105" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.CustomersService ^
 ; new-tab --title "Auth 5106" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.AuthentificationService ^
 ; new-tab --title "Gateway 5010" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.Gateway ^
 ; new-tab --title "Client 5000" --suppressApplicationTitle -d "%CD%" cmd /k dotnet run --no-build --launch-profile http --project RepairIT.Client

echo Tous les projets sont lances. Front : http://localhost:5000
start "" http://localhost:5000