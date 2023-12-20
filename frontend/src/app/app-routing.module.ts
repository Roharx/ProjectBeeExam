import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BeesFeedComponent} from "./components/beesFeed.component";
import {LoginComponent} from "./components/login.component";
import {UserComponent} from "./components/user.component";
import {FieldComponent} from "./components/field.component";
import {CreateFieldComponent} from "./components/createField.component";
import {CreateHiveComponent} from "./components/createHive.component";
import {EditHiveComponent} from "./components/editHive.component";
import {AccountsComponent} from "./components/accounts.component";
import {CreateAccountComponent} from "./components/createAccount.component";
import {RankModalComponent} from "./components/rankModal.component";


let routes: Routes;
routes = [
  {path: '', redirectTo: '/login-component', pathMatch: 'full'},
  {path: 'login-component', component: LoginComponent},
  {path: 'bees-feed', component: BeesFeedComponent},
  {path: 'user-component', component: UserComponent},
  {path: 'field-component', component: FieldComponent},
  {path: 'create-field-component', component: CreateFieldComponent},
  {path: 'create-hive-component', component: CreateHiveComponent},
  {path: 'edit-hive-component', component: EditHiveComponent},
  {path: 'accounts-component', component: AccountsComponent},
  {path: 'create-account-component', component: CreateAccountComponent},
  {path: 'rank-modal-component', component: RankModalComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
