# assumes Powershell 3+ due to $PSScriptRoot
$root = $PSScriptRoot
$version = "0.0.2"

if (Test-Path  $env:ProgramFiles\MSBuild\12.0\bin\) {
  $msbuild = "$env:ProgramFiles\MSBuild\12.0\bin\msbuild.exe"
} else {
  # TODO: fail this script
}

# ensure you're running in the script root
Push-Location -Path $root

# build the thing
.\tools\nuget.exe restore .\ReactiveGit.sln
. $msbuild .\ReactiveGit.sln /target:Rebuild /property:Configuration=Release

# TODO: run the tests

# create the packages
.\tools\nuget.exe pack .\nuspec\ReactiveGit.nuspec -Version $version
.\tools\nuget.exe pack .\nuspec\ReactiveGit.Source.nuspec -Version $version

Pop-Location