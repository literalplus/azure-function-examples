```bash
export GRP=grp-az204-blob
az group create --location westeurope --name $GRP
az storage account create --resource-group $GRP --name learnaz204blobXXXRANDXXX --sku Standard_LRS

az group delete --name $GRP --no-wait
```
