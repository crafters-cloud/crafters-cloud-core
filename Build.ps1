# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
        [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0)
    {
        throw ("Exec: " + $errorMessage)
    }
}

$artifacts = ".\artifacts"

if (Test-Path $artifacts)
{
    Remove-Item $artifacts -Force -Recurse
}

exec { & dotnet clean -c Release }

exec { & dotnet build -c Release }

exec { & dotnet test -c Release --no-build -l trx --verbosity=normal }

exec { & dotnet pack .\src\Core\Core.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\AspNetCore\AspNetCore.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\AspNetCore.Tests.SystemTextJson\AspNetCore.Tests.SystemTextJson.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\AspNetCore.TestUtilities\AspNetCore.TestUtilities.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\EntityFramework\EntityFramework.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\EntityFramework.Infrastructure\EntityFramework.Infrastructure.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\EventBus\EventBus.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\HealthChecks\HealthChecks.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\Infrastructure\Infrastructure.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\IntegrationEvents\IntegrationEvents.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\Core.MediatR\Core.MediatR.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\SmartEnums\SmartEnums.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\SmartEnums.EntityFramework\SmartEnums.EntityFramework.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\SmartEnums.Swagger\SmartEnums.Swagger.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\SmartEnums.SystemTextJson\SmartEnums.SystemTextJson.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\SmartEnums.VerifyTests\SmartEnums.VerifyTests.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\Swagger\Swagger.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\TestUtilities\TestUtilities.csproj -c Release -o $artifacts --no-build }



