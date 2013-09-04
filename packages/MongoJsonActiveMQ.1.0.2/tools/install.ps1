param($installPath, $toolsPath, $package, $project)
$dte.ItemOperations.OpenFile($toolsPath + '\DependencyInformation.txt')

if ($host.Version.Major -eq 1 -and $host.Version.Minor -lt 1)
{
    "NOTICE: This package only works with NuGet 1.1 or above. Please update your NuGet install at http://nuget.codeplex.com. Sorry, but you're now in a weird state. Please 'uninstall-package AddMvc3ToWebForms' now."
}

