#
# Powershell script to cleanup the resources - Customer portal, Publisher portal and the Azure SQL Database
#

#.\Cleanup.ps1 `
# -WebAppNamePrefix "amp_saas_accelerator_<unique>" `
# -ResourceGroupForDeployment "<your-resource-group-name-if-used"

Param(  
   [string][Parameter(Mandatory)]$WebAppNamePrefix, # Prefix used for creating web applications
   [string][Parameter(Mandatory)]$ResourceGroupForDeployment, # Name of the resource group to deploy the resources
   [string][Parameter(Mandatory)]$ADApplicationID, # The value should match the value provided for Active Directory Application ID in the Technical Configuration of the Transactable Offer in Partner Center
   [string][Parameter()]$KeyVault, # Name of KeyVault
   [switch][Parameter()]$Quiet #if set, only show error / warning output from script commands
)

# Make sure to install Az Module before running this script
# Install-Module Az
# Install-Module -Name AzureAD

$ErrorActionPreference = "Stop"
$startTime = Get-Date
#region Set up Variables and Default Parameters

if ($ResourceGroupForDeployment -eq "") {
    $ResourceGroupForDeployment = $WebAppNamePrefix 
}

if($KeyVault -eq "")
{
   $KeyVault=$WebAppNamePrefix+"-kv"
}

$SaaSApiConfiguration_CodeHash= git log --format='%H' -1
$azCliOutput = if($Quiet){'none'} else {'json'}

#endregion

#region Validate Parameters

if($WebAppNamePrefix.Length -gt 21) {
    Throw "🛑 Web name prefix must be less than 21 characters."
    Exit
}


if(!($KeyVault -match "^[a-z0-9-]+$")) {
    Throw "🛑 KeyVault name only allows alphanumeric and hyphens."
    Exit
}
#endregion 

Write-Host "Starting SaaS Accelerator Cleanup..."

#region Resource cleanup

$currentContext = az account show | ConvertFrom-Json
$currentTenant = $currentContext.tenantId
$currentSubscription = $currentContext.id

Write-host "   🔵 Resource Group"
Write-host "      ➡️ Cleanup Resource Group"
az group delete --name $ResourceGroupForDeployment --yes

Write-host "      ➡️ Purge KeyVault"
az keyvault purge --name $WebAppNamePrefixadi-kv

Write-host "🔑 Creating Fulfilment API App Registration"
Write-host "      ➡️ Cleanup FulfillmentAppReg App Registration"
#az ad app delete --id ecbc24c7-826d-4358-a902-2ac5cdf6754b

Write-host "      ➡️ Cleanup LandingpageAppReg App Registration"
#az ad app delete --id a00b9cb5-0d20-462c-9433-c88a024501c2


$duration = (Get-Date) - $startTime
Write-Host "Deployment Complete in $($duration.Minutes)m:$($duration.Seconds)s"
Write-Host "DO NOT CLOSE THIS SCREEN.  Please make sure you copy or perform the actions above before closing."
#endregion
