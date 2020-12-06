export interface ShipIndexEntry {
  ClassName: string;
  Name: string;
  Description: string;
  Career: string;
  Role: string;
  Size?: number;
  IsVehicle: boolean;
  IsGravlev: boolean;
  IsSpaceship: boolean;
  Manufacturer: {
    Code: string;
    Name: string;
  },

  // We add these fields as we parse what we download from the API
  roles: {
    role: string;
    subRole: string;
  }[];
}
