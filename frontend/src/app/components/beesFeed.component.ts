import {Component, OnInit } from "@angular/core";
import {Router} from "@angular/router";
import {State} from "../../state";
import {TokenService} from "../services/token.service";
import {JwtService} from "../services/jwt.service";
import {ModalController} from "@ionic/angular";
import {CreateFieldComponent} from "./createField.component";
import {firstValueFrom} from "rxjs";
import {Account, Bee, Field, Hive, ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {EditHiveComponent} from "./editHive.component";

@Component({
  template: `
      <div id="page-container">
          <div id="top-bar">

              <div id="user-container">
                  <div id="user-background" (click)="editUser()">
                      <img src="../assets/images/hive.png" height="50" width="50">
                  </div>
                  <div id="user-controls">
                      <p id="username" (click)="editUser()">{{username}}</p>
                      <p id="log-out" (click)="logout()">Log <span id="out">out</span></p>
                  </div>

              </div>
              <div id="warning-triangle" *ngIf="showWarning">
                  !
              </div>
          </div>

          <div id="field-container">

              <div class="field" *ngFor="let field of this.state.fields">

                  <div class="field-top">
                      {{field.name}}
                      <button class="field-edit-button" (click)="editField(field)">...</button>
                  </div>
                  <div class="field-content">
                      <div class="hive" *ngFor="let hive of field.hives">
                          <div class="hive-top">
                              <p class="hive-name">{{ hive.name }}</p>
                          </div>
                          <div class="hive-middle">
                              <p>ID: {{ hive.id }}</p>
                          </div>
                          <div class="hive-bottom">
                              <button class="manage-hive-button" (click)="editHive(hive)">Edit</button>
                          </div>
                      </div>

                  </div>

              </div>

          </div>
          <button id="add-button" (click)="createField()">+</button>
      </div>
  `,
  styleUrls: ['../css/beesFeed.component.scss'],
  selector: 'bees-feed'
})

export class BeesFeedComponent implements OnInit {
  username: any;
  showWarning: boolean = false;//change the value if there are global warnings
  constructor(private router: Router, private jwtService: JwtService, public modalController: ModalController,
              public state: State, public tokenService: TokenService, public http: HttpClient) {
  }
  ngOnInit(): void {

    const token = this.tokenService.getToken();

    if (token) {
      const decodedToken = this.jwtService.decodeToken(token);
      this.username = decodedToken ? decodedToken['sub'] : null;
      this.getFieldsFOrAccount(decodedToken);
    } else {
      this.router.navigate(['login-component']);
    }
  }

  private async getFieldsFOrAccount(decodedToken: any) {
    var response = await firstValueFrom(
      this.http.get<ResponseDto<Field[]>>(environment.baseURL + '/api/getFieldsForAccount/' + decodedToken.id));
    this.state.fields = response.responseData!;
    await this.getHivesForFields();
  }

  private async getHivesForFields() {
      for (const field of this.state.fields) {
          const response = await firstValueFrom(
              this.http.get<ResponseDto<Hive[]>>(environment.baseURL + `/api/getHivesForField/${field.id}`)
          );
          field.hives = response.responseData!;
      }
  }
  private async getManagersForFields() {
    if(this.state.selectedField !== null){
      const response = await firstValueFrom(
          this.http.get<ResponseDto<Account[]>>(environment.baseURL + `/api/getAccountsForField/${this.state.selectedField.id}`)
      );
      this.state.selectedField.managers = response.responseData!;
    }
  }

  logout() {
    this.tokenService.removeToken();
    this.router.navigate(['login-component']);
  }

  editUser() {
    this.router.navigate(['user-component']);
  }

  editField(selectedField: Field) {
    this.state.selectedField = selectedField;
    this.getManagersForFields();
    this.fillManagers();
    this.router.navigate(['field-component']);
  }

  async createField() {
    const modal = await this.modalController.create({
      component: CreateFieldComponent,
      cssClass: 'custom-field-modal'
    });
    this.fillManagers();
    modal.present();
  }

  private async fillManagers() {
    var response = await firstValueFrom(
      this.http.get<ResponseDto<Account[]>>(environment.baseURL + '/api/getManagers'));
    this.state.managers = response.responseData!;
  }

  async editHive(hive: Hive) {
    this.state.selectedHive = hive;
    const modal = await this.modalController.create({
      component: EditHiveComponent,
      cssClass: 'custom-edit-hive-modal'
    });
    var response = await firstValueFrom(
        this.http.get<ResponseDto<Bee[]>>(environment.baseURL + '/api/getBees'));
    this.state.bees = response.responseData!;
    modal.present();
  }
}
