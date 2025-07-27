param (
  [Parameter(Mandatory = $true, ParameterSetName = 'Bump')]
  [ValidateSet('Major', 'Minor', 'Patch')]
  [string]
  $Bump,
  
  [Parameter(Mandatory = $true, ParameterSetName = 'PreRelease')]
  [ValidateSet('Increment', 'Off')]
  [string]
  $PreRelease
)
  
function UpdateNode {
  param (
    [System.Xml.XmlDocument]
    $doc,
    
    [string]
    $nodeName,

    [string]
    $oldValue,

    [string]
    $newValue,

    [ref]
    $changeCount
  )

  # Don't update if there is no change in the value
  if ($newValue -eq $oldValue) {
    return
  }
  
  # Iterate over nodes matching the node name
  foreach ($node in $doc.SelectNodes(".//$nodeName")) {

    # Don't update if the current node value is different from the old value
    if ($node.InnerText -eq $newValue) {
      continue
    }

    $changeCount.Value++
    $node.InnerText = $newValue
  }
}

# Main script
$configFileName = Join-Path $PSScriptRoot ReleaseConfig.xml
$config = [xml](Get-Content -Path ($configFileName))

$majorVersion = [int]$config.Config.Versions.Major
$minorVersion = [int]$config.Config.Versions.Minor
$patchVersion = [int]$config.Config.Versions.Patch
$prereleaseVersion = [int]$config.Config.Versions.Prerelease

if ($Bump) {
  $prereleaseVersion = 0

  switch ($Bump) {
    'Major' {
      $majorVersion = $majorVersion + 1
      $minorVersion = 0
      $patchVersion = 0
      $prereleaseVersion = 0
    }
    'Minor' {
      $minorVersion = $minorVersion + 1
      $patchVersion = 0
      $prereleaseVersion = 0
    }
    'Patch' {
      $patchVersion = $patchVersion + 1
      $prereleaseVersion = 0
    }
  }
}

if ($PreRelease -eq 'Increment') {
  $prereleaseVersion++
}

if ($PreRelease -eq 'Off') {
  $prereleaseVersion = 0
}

# Rewrite values
$config.Config.Versions.Major = $majorVersion.ToString()
$config.Config.Versions.Minor = $minorVersion.ToString()
$config.Config.Versions.Patch = $patchVersion.ToString()
$config.Config.Versions.Prerelease = $prereleaseVersion.ToString()

# Get old values
$oldVersion = $config.Config.ProjectProperties.Version
$oldAssemblyVersion = $config.Config.ProjectProperties.AssemblyVersion
$oldFileVersion = $config.Config.ProjectProperties.FileVersion
$oldPublishImageTag = $config.Config.PublishProperties.PublishImageTag

# Update properties
$config.Config.ProjectProperties.Version = "$majorVersion.$minorVersion.$patchVersion"
$config.Config.ProjectProperties.AssemblyVersion = "$majorVersion.$minorVersion.$patchVersion"
$config.Config.ProjectProperties.FileVersion = "$majorVersion.$minorVersion.$patchVersion"

if ($prereleaseVersion -ne 0) {
  $config.Config.PublishProperties.PublishImageTag = "$majorVersion.$minorVersion.$patchVersion-beta.$prereleaseVersion"
}
else {  
  $config.Config.PublishProperties.PublishImageTag = "$majorVersion.$minorVersion.$patchVersion"
}

$config.Save($configFileName)

"Updated product versions. New versions are:"
"Version: $($config.Config.ProjectProperties.Version)"
"AssemblyVersion: $($config.Config.ProjectProperties.AssemblyVersion)"
"FileVersion: $($config.Config.ProjectProperties.FileVersion)"
"PublishImageTag: $($config.Config.PublishProperties.PublishImageTag)"

# Read CSPROJ files
'Getting *.csproj'
$csprojFiles = Get-ChildItem -Path "$PSScriptRoot\*.csproj" -File -Recurse -Depth 0
foreach ($file in $csprojFiles) {
  if ($file.Name.EndsWith('- Backup.csproj')) {
    continue
  }

  $file.FullName

  $doc = [System.Xml.XmlDocument]::new()
  $doc.PreserveWhitespace = $true
  $doc.Load($file.FullName)

  [ref]$changeCount = 0
  UpdateNode $doc 'Version' $oldVersion $config.Config.ProjectProperties.Version $changeCount
  UpdateNode $doc 'AssemblyVersion' $oldAssemblyVersion $config.Config.ProjectProperties.AssemblyVersion $changeCount
  UpdateNode $doc 'FileVersion' $oldFileVersion $config.Config.ProjectProperties.FileVersion $changeCount

  if ($changeCount.Value -gt 0 ) {
    $doc.Save($file.FullName)
  }
}

# Read PUBXML files
'Getting *.pubxml'
$pubxmlFiles = Get-ChildItem -Path "$PSScriptRoot\*.pubxml" -File -Recurse -Depth 0
foreach ($file in $pubxmlFiles) {
  $file.FullName

  $doc = [System.Xml.XmlDocument]::new()
  $doc.PreserveWhitespace = $true
  $doc.Load($file.FullName)

  [ref]$changeCount = 0
  UpdateNode $doc 'PublishImageTag' $oldPublishImageTag $config.Config.PublishProperties.PublishImageTag $changeCount

  if ($changeCount.Value -gt 0 ) {
    $doc.Save($file.FullName)
  }
}