export interface JsonLoadout {
  portName: string | undefined;
  itemName: string | undefined;
  Items: JsonLoadout[] | undefined;
}

export interface SEntityComponentDefaultLoadoutParams {
  loadout: SItemPortLoadout;
}

export interface SItemPortLoadoutManualParams {
  entries: SItemPortLoadoutEntryParams[];
}

export interface SItemPortLoadoutEntryParams {
  itemPortName: string;
  entityClassName: string;
  loadout: SItemPortLoadout;
}

export interface SItemPortLoadout {
  SItemPortLoadoutManualParams: SItemPortLoadoutManualParams;
  SItemPortLoadoutXMLParams: SItemPortLoadoutXMLParams;
}

export interface SItemPortLoadoutXMLParams {
  loadoutPath: string;
}
