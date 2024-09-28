# az-keyvault

## local poc

```bash
export GRP=grp-akv
export VAULT=learnvault3245634526
az group create -n $GRP -l westeurope
az keyvault create -n $VAULT -g $GRP -l westeurope

az keyvault secret set --vault-name $VAULT -n examplepw --value oop
az keyvault secret show -vault-name $VAULT -n examplepw

az group delete -n $GRP --no-wait
```

## application with system-managed identity

```bash
export GRP=grp-akv
export VAULT=learnvault3245634526
export REG=learnakv15346
az acr create -g $GRP -n $REG --sku Basic
az acr build --image learn/az-keyvault:v1 --registry $REG .
az acr repository list -n $REG -o table

# Managed identity for the RUNNING container. It doesn't seem possible to use the same principal
# to pull from ACR, since Container Instances needs a username/pw combo to pull from the registry
# and doesn't seem to support pulling with a managed identity........................
az identity create -g $GRP -n az-keyvault
export SPID=$(az identity show -g $GRP -n az-keyvault --query principalId -o tsv)
export SPPATH=$(az identity show -g $GRP -n az-keyvault --query id -o tsv)

# Pull images from ACR
export REGSCOPE=$(az acr show -n $REG --query id -o tsv)
az role assignment create --assignee $SPID --scope $REGSCOPE --role acrpull

# Read secrets in AKV
# MS recommendation seems to be to use one AKV per (application, stage) without more granular access control on the key level
export AKVSCOPE=$(az keyvault show -n $VAULT --query id -o tsv)
az role assignment create --assignee $SPID --scope $AKVSCOPE --role 'Key Vault Secrets User'

# In another console run: 
# az container attach -g $GRP -n az-keyvault

# Note that we could use different identities to separate ACR pulling and actual runtime, this is omitted for simplicity
# Note that atm we can only control access at registry scope for Entra ID principals. ACR Tokens allow per-repository control.
## Token example
# export REGUSER=az-keyvault-pull
# az acr token create -n $REGUSER -r $REG --repository learn/az-keyvault content/read metadata/read >/dev/null
# export REGPW=$(az acr token credential generate -n $REGUSER -r $REG --password1 --query "passwords[0].value" -o tsv)
# and --registry-username and --registry-password on the CLI. Password can only be accessed at reset time.


az container create -g $GRP -n az-keyvault \
    --image $REG.azurecr.io/learn/az-keyvault:v1 \
    --restart-policy Never \
    --assign-identity $SPPATH \
    --acr-identity $SPPATH

az group delete -n $GRP --no-wait
```
