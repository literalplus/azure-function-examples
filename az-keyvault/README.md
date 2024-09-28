# az-keyvault

## local poc

```bash
export GRP=grp-akv
export VAULT=learnvault3245634526
az group create -n $GRP -l westeurope
az keyvault create -n $VAULT -g $GRP -l westeurope

az keyvault secret set -v $VAULT -n examplepw -v oop
az keyvault secret show -v $VAULT -n examplepw

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
az role assignment create --assignee $SPID --scope $REGSCOPE --role acrpull

# Pull service principal in Entra ID. Managed Identity also creates a SP in the background, this is
# just the ugly manual way to do it. Password can only be see during creation, which is a bit
# unergonomic here. Alternatively we could just create a Token in ACR, which we should delete
# afterwards. Both approaches seem to suck about equally. The token approach has the benefit
# that at least the tokens are scope to the ACR lifetime and don't permanently pollute our
# Entra ID Directory. Also, we can easily scope it to a single repository, which is cool.
## Service Principal (this is equivalent to an OIDC client it seems)
#export REGSCOPE=$(az acr show -n $REG --query id -o tsv)
#export REGPW=$(az ad sp create-for-rbac -n az-keyvault-pull --scopes $REGSCOPE --role acrpull --query password -o tsv)
#export REGUSER=$(az ad sp list --display-name az-keyvault-pull --query "[].appId" -o tsv)

## Token
export REGUSER=az-keyvault-pull
az acr token create -n $REGUSER -r $REG --repository learn/az-keyvault content/read metadata/read >/dev/null
export REGPW=$(az acr token credential generate -n $REGUSER -r $REG --password1 --query "passwords[0].value" -o tsv)

# In another console run: 
# az container attach -g $GRP -n az-keyvault

az container create -g $GRP -n az-keyvault \
    --image $REG.azurecr.io/learn/az-keyvault:v1 \
    --restart-policy Never \
    --assign-identity $SPPATH \
    --registry-username $REGUSER \
    --registry-password $REGPW

az group delete -n $GRP --no-wait
```
