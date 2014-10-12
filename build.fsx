#r @"tools\FAKE.Core\tools\FakeLib.dll"
open Fake 
open System

let buildDir = "./ReactiveGit/bin"

let releaseNotes = 
    ReadFile "ReleaseNotes.md"
    |> ReleaseNotesHelper.parseReleaseNotes

let buildMode = getBuildParamOrDefault "buildMode" "Release"

Target "Clean" (fun _ ->
    CleanDirs [ buildDir ]
)

open Fake.AssemblyInfoFile

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
      [ Attribute.Product "ReactiveGit"
        Attribute.Version releaseNotes.AssemblyVersion
        Attribute.FileVersion releaseNotes.AssemblyVersion
        Attribute.ComVisible false ]
)

Target "Build" (fun _ ->
    MSBuild null "Build" ["Configuration", buildMode] ["./ReactiveGit.sln"]
    |> Log "Build-Output: "
)

Target "Package" (fun _ ->
    NuGet (fun p -> 
        {p with
            Version = releaseNotes.AssemblyVersion
            ReleaseNotes = toLines releaseNotes.Notes
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" }) "nuspec/ReactiveGit.nuspec"
)

Target "SourcePackage" (fun _ ->

    NuGet (fun p -> 
        {p with
            Version = releaseNotes.AssemblyVersion
            ReleaseNotes = toLines releaseNotes.Notes
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" }) "nuspec/ReactiveGit.Source.nuspec"
)

Target "Default" DoNothing

Target "CreatePackages" DoNothing

"Clean"
   ==> "AssemblyInfo"
   ==> "CheckProjects"
       ==> "BuildApp"

"UnitTests"
   ==> "Default"

"ConventionTests"
   ==> "Default"

"IntegrationTests"
   ==> "Default"

"CreateOctokitPackage"
   ==> "CreatePackages"
"CreateOctokitReactivePackage"
   ==> "CreatePackages"

RunTargetOrDefault "Default"
