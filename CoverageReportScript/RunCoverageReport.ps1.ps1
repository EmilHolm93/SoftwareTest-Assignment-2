#!/usr/bin/env pwsh

param (
    [Parameter(Mandatory=$true)]
    [string]$TestProject
)

if (-not $TestProject) {
    Write-Host "Usage: $PSCommandPath test-project-with-coverage"
    Write-Host "Example: $PSCommandPath MyCalculator.Lib.Tests.UnitTest"
    exit 1
}

# Run dotnet test with coverage collection
dotnet test $TestProject --collect:"XPlat Code Coverage" --logger:"junit;MethodFormat=Class;FailureBodyFormat=Verbose"

# Find coverage.cobertura.xml file
$CoverageFiles = Get-ChildItem -Path "C:\Users\agadg\Documents\Skolearbejde\4. semester\SWT\swt-group6-handin2\MobileChargingStation.Lib.Tests\TestResults\" -Filter "coverage.cobertura.xml" -Recurse | Select-Object -ExpandProperty FullName | Sort-Object CreationTime -Descending | Select-Object -First 1

if (-not $CoverageFiles) {
    Write-Host "Error: No coverage.cobertura.xml file found."
    exit 1
}

# Get the path to ReportGenerator.dll
$ReportGeneratorPath = Get-ChildItem -Path "$env:USERPROFILE\.nuget\packages\reportgenerator\*\tools\net8.0\ReportGenerator.dll" | Select-Object -ExpandProperty FullName

# Run ReportGenerator
dotnet $ReportGeneratorPath -reports:$CoverageFiles -targetdir:coveragereport -reporttypes:"Html;TextSummary"

# Display coverage summary
Get-Content "coveragereport/Summary.txt"
