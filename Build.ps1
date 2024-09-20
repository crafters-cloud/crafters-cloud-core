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

exec { & dotnet pack .\src\CraftersCloud.Core\CraftersCloud.Core.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.AspNetCore\CraftersCloud.Core.AspNetCore.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.EntityFramework\CraftersCloud.Core.EntityFramework.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.EntityFramework.Infrastructure\CraftersCloud.Core.EntityFramework.Infrastructure.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.EventBus\CraftersCloud.Core.EventBus.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.HealthChecks\CraftersCloud.Core.HealthChecks.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.Infrastructure\CraftersCloud.Core.Infrastructure.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.MediatR\CraftersCloud.Core.MediatR.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.Swagger\CraftersCloud.Core.Swagger.csproj -c Release -o $artifacts --no-build }
exec { & dotnet pack .\src\CraftersCloud.Core.TestUtils\CraftersCloud.Core.TestUtils.csproj -c Release -o $artifacts --no-build }



