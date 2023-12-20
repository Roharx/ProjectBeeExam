export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}

//comments are for future reference to make development easier
export class Account {
  id!: number;
  email!: string;
  name!: string;
  rank!: AccountRank;
}

export class AccountCreateUpdateDto {
  id!: number;
  email!: string;
  name!: string;
  rank!: AccountRank;
  password!: string;
}


export class Ailment {
  id!: number;
  hiveId!: number;
  name!: string;
  severity!: number;
  comment?: string;
  solved!: boolean;
}

export class Bee {
  id!: number;
  name!: string;
  description!: string;
  comment?: string;
}

export class Field {
  id!: number;
  name!: string;
  location!: string;
  hives?: Hive[];
  managers?: Account[];
}

export class Harvest {
  id!: number;
  hiveId!: number;
  time!: string;//format: yyyy-mm-dd hh:mm:ss
  honeyAmount!: number;
  beeswaxAmount!: number;
  comment?: string;
}

export class Hive {
  id?: number;
  field_Id?: number;
  name?: string;
  location?: string;
  placement?: string;//format: yyyy-mm-dd
  last_Check?: string;//format: yyyy-mm-dd hh:mm:ss
  ready?: boolean;
  color?: string;
  bee_Type?: number;
  comment?: string;
}

export class Honey {
  id!: number;
  name!: string;
  liquid!: boolean;
  harvestId!: number;
  moisture!: number;//float
  flowers!: string;
}

export class Inventory {
  id!: number;
  fieldId!: number;
  name!: string;
  description!: string;
  amount!: number;
}

export class Task {
  id!: number;
  hiveId!: number;
  name!: string;
  description!: string;
  done!: boolean;
}

export enum AccountRank {
  Admin,
  FieldManager,
  Keeper,
  Sales,
  Guest
}
