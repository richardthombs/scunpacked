## Usage

Create a folder to store the extracted Star Citizen data

```
mkdir c:\scdata\3.7.2
```

Run the extraction tools

```
cd c:\scdata\3.7.2
unp4k.exe 'C:\Program Files\Roberts Space Industries\StarCitizen\LIVE\Data.p4k' *.xml
unforge.exe .
```

Run the parser

```
shipparser.exe
```

SItemPortLoadoutEntryParams.itemPortName == Part.name == Item.portName
