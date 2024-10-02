# service bus

finally we can enterprise inside of the cloud

```bash
GRP=grp-svcbus-learn
NAMESPACE=az204busvroom
REG=az204vroomreg
IMG=playground/servicebus:v1
az group create -n $GRP -l westeurope

az servicebus namespace create -g $GRP -n $NAMESPACE -l westeurope
az servicebus queue create -g $GRP --namespace-name $NAMESPACE -n az204q

az acr create -g $GRP -l westeurope -n $REG --sku Basic
az acr build -r $REG -g $GRP -t $IMG .

IDID=$(az identity create -n imgpull -g $GRP -l westeurope -o tsv --query id)
IDPID=$(az identity show -n imgpull -g $GRP -o tsv --query principalId)
REGSCOPE=$(az acr show -g $GRP -n $REG -o tsv --query id)
az role assignment create --scope $REGSCOPE --role acrpull --assignee $IDPID

EBID=$(az servicebus namespace show -g $GRP -n $NAMESPACE -o tsv --query id)

#az role assignment create --scope $EBID --assignee $IDPID --role Contributor

az container create -g $GRP -n servicebus \
    --image $REG.azurecr.io/$IMG \
    --scope $EBID \
    --role "Azure Service Bus Data Owner" \
    --assign-identity "[system]" $IDID \
    --acr-identity $IDID \
    --restart-policy never

az group delete -n $GRP --no-wait
```
