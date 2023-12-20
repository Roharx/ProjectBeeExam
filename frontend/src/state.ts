import {Injectable} from "@angular/core";
import {Account, Bee, Field, Hive} from "./models";

@Injectable({
  providedIn: 'root'
})
export class State {
  accounts: Account[] = [];
  managers: Account[] = [];
  fields: Field[] = [];
  bees: Bee[] = [];
  selectedField: Field = {id: 0, location: "", name: ""};
  selectedHive: Hive = {};
  selectedAccount: Account = {email: "", id: 0, name: "", rank: undefined!};
}
