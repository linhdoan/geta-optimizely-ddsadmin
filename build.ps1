$outputDir = "c:\NuGetLocal\"
$build = "Release"
$version = "11.15"

.\.nuget\NuGet.exe pack .\src\Geta.DdsAdmin.nuspec -properties Configuration=$build -Version $version -OutputDirectory $outputDir
