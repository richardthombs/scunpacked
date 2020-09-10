![Build](https://github.com/richardthombs/scunpacked/workflows/Build/badge.svg)

## Welcome to Star Citizen Unpacked!

_This is an unofficial Star Citizen fansite, not affiliated with the Cloud Imperium group of companies. All content on this site not authored by its host or users are property of their respective owners._

## Introduction

This projects is made up of three parts:

1. `loader` - this is a .NET Core application which parses XML data extracted from the Star Citizen game files and produces a set of JSON files.

1. `api` - this is (currently) a static website which serves up the JSON files to be consumed by the website. In a later release this will be upgraded to allow server-side querying of the data.

1. `website` - this is an Angular application which provides a front-end to the data available through the API.

## Extracting data from Star Citizen

Create a folder to store the extracted Star Citizen data:

```
mkdir c:\scdata\3.7.2
```

Download and run the [extraction tools](https://github.com/dolkensp/unp4k):

```
cd c:\scdata\3.7.2
unp4k.exe 'C:\Program Files\Roberts Space Industries\StarCitizen\LIVE\Data.p4k' *.xml
unp4k.exe 'C:\Program Files\Roberts Space Industries\StarCitizen\LIVE\Data.p4k' *.ini
unforge.exe .
```

Run the loader:

```
loader.exe -scdata=c:\scdata\3.7.2 -output=c:\scdata\3.7.2-json
```

Now you will have a folder `c:\scdata\3.7.2-json` which contains:

```
ammo/              - Folder containing the json for each type of ammunition
items/             - Folder containing the json for each item in Star Citizen, named after the class name (there are a lot)
loadouts/          - Folder containing loadouts for ships and items named after the loadout filename (CIG seem to be moving away from loadout files)
ships/             - Folder containing the json for each ships, named after the class name
ammo.json          - Index of all the ammunition fired by weapons and countermeasures
fps-items.json     - Index of player equipment
items.json         - Index of pretty much all the items in Star Citizen (this is quite large)
labels.json        - English translations of all labels
manufacturers.json - Index of all the manufacturers in the game
ship-items.json    - Index of ship equipment
ships.json         - Index of all the ships
shops.json         - Index of all shops and everything that they sell or buy
starmap.json       - Index of the locations in the star map
```
