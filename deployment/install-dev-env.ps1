# https://microsoft.github.io/Mastering-the-Marketplace/saas-accelerator/#setting-up-a-development-environment-for-the-saas-accelerator
# https://go.microsoft.com/fwlink/?linkid=2224222

###### DEV FIRST DEPLOY COMMAND

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh; `
chmod +x dotnet-install.sh; `
./dotnet-install.sh; `
$ENV:PATH="$HOME/.dotnet:$ENV:PATH"; `
dotnet tool install --global dotnet-ef; `
git clone https://github.com/adactafintech/saas-accelerator.git -b adacta-setup-01 --depth 1; `
cd ./saas-accelerator/deployment; `
.\Deploy.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev" `
 -PublisherAdminUsers "igor.mileusnic@adacta-fintech.com,jernej.kladnik@adacta-fintech.com" `
 -ProvisionAPIBaseURL "https://git.adacta-fintech.com/api/v4/projects/668/trigger/pipeline" `
 -ProvisionBranch "saas-env/prerelease" `
 -ProvisionToken "glptt-0f7d1aad5bd44c804701c630cd02917d4659892e" `
 -ProvisionWebHookToken "ac64baa3ad9884ahe493dfc6kb9077" `
 -Location "West Europe" 

 # AdiCloudKeyVaultName = "adisaasprovprerelease"
 # ProvisionTokenSecretName = "ProvisionToken" -> cloud-ci-trigger-token
 # ProvisionWebHookTokenSecretName = "ProvisionWebHookToken" -> cloud-sa-webhook-token
    # dev = ac64baa3ad9884ahe493dfc6kb9077
    # preprod = ac64baa3ad9884ahe493dfc6kb9077
    # prod = bc63b143ad9ae4ahe793dfa4kb9d37

###### PROD FIRST DEPLOY COMMAND

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh; `
chmod +x dotnet-install.sh; `
./dotnet-install.sh; `
$ENV:PATH="$HOME/.dotnet:$ENV:PATH"; `
dotnet tool install --global dotnet-ef; `
git clone https://github.com/adactafintech/saas-accelerator.git -b adacta-setup-01 --depth 1; `
cd ./saas-accelerator/deployment; `
.\Deploy.ps1 `
 -WebAppNamePrefix "adi-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator" `
 -PublisherAdminUsers "igor.mileusnic@adacta-fintech.com,jernej.kladnik@adacta-fintech.com" `
 -ProvisionAPIBaseURL "https://git.adacta-fintech.com/api/v4/projects/668/trigger/pipeline" `
 -ProvisionBranch "saas-env/release" `
 -ProvisionToken "glptt-85f9d6c763b4f7075085c9500cbd7de126518661" `
 -ProvisionWebHookToken "bc63b143ad9ae4ahe793dfa4kb9d37" `
 -Location "West Europe" 

###### UPGRADE COMMAND

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh; `
chmod +x dotnet-install.sh; `
./dotnet-install.sh; `
$ENV:PATH="$HOME/.dotnet:$ENV:PATH"; `
dotnet tool install --global dotnet-ef; `
git clone https://github.com/adactafintech/saas-accelerator.git -b adacta-setup-01 --depth 1; `
cd ./saas-accelerator/deployment; `
.\Upgrade.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev"

###### DEV PARAMETERS

#  ➡️ Landing Page section:       https://adi-dev-sa-portal.azurewebsites.net/
#  ➡️ Connection Webhook section: https://adi-dev-sa-portal.azurewebsites.net/api/AzureWebhook
#  ➡️ Tenant ID:                  cdeb9156-219a-49e3-b414-f63acd298e9c
#  ➡️ AAD Application ID section: 11b97a06-846d-45eb-83d2-121e54027a13

###### PROD PARAMETERS

# ➡️ Landing Page section:       https://adi-sa-portal.azurewebsites.net/
# ➡️ Connection Webhook section: https://adi-sa-portal.azurewebsites.net/api/AzureWebhook
# ➡️ Tenant ID:                  cdeb9156-219a-49e3-b414-f63acd298e9c
# ➡️ AAD Application ID section: 254acb11-7727-4db4-a58c-1a4fee74928d

 # Allowlist IP 89.216.208.132 on server adi-dev-sa-sql

###### PREPROD CONFIGURATION

#  {
#     "name": "SaaSApiConfiguration__ProvisionAPIBaseURL",
#     "value": "https://git.adacta-fintech.com/api/v4/projects/668/trigger/pipeline",
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionBranch",
#     "value": "saas-env/prerelease",
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionToken",
#     "value": "glptt-0f7d1aad5bd44c804701c630cd02917d4659892e",
#     "value": "@Microsoft.KeyVault(VaultName=adi-dev-sa-kv;SecretName=ADApplicationSecret)"
adisaasprov    cloud-ci-trigger-token-dev
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionWebHookToken",
#     "value": "ac64baa3ad9884ahe493dfc6kb9077",
#     "slotSetting": false
#   },

###### PROD CONFIGURATION

# {
#     "name": "SaaSApiConfiguration__ProvisionAPIBaseUrl",
#     "value": "https://git.adacta-fintech.com/api/v4/projects/668/trigger/pipeline",
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionBranch",
#     "value": "saas-env/release",
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionToken",
#     "value": "glptt-85f9d6c763b4f7075085c9500cbd7de126518661",
#     "value": "@Microsoft.KeyVault(VaultName=adi-dev-sa-kv;SecretName=ADApplicationSecret)"
adisaasprov     cloud-ci-trigger-token
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionWebHookToken",
#     "value": "bc63b143ad9ae4ahe793dfa4kb9d37",
#     "slotSetting": false
#   },


###### PROD CONFIGURATION