<#
.SYNOPSIS
    Generates the fixture model with the DevartCodeGeneration templates, builds the result and checks the templates'
    refusals - the regression check of the templates.

.DESCRIPTION
    Fixture.efml is a small model exercising the template features the Damara consumers depend on: an aggregate root
    with capable classes, a capability stamped on the root's members, and a capability backed by a zero-or-one child
    entity (with a lookup navigation and a child collection of its own). The script

    1. copies Fixture.efml + Fixture.edps into a temporary folder beside this one - same depth, so the relative
       template path (..\..\DevartCodeGeneration\*.tmpl) still resolves - and runs Entity Developer's console there;
    2. replaces Generated\ with the output (CRLF, UTF-8 BOM, as Visual Studio writes them);
    3. builds Damara.Fixture.csproj, so the generated code must compile against Damara.Core;
    4. generates variants of the model the templates must refuse (a public backing navigation, a capability both
       stamped and backed, ...) and checks each fails with its message;
    5. lists the generated files that differ from the committed ones. With -Check it fails when any do: a template
       change must regenerate the fixture and commit the new output with it, so the review shows what changed.

.EXAMPLE
    .\Invoke-FixtureCodegen.ps1
    .\Invoke-FixtureCodegen.ps1 -Check
#>
[CmdletBinding()]
param(
    [switch] $Check,
    [string] $EntityDeveloper = (Join-Path $env:ProgramFiles 'Devart\EntityDeveloper\win64\Console\ed.exe')
)

$ErrorActionPreference = 'Stop'

$fixture = $PSScriptRoot
$generatedFolder = Join-Path $fixture 'Generated'
if (-not (Test-Path $EntityDeveloper)) { throw "$EntityDeveloper not found." }
$model = [IO.File]::ReadAllText((Join-Path $fixture 'Fixture.efml')) -replace "`r`n", "`n"

# Runs ed.exe on the given model text in a temporary folder beside the fixture and passes the folder and the log to
# $OnGenerated; the folder is deleted afterwards.
function Invoke-Generation([string] $ModelText, [scriptblock] $OnGenerated) {
    $work = Join-Path (Split-Path $fixture -Parent) ('.fixture-codegen-' + [guid]::NewGuid().ToString('N').Substring(0, 8))
    [void] [IO.Directory]::CreateDirectory($work)
    try {
        [IO.File]::WriteAllText((Join-Path $work 'Fixture.efml'), ($ModelText -replace "`n", "`r`n"))
        Copy-Item (Join-Path $fixture 'Fixture.edps') $work
        Push-Location $work
        try { $log = (& $EntityDeveloper Generate-Code --ModelFile (Join-Path $work 'Fixture.efml') 2>&1 | ForEach-Object { "$_" }) -join "`n" }
        finally { Pop-Location }
        & $OnGenerated $work $log
    }
    finally {
        if ([IO.Directory]::Exists($work)) { [IO.Directory]::Delete($work, $true) }
    }
}

# ed.exe exits 0 even when generation fails, so its log carries the verdict.
function Test-Failed([string] $Log) { $Log -match '\[ERROR\]|generation failed|Validation error|Exception' }

Write-Host 'Generating the fixture model...' -ForegroundColor Cyan
Invoke-Generation $model {
    param($work, $log)
    if (Test-Failed $log) {
        $log -split "`n" | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
        throw 'ed.exe Generate-Code failed.'
    }
    $files = @(Get-ChildItem $work -File -Filter '*.cs')
    if (Test-Path $generatedFolder) { Remove-Item $generatedFolder -Recurse -Force }
    [void] [IO.Directory]::CreateDirectory($generatedFolder)
    foreach ($file in $files) {
        $content = [IO.File]::ReadAllText($file.FullName) -replace "`r`n", "`n"
        [IO.File]::WriteAllText((Join-Path $generatedFolder $file.Name), ($content -replace "`n", "`r`n"), [Text.UTF8Encoding]::new($true))
    }
    Write-Host "  $($files.Count) file(s) generated into Generated\" -ForegroundColor DarkGray
}

