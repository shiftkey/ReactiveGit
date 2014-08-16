# assumes Powershell 3+ due to $PSScriptRoot

if (Test-Path  $env:ProgramFiles\MSBuild\12.0\bin\) {
  $msbuild = "$env:ProgramFiles\MSBuild\12.0\bin\msbuild.exe"
} else {
  # TODO: fail this script
}

. $msbuild "$PSScriptRoot\ReactiveGit.sln" /target:Rebuild /property:Configuration=Release