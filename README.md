# az-playground

funny things to do with blue cloud

## fix az interactive error 'ConfigParser' object has no attribute 'readfp'. Did you mean: 'read'

probably caused by python 3.12 which dropped `readfp`. internet says `read_file` is cool now.

clearly a duct tape temp fix, don't use this in any meaningful environement etc.

```bash
cd ~/home/lit~/.azure/cliextensions/interactive/azext_interactive/azclishell
vim configuration.py # replace the problematic `self.config.readfp` with `self.config.read_file`
```
