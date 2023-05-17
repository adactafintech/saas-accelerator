# https://microsoft.github.io/Mastering-the-Marketplace/saas-accelerator/#setting-up-a-development-environment-for-the-saas-accelerator
# https://go.microsoft.com/fwlink/?linkid=2224222


# dotnet tool install --global dotnet-ef; `
git clone https://github.com/adactafintech/saas-accelerator.git -b 6.1.2 --depth 1; `
cd ./saas-accelerator/deployment; `
.\Deploy.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev" `
 -PublisherAdminUsers "igor.mileusnic@adacta-fintech.com" `
 -Location "West Europe" 


#  ➡️ Landing Page section:       https://adi-dev-sa-portal.azurewebsites.net/
#  ➡️ Connection Webhook section: https://adi-dev-sa-portal.azurewebsites.net/api/AzureWebhook
#  ➡️ Tenant ID:                  cdeb9156-219a-49e3-b414-f63acd298e9c
#  ➡️ AAD Application ID section: 11b97a06-846d-45eb-83d2-121e54027a13



git clone https://github.com/adactafintech/saas-accelerator.git -b adacta-setup-01 --depth 1; `
cd ./saas-accelerator/deployment; `
.\Upgrade.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev"


 # Allowlist IP 89.216.208.132 on server adi-dev-sa-sql



#  {
#     "name": "SaaSApiConfiguration__ProvisionAPIBaseURL",
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
#     "value": "9c25baa49d9884fbe493dfc41b90ee",
#     "slotSetting": false
#   },
#   {
#     "name": "SaaSApiConfiguration__ProvisionWebHookToken",
#     "value": "ac64baa3ad9884ahe493dfc6kb9077",
#     "slotSetting": false
#   },