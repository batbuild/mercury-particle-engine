// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile


RestorePackages()

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"
let packagesDir = @".\packages"


// Project info
let authors = ["Andrea Magnorsky";"Andrew O'Connor"; "Matt Davey"]
let projectName = "MercuryParticleEngine"
type ProjectInfo = { 
    Name: string;
    Description: string; 
    Version: string;
  }
let info = {
  Name= projectName
  Description =  "Mercury particle engine for android (used in Duality)"
  Version = if isLocalBuild then "0.2-local" else "0.2."+buildVersion
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./Properties/AssemblyInfo.cs"
        [Attribute.Title info.Name
         Attribute.Description info.Description
         Attribute.Guid "c1dcbc84-7e8b-46f3-a253-9d9527434dee"         
         Attribute.Version info.Version
         Attribute.FileVersion info.Version]
)

Target "Compile" (fun _ ->          
    log "This step just logs this message"  
)

Target "CompileAndroid" (fun _ ->          
    let buildMode = getBuildParamOrDefault "buildMode" "Release"
    let setParams defaults =
        { defaults with
            Verbosity = Some(Normal)
            Targets = ["Build"]
            Properties =
                [
                    "Optimize", "True"                    
                    "Configuration", buildMode
                    "AllowUnsafeBlocks", "True"
                ]
        }
    build setParams "./Mercury.ParticleEngine.Android.sln"    
    |> DoNothing
)

Target "CompileTest" (fun _ ->
    !! @"**\Test*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->
    !! (testDir + @"\Test*.dll")
      |> NUnit (fun p ->
                 {p with
                   DisableShadowCopy = true;
                   OutputFile = testDir + @"TestResults.xml"})
)

let nugetPath = ".build/NuGet.exe"

Target "CreatePackage" (fun _ ->        
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName            
            Version = info.Version
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description                               
            PublishUrl = getBuildParamOrDefault "nugetrepo" ""
            AccessKey = getBuildParamOrDefault "nugetkey" ""            
            Publish = hasBuildParam "nugetkey"  
            }) 
            "DOESNTEXIST.0.1.0.0.nuspec"
)

Target "AndroidPack" (fun _ ->    
    
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName+".Android"
            Version = info.Version
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description            
            Tags = "Coroutine library C# for Android"           
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
            PublishUrl = getBuildParamOrDefault "nugetUrl" ""
            }) 
            "nuget/MercuryParticleEngine.Android.nuspec"
)


// Dependencies
"Clean"
  ==> "SetVersions"
  ==> "Compile"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "CreatePackage"

"Clean"
  ==> "SetVersions"
  ==> "CompileAndroid"
  ==> "AndroidPack"

// start build
RunTargetOrDefault "AndroidPack"