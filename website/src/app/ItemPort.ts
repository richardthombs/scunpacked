import { ItemPortClassification } from './ItemPortClassification';
import { Item } from "./Item";
import { SCItem } from './SCItem';

export interface ItemPort {
  types: {};
  flags: {};
  classification: ItemPortClassification;
  minsize: number;
  maxsize: number;
  item: string;
  port: string;
  loadedItem: SCItem | undefined;
}
