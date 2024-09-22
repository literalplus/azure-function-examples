```bash
export GRP=grp-az204-blob
az group create --name $GRP --region europewest
az cosmosdb create --name learn-cosmos-RAND --resource-group $GRP \
    --locations germanywestcentral \
    --backup-policy-type Continuous \
    --continuous-tier Continuous7Days \
    --lomd GöpbaöDocumentDB

az group delete --name $GRP --no-wait
```