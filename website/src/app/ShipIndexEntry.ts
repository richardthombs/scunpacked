export interface ShipIndexEntry {
	jsonFilename: string;
	name: string;
	career: string;
	role: string;
	className: string;
	type: string;
	subType: string;
	dogFightEnabled: boolean;
	size?: number;
	isGroundVehicle: boolean;
	isGravlevVehicle: boolean;
	isSpaceship: boolean;
	noParts: boolean;
	// We add these fields as we parse what we download from the API
	roles: {
		role: string;
		subRole: string;
	}[];
}
