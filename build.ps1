# assumes Powershell 3+ due to $PSScriptRoot
$root = $PSScriptRoot

# ensure you're running in the script root
Push-Location -Path $root

# grab FAKE
.\tools\nuget.exe install FAKE.Core -OutputDirectory tools -ExcludeVersion -version 3.5.8

# build the thing
.\tools\nuget.exe restore .\ReactiveGit.sln

.\tools\FAKE.Core\tools\Fake.exe build.fsx target=Package

Pop-Location