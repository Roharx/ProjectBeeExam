import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BeesFeedComponent} from "./components/bees-feed.component";
import {LoginComponent} from "./components/login.component";
import {UserComponent} from "./components/user.component";
import {EditFieldComponent} from "./components/edit-field.component";
import {CreateFieldModal} from "./modals/create-field.modal";
import {CreateHiveModal} from "./modals/create-hive.modal";
import {EditHiveModal} from "./modals/edit-hive.modal";
import {AccountsComponent} from "./components/accounts.component";
import {CreateAccountModal} from "./modals/create-account.modal";
import {ChangeRankModal} from "./modals/change-rank.modal";
import {ConfirmModal} from "./modals/confirm.modal";


let routes: Routes;
routes = [
  {path: '', redirectTo: '/login-component', pathMatch: 'full'},
  {path: 'login-component', component: LoginComponent},
  {path: 'bees-feed', component: BeesFeedComponent},
  {path: 'user-component', component: UserComponent},
  {path: 'field-component', component: EditFieldComponent},
  {path: 'create-field-component', component: CreateFieldModal},
  {path: 'create-hive-component', component: CreateHiveModal},
  {path: 'edit-hive-component', component: EditHiveModal},
  {path: 'accounts-component', component: AccountsComponent},
  {path: 'create-account-component', component: CreateAccountModal},
  {path: 'rank-modal-component', component: ChangeRankModal},
  {path: 'confirm-modal', component: ConfirmModal}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