Write-Host 'Building the fixture...' -ForegroundColor Cyan
dotnet build (Join-Path $fixture 'Damara.Fixture.csproj') --nologo -v quiet -clp:ErrorsOnly
if ($LASTEXITCODE -ne 0) { throw 'The generated fixture code does not build.' }
Write-Host '  Build succeeded' -ForegroundColor Green

# Models the templates must refuse: each replaces one fragment of Fixture.efml and expects a message in the log.
$labelOnRoot = @'
    <property name="Label" type="String" p1:ValidateRequired="false" p1:Guid="8fb1d381-e5e2-48d9-994d-271c44fe5a01" p1:DefaultValue="string.Empty" p1:Capabilities="Core">
      <column name="label" not-null="True" length="50" sql-type="nvarchar" p1:unicode="True" />
    </property>
'@ -replace "`r`n", "`n"
$refusals = @(
    @{ Name = 'a public backing navigation'; Expect = 'must have a non-public getter'
       From = 'p1:getter-modifier="Internal" p1:setter-modifier="Internal" p1:Guid="8b7d075d'; To = 'p1:Guid="8b7d075d' }
    @{ Name = 'a capability both stamped and backed'; Expect = 'is both stamped on members and backed'
       From = 'p1:Capabilities="Core">' + "`n" + '      <column name="code"'; To = 'p1:Capabilities="Core,Gear">' + "`n" + '      <column name="code"' }
    @{ Name = 'a validator on the backing child'; Expect = 'must not generate a validator'
       From = 'p1:GenerateFactory="True" p1:AggregateRootName="Widget">' + "`n" + '    <id name="Id" type="Guid" p1:ValidateRequired="true" p1:Guid="da0483ad'
       To = 'p1:GenerateFactory="True" p1:GenerateValidator="True" p1:AggregateRootName="Widget">' + "`n" + '    <id name="Id" type="Guid" p1:ValidateRequired="true" p1:Guid="da0483ad' }
    @{ Name = 'Capabilities stamps on the backing child'; Expect = 'cannot carry Capabilities stamps'
       From = 'p1:Guid="398fd467-6581-46c1-88fb-b4cdba8d1822">'; To = 'p1:Guid="398fd467-6581-46c1-88fb-b4cdba8d1822" p1:Capabilities="Core">' }
    @{ Name = 'a member name two composed capabilities store apart'; Expect = 'over different storage'
       From = '      <column name="code" not-null="True" length="20" sql-type="nvarchar" p1:unicode="True" />' + "`n" + '    </property>' + "`n"
       To = '      <column name="code" not-null="True" length="20" sql-type="nvarchar" p1:unicode="True" />' + "`n" + '    </property>' + "`n" + $labelOnRoot }
    @{ Name = 'a backing child without a factory parameter'; Expect = 'needs a factory and a repository'
       From = 'p1:Guid="6911cc94-bdce-4b49-9c27-7cd02f5ea30c" p1:GenerateFactoryParameter="True">'; To = 'p1:Guid="6911cc94-bdce-4b49-9c27-7cd02f5ea30c">' }
)

Write-Host 'Checking the refusals...' -ForegroundColor Cyan
$failures = 0
foreach ($refusal in $refusals) {
    if (-not $model.Contains($refusal.From)) { throw "Refusal '$($refusal.Name)': its fragment is no longer in Fixture.efml." }
    Invoke-Generation $model.Replace($refusal.From, $refusal.To) {
        param($work, $log)
        if ((Test-Failed $log) -and $log.Contains($refusal.Expect)) { Write-Host "  refused $($refusal.Name)" -ForegroundColor DarkGray }
        else {
            Write-Host "  NOT refused: $($refusal.Name) (expected '$($refusal.Expect)')" -ForegroundColor Red
            $script:failures++
        }
    }
}
if ($failures) { throw "$failures model(s) the templates must refuse were generated." }

$changes = @(git -C $fixture status --porcelain -- Generated)
foreach ($change in $changes) { Write-Host "  $change" -ForegroundColor Yellow }
if ($changes.Count -eq 0) { Write-Host '  Generated output matches the committed fixture' -ForegroundColor Green }
elseif ($Check) { throw "$($changes.Count) generated file(s) differ from the committed fixture - regenerate and commit them with the template change." }
