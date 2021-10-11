# DGC-testdata-trustlist 
## A GreenPassValidator trustlist formatter
This is a C# Console App which converts files from the [dgc-testdata official repository](https://github.com/eu-digital-green-certificates/dgc-testdata) to the trustlist format used in [lpisano's Green Pass Validator](https://github.com/lucapisano/GreenPassValidator). 

## Summary
- [How to use](#how-to-use)
- [Important info for validation](#important-info-for-validation)
- [Who can use this project](#who-can-use-this-project)
## How to use

First, clone this repository 
```
git clone https://github.com/Mrizzi-96/dgc-testdata-trustlist.git
```
Then, initialize submodules in the assets folder in order to use the official testdata repository
```
git submodule init https://github.com/eu-digital-green-certificates/dgc-testdata.git 
```
Lastly, Start the console app. You will find your formatted trustlist in a file called ```cached_trust_list.json```.
## Important info for validation
**Remember that validation using dgc-testdata's certificates needs to be done at the moment which is written in the ```TESTCTX.VALIDATIONCLOCK```  JSON property. See their [README file](https://github.com/eu-digital-green-certificates/dgc-testdata#readme) for clarification.**
## Who can use this project
See [LICENSE.txt](/LICENSE.txt).
