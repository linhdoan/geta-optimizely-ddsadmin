$outputDir = "c:\NuGetLocal\"
$build = "Release"
$version = "11"

.\.nuget\NuGet.exe pack .\src\Geta.DdsAdmin.csproj -IncludeReferencedProjects -properties Configuration=$build -Version $version -OutputDirectory $outputDir
