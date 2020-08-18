## Contributing
If you'd like to contribute to scunpacked, then first of all, THANK YOU!

Pull requests are always welcome, please make sure that each PR is tightly focussed on making a specific change. This will make it much easier for me to evaluate and merge.

## Tools
I develop on Windows 10 with the following tools:

[Visual Studio Code](https://code.visualstudio.com/Download)  
[NVM for Windows](https://github.com/coreybutler/nvm-windows/releases)  
[.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core)

## Getting started
1. Install the tools listed above.
1. Install `nvm` to install the most recent LTS version of node. Eg:
```
nvm install 12.18.3
```
3. Install the Angular CLI globally:
```
npm install -g @angular/cli
```
4. In the folder where you've cloned `scunpacked` run
```
npm install
```

There is a VSCODE workspace called `scunpacked.code-workspace`, use this to launch VSCODE:
```
vscode scunpacked.code-workspace
```

## Running the website
The website is build using Angular and can be launched from the Windows command line like this:
```
ng serve
```
This will fire up the development server on port 4200, so you can browse the website at http://localhost:4200. By default it connects to the live API at https://scunpacked.com/api.

Find me as @stony on the hardpoint.io Discord.
