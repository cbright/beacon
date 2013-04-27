properties {
	$runRebuildB = $false
}

Task RebuildDb -precondition { $runRebuildDb -eq $true }{
	Write-Host "Rebuilding PMR Database"
	
	#exec { SQLCMD.exe -S tcp:hzi8wcjzti.database.windows.net,1433 -d tanktemAquT08eiy -U cbright -P Sb9ha1s7
	exec { SQLCMD.exe -E -S localhost -Q "IF EXISTS (SELECT name FROM sys.databases where name= N'tanktemp') BEGIN ALTER DATABASE [tanktemp] SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE [tanktemp] END"}
	exec {sqlcmd -E -S localhost -Q "CREATE DATABASE [tanktemp]"}
}

Task MigrateDb -depends RebuildDb{
	exec { .\packages\FluentMigrator.1.0.6.0\tools\Migrate.exe /connection "Server=tcp:hzi8wcjzti.database.windows.net,1433;Database=tanktemAquT08eiy;User ID=cbright@hzi8wcjzti;Password=Sb9ha1s7;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;" /db sqlserver2005 /target ".\TankTempWeb\bin\TankTempWeb.dll"}
}

Task Package {

	if(Test-Path $dest){
		Remove-Item $dest -Force -Recurse
	}
	exec { robocopy $src $dest /s /purge /xf '*.cs' '*.csproj' '*.user' '*.obj' '*packagegs.config' /xd 'obj' 'Logs' }
}

Task Clean {
	$packageDir = "Output"
	$Logs = "Logs"
	

	if(Test-Path $packageDir){
		Remove-Item $packageDir -Force -Recurse
	}
	
	if(Test-Path $Logs){
		Remove-Item $Logs -Force -Recurse
	}
}