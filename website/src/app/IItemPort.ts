import { SCItem } from './SCItem';

export interface IItemPort {
  name: string;
  displayName: string;
  minSize: number;
  maxSize: number;
  flags: string[];
  types: string[];
  item: SCItem | undefined;

  findItemPorts(predicate?: (itemPort: IItemPort) => boolean): IItemPort[];
}
