[CmdletBinding()]
param (
    [string] $outputFile
)

$files = Get-Childitem -Recurse -File
$files += Get-ChildItem -Recurse -File -Path .github/workflows

function packageEcosystem() {
    param (
        [string] $ecosystem,
        [string] $relPath,
        [string] $interval = "daily" # default = weekly
    )

    $block = @"
- package-ecosystem: "$ecosystem"
  directory: "$relPath"
  schedule:
    interval: "$interval" 
"@

    return $block
}

$output = @"
# This file is auto-generated by .github/scripts/update-dependabot.ps1
version: 2
updates:
"@

foreach ($file in $files) {
    $relPath = Resolve-Path -relative $($file.FullName) | Split-Path -Parent 
    $relPath = $relPath -replace '\./', '/' # replace leading ./ with /

    # Terraform
    if ($file.Name -eq 'main.tf') {
        Write-Host "Found main.tf in $($file.FullName)"
        $ecosystem = "terraform"
        $block = packageEcosystem -ecosystem $ecosystem `
                                  -relpath $relPath
        $output += "`r`n"+$block+"`r`n"

    # Docker
    } elseif ($file.Name -eq 'Dockerfile') {
        Write-Host "Found Dockerfile in $($file.FullName)"
        $ecosystem = "docker"
        $block = packageEcosystem -ecosystem $ecosystem `
                                  -relpath $relPath
        $output += "`r`n"+$block+"`r`n"

    # NPM
    } elseif ($file.Name -eq 'package.json') {
        Write-Host "Found package.json in $($file.FullName)"
        $ecosystem = "npm"

        $block = packageEcosystem -ecosystem $ecosystem `
                                  -relpath $relPath

        # NPM uses a customized package-ecosystem block                          
        $block += "`r`n"+@"
  allow:
  - dependency-type: direct
  - dependency-type: production # check only dependencies, which are going to the compiled app, not supporting tools like @vue-cli
"@
        $output += "`r`n"+$block+"`r`n"

    # NuGET / dotNet
    } elseif ($file.Name -like '*.csproj') {
        Write-Host "Found *.csproj in $($file.FullName)"
        $ecosystem = "nuget"
        $block = packageEcosystem -ecosystem $ecosystem `
                                  -relpath $relPath
        $output += "`r`n"+$block+"`r`n"

    # GitHub actions
    } elseif ($file.FullName -like '*.github/workflows*' -and ($githubAction -ne $true)) {
        Write-Host "Found *.github/workflows* in $($file.FullName)"
        $ecosystem = "github-actions"
        $githubAction = $true
        $block = packageEcosystem -ecosystem $ecosystem `
                                  -relpath "/"
        $output += "`r`n"+$block+"`r`n"
    }
}

if ($outputFile -ne "") {
    Write-Host "*** Writing output to $outputFile"
    $output | Out-file -FilePath $outputFile -Encoding UTF8
} else {
    Write-Host $output
}