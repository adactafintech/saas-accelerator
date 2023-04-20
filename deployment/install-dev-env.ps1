# https://microsoft.github.io/Mastering-the-Marketplace/saas-accelerator/#setting-up-a-development-environment-for-the-saas-accelerator
# https://go.microsoft.com/fwlink/?linkid=2224222


# dotnet tool install --global dotnet-ef; `
git clone https://github.com/Azure/Commercial-Marketplace-SaaS-Accelerator.git -b 6.1.2 --depth 1; `
cd ./Commercial-Marketplace-SaaS-Accelerator/deployment; `
.\Deploy.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev" `
 -PublisherAdminUsers "igor.mileusnic@adacta-fintech.com" `
 -Location "West Europe" 


#  ➡️ Landing Page section:       https://adi-dev-sa-portal.azurewebsites.net/
#  ➡️ Connection Webhook section: https://adi-dev-sa-portal.azurewebsites.net/api/AzureWebhook
#  ➡️ Tenant ID:                  cdeb9156-219a-49e3-b414-f63acd298e9c
#  ➡️ AAD Application ID section: 11b97a06-846d-45eb-83d2-121e54027a13



git clone https://github.com/Azure/Commercial-Marketplace-SaaS-Accelerator.git -b 6.1.2 --depth 1; `
cd ./Commercial-Marketplace-SaaS-Accelerator/deployment; `
.\Upgrade.ps1 `
 -WebAppNamePrefix "adi-dev-sa" `
 -ResourceGroupForDeployment "rg-saas-accelerator-dev"


 # Allowlist IP 89.216.208.132 on server adi-dev-sa-sql