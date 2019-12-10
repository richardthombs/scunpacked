import { ItemPortClassification } from './ItemPortClassification';
import { SCItem } from './SCItem';

export interface ItemPortLoadout {
  types: {};
  flags: {};
  classification: ItemPortClassification;
  minsize: number;
  maxsize: number;
  item: string;
  port: string;
  loadedItem: SCItem | undefined;
}
