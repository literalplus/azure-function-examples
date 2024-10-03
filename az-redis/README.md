# az-redis

```bash
GRP=az204-redis-horse
REDIS=az204cachehorse

az group create -n $GRP -l westeurope

az redis create -g $GRP -l westeurope -n $REDIS --sku Basic --vm-size c0

az redis list-keys -g $GRP -n $REDIS -o tsv --query primaryKey

az group delete -n $GRP --no-wait
```
