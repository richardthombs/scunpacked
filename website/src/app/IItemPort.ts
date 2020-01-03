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

  /** Returns TRUE if the user can fit an item to this port */
  isEditable(): boolean;

  /** Returns TRUE if the port can be used to mount a gun (ballistic, laser, mining laser, tractor beam) */
  isGunHardpoint(): boolean;

  /** Returns TRUE if the port can be used to mount a missile rack */
  isMissileHardpoint(): boolean;

  /** Returns TRUE if the port can be used to mount a weapon */
  isWeaponHardpoint(): boolean;

  /** Returns TRUE if the port can be controlled remotely */
  isRemoteControlled(): boolean;

  /** Returns TRUE if the port requires a crew member to operate */
  isManned(): boolean;

  /** Returns TRUE if this port is under the pilot's control */
  isPilotControlled(): boolean;

  /** Returns TRUE if the port is a weapon attachment, IE something that augments a weapon */
  isWeaponAttachment(): boolean;

  /** Returns TRUE if the port is gimballed */
  isGimballed(): boolean;
}
