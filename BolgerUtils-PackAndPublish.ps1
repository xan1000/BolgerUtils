Set-Variable ApiKey -Option Constant -Value (Get-Content -Raw ../ApiKey.txt)
Set-Variable NuGetSource -Option Constant -Value "https://api.nuget.org/v3/index.json"
Set-Variable Options -Option Constant -Value @(
    "BolgerUtils",
    "BolgerUtils.EnumDisplay",
    "BolgerUtils.FileToObjectMapping",
    "BolgerUtils.TimeZoneConverter"
)

function Main {
    while($true) {
        for($i = 0; $i -lt $Options.length; $i++) {
            Write-Host "$($i + 1)    -> $($Options[$i])"
        }
        Write-Host "Quit"
        Write-Host

        $input = SelectAnOption

        if($input -ieq "quit") {
            return
        }

        $index = 0
        if(!([int]::TryParse($input, [ref] $index)) -or $index -lt 1 -or $index -gt $Options.length) {
            InvalidInput
            continue
        }
        
        $index--
        $option = $Options[$index]
        MenuOption $option
    }
}

function MenuOption {
    param(
        [Parameter(Mandatory=$true)]
        [string] $option
    )

    while($true) {
        Write-Host "--- $($option) ---"
        Write-Host "1    -> Pack"
        Write-Host "2    -> Publish"
        Write-Host "Back"
        Write-Host

        $input = SelectAnOption

        if($input -eq "1") {
            Pack $option

            $input = SelectAnOption "Publish $($option) package now (yes)? "
            if($input -ieq "yes") {
                Publish $option $false
            }
        }
        elseif($input -eq "2") {
            Publish $option
        }
        elseif($input -ieq "back") {
            return
        }
        else {
            InvalidInput
        }
    }
}

function Pack {
    param(
        [Parameter(Mandatory=$true)]
        [string] $option
    )

    Write-Host "--- Packing: $($option) ---"
    Write-Host

    Remove-Item "$($option).*.nupkg"

    $csprojPath = "./$($option)/$($option).csproj"

    dotnet pack $csprojPath -o (Get-Location).Path
    
    Write-Host
}

function Publish {
    param(
        [Parameter(Mandatory=$true)]
        [string] $option,
        [bool] $shouldConfirm = $true
    )

    Write-Host "--- Publishing: $($option) ---"
    Write-Host

    $packages = @(Get-Item "$($option).*.nupkg")
    if($packages.Length -ne 1) {
        throw
    }

    $packageName = $packages[0].Name
    if($shouldConfirm) {
        $input = SelectAnOption "Are you sure you want to publish the $($packageName) package (yes)? "
        if($input -ine "yes") {
            exit
        }
    }
    
    dotnet nuget push $packageName -k $ApiKey -s $NuGetSource

    Remove-Item $packageName
    Write-Host
    exit
}

function SelectAnOption {
    param(
        [string] $prompt = "Select an option: "
    )

    Write-Host -NoNewline $prompt
    $input = Read-Host
    Write-Host

    return $input
}

function InvalidInput {
    Write-Host "Invalid input."
    Write-Host
}

# Entry point.
Main
