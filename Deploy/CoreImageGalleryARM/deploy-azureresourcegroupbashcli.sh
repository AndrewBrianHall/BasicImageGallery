rgname="coreimagegallery-" + $(date +"%m_%d_%Y")
az group create --name $(rgname) --location "East US"
az group deployment create \
  --name coreimagegallerydeployment \
  --resource-group $(rgname) \
  --template-file azuredeploy.json \
  --parameters @azuredeploy-parameters.json