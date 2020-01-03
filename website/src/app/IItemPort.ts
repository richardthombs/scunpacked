import { SCItem } from './SCItem';

export interface IItemPort {
  name: string;
  displayName: string;
  minSize: number;
  maxSize: number;
  flags: string[];
  types: string[];
  item: SCItem | undefined;
  itemClass: string | undefined;

  findItemPorts(predicate?: (itemPort: IItemPort) => boolean): IItemPort[];

  isEditable(): boolean;

  isGunHardpoint(): boolean;

  isMissileHardpoint(): boolean;

  isWeaponHardpoint(): boolean;

  isRemoteControlled(): boolean;

  isManned(): boolean;

  isPilotControlled(): boolean;

  isWeaponAttachment(): boolean;

  isGimballed(): boolean;
}
