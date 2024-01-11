import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';

import {IonicModule, IonicRouteStrategy} from '@ionic/angular';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {LoginComponent} from "./components/login.component";
import {BeesFeedComponent} from "./components/bees-feed.component";
import {NgOptimizedImage} from "@angular/common";
import {UserComponent} from "./components/user.component";
import {EditFieldComponent} from "./components/edit-field.component";
import {CreateFieldModal} from "./modals/create-field.modal";
import {CreateHiveModal} from "./modals/create-hive.modal";
import {EditHiveModal} from "./modals/edit-hive.modal";
import {AccountsComponent} from "./components/accounts.component";
import {CreateAccountModal} from "./modals/create-account.modal";
import {ChangeRankModal} from "./modals/change-rank.modal";
import {ConfirmModal} from "./modals/confirm.modal";
import {AuthInterceptor} from "./interceptors/auth.interceptor";

@NgModule({
  declarations: [AppComponent, LoginComponent, BeesFeedComponent, UserComponent, EditFieldComponent,
    CreateFieldModal, CreateHiveModal, EditHiveModal, AccountsComponent, CreateAccountModal, ChangeRankModal, ConfirmModal],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, HttpClientModule, ReactiveFormsModule, FormsModule, NgOptimizedImage],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  },{provide: RouteReuseStrategy, useClass: IonicRouteStrategy}],
  bootstrap: [AppComponent],
})
export class AppModule {
}
