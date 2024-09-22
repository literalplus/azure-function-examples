```bash
export GRP=grp-az204-blob
export NAME=learn-cosmos-RAND
az group create --name $GRP --region europewest
az cosmosdb create --name $NAME --resource-group $GRP \
    --locations germanywestcentral \
    --backup-policy-type Continuous \
    --continuous-tier Continuous7Days

az cosmosdb keys list --name $NAME --resource-group $GRP --type connection-strings

az group delete --name $GRP --no-wait
```
