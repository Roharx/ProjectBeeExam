import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';

import {IonicModule, IonicRouteStrategy} from '@ionic/angular';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {LoginComponent} from "./components/login.component";
import {BeesFeedComponent} from "./components/beesFeed.component";
import {NgOptimizedImage} from "@angular/common";
import {UserComponent} from "./components/user.component";
import {FieldComponent} from "./components/field.component";
import {CreateFieldComponent} from "./components/createField.component";
import {CreateHiveComponent} from "./components/createHive.component";
import {EditHiveComponent} from "./components/editHive.component";
import {AccountsComponent} from "./components/accounts.component";
import {CreateAccountComponent} from "./components/createAccount.component";
import {RankModalComponent} from "./components/rankModal.component";

@NgModule({
  declarations: [AppComponent, LoginComponent, BeesFeedComponent, UserComponent, FieldComponent,
    CreateFieldComponent, CreateHiveComponent, EditHiveComponent, AccountsComponent, CreateAccountComponent, RankModalComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, HttpClientModule, ReactiveFormsModule, FormsModule, NgOptimizedImage],
  providers: [{provide: RouteReuseStrategy, useClass: IonicRouteStrategy}],
  bootstrap: [AppComponent],
})
export class AppModule {
}
