properties {
	$runRebuildB = $false
}

Task RebuildDb -precondition { $runRebuildDb -eq $true }{
	Write-Host "Rebuilding PMR Database"
	exec { SQLCMD.exe -E -S localhost -Q "IF EXISTS (SELECT name FROM sys.databases where name= N'tanktemp') BEGIN ALTER DATABASE [tanktemp] SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE [tanktemp] END"}
	exec {sqlcmd -E -S localhost -Q "CREATE DATABASE [tanktemp]"}
}

Task MigrateDb -depends RebuildDb{
	exec { .\packages\FluentMigrator.1.0.6.0\tools\Migrate.exe /connection "Data Source=localhost;Initial Catalog=tanktemp;Integrated Security=SSPI;" /db sqlserver2005 /target ".\TankTempWeb\bin\TankTempWeb.dll"}
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