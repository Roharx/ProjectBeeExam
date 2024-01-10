import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import {ModalController, ToastController} from "@ionic/angular";
import { firstValueFrom } from "rxjs";
import { Account, Bee, Field, Hive, ResponseDto } from "../../models";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { JwtService } from "../services/jwt.service";
import { TokenService } from "../services/token.service";
import { State } from "../../state";
import { CreateFieldModal } from "../modals/create-field.modal";
import { EditHiveModal } from "../modals/edit-hive.modal";
import { CreateHiveModal } from "../modals/create-hive.modal";

@Component({
  template: `
    <div id="page-container">
      <div id="top-bar">
        <div id="user-container">
          <div id="user-background" (click)="editUser()">
            <img src="../assets/images/hive.png" height="50" width="50">
          </div>
          <div id="user-controls">
            <p id="username" (click)="editUser()">{{ username }}</p>
            <p id="log-out" (click)="logout()">Log <span id="out">out</span></p>
          </div>
        </div>
        <button
          id="manage-accounts-button"
          *ngIf="rank < 1"
          (click)="manageAccounts()"
        >
          Manage Accounts
        </button>
        <div id="warning-triangle" *ngIf="showWarning">!</div>
      </div>

      <div id="field-container">
        <div class="field" *ngFor="let field of state.fields">
          <div class="field-top">
            {{ field.name }}
            <button
              class="field-edit-button"
              (click)="editField(field)"
            >
              ...
            </button>
          </div>
          <div class="field-content">
            <div class="hive" *ngFor="let hive of field.hives">
              <div class="hive-top">
                <p class="hive-name">{{ hive.name }}</p>
              </div>
              <div class="hive-middle">
                <p>ID: {{ hive.id }}</p>
                <p>
                  {{
                  hive.ready ? "Ready To Harvest" : "Not Ready"
                  }}
                </p>
              </div>
              <div class="hive-bottom">
                <button
                  class="manage-hive-button"
                  (click)="editHive(hive)"
                >
                  Edit
                </button>
              </div>
            </div>
          </div>
          <div class="field-bottom">
            <button
              class="create-hive-button"
              (click)="createHive(field)"
            >
              Add hive
            </button>
          </div>
        </div>
      </div>
      <button id="add-button" (click)="createField()">+</button>
    </div>
  `,
  styleUrls: ["../scss/bees-feed.component.scss"],
  selector: "bees-feed",
})
export class BeesFeedComponent implements OnInit {
  username: any;
  showWarning: boolean = false;
  rank: any;

  constructor(
    private router: Router,
    private jwtService: JwtService,
    public modalController: ModalController,
    public state: State,
    public toastController: ToastController,
    public tokenService: TokenService,
    public http: HttpClient
  ) {}

  async ngOnInit(): Promise<void> {
    const token = this.tokenService.getToken();

    if (token) {
      const decodedToken = this.jwtService.decodeToken(token);
      this.username = decodedToken ? decodedToken['sub'] : null;
      this.rank = decodedToken ? decodedToken['rank'] : null;
      await this.getFieldsForAccount(decodedToken);
    } else {
      this.router.navigate(["login-component"]);
    }
  }

  private async getFieldsForAccount(decodedToken: any): Promise<void> {
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Field[]>>(
        `${
          environment.baseURL
        }/api/getFields${this.rank > 0 ? `ForAccount/${decodedToken.id}` : ""}`
      )
    );
    this.state.fields = response.responseData || [];

    await this.getHivesForFields();
  }

  private async getHivesForFields(): Promise<void> {
    for (const field of this.state.fields) {
      const response = await firstValueFrom(
        this.http.get<ResponseDto<Hive[]>>(
          `${environment.baseURL}/api/getHivesForField/${field.id}`
        )
      );
      field.hives = response.responseData || [];
    }
  }

  private async getManagersForFields(): Promise<void> {
    if (this.state.selectedField) {
      const response = await firstValueFrom(
        this.http.get<ResponseDto<Account[]>>(
          `${environment.baseURL}/api/getAccountsForField/${this.state.selectedField.id}`
        )
      );
      this.state.selectedField.accounts = response.responseData || [];
    }
  }

  logout(): void {
    this.tokenService.removeToken();
    this.router.navigate(["login-component"]);
  }

  editUser(): void {
    this.router.navigate(["user-component"]);
  }

  editField(selectedField: Field): void {
    if(this.rank < 2){
      this.state.selectedField = selectedField;
      this.getManagersForFields();
      this.fillManagers();
      this.fillKeepers();
      this.router.navigate(["field-component"]);
    }
    else{
      this.showRankError();
    }
  }
  private async showRankError() {
    const toast = await this.toastController.create({
      message: "Only field managers can modify fields.",
      duration: 5000,
      color: "danger"
    });
    await toast.present();
  }
  async createField(): Promise<void> {
    const modal = await this.modalController.create({
      component: CreateFieldModal,
      cssClass: "custom-field-modal",
    });
    this.fillManagers();
    modal.present();
  }

  private async fillManagers(): Promise<void> {
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Account[]>>(
        `${environment.baseURL}/api/getManagers`
      )
    );
    this.state.managers = response.responseData || [];
  }

  async editHive(hive: Hive): Promise<void> {
    this.state.selectedHive = hive;
    const modal = await this.modalController.create({
      component: EditHiveModal,
      cssClass: "custom-edit-hive-modal",
    });
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Bee[]>>(
        `${environment.baseURL}/api/getBees`
      )
    );
    this.state.bees = response.responseData || [];
    modal.present();
  }

  async createHive(selectedField: Field): Promise<void> {
    const modal = await this.modalController.create({
      component: CreateHiveModal,
      cssClass: "custom-hive-modal",
    });
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Bee[]>>(
        `${environment.baseURL}/api/getBees`
      )
    );
    this.state.bees = response.responseData || [];
    this.state.selectedField = selectedField;
    modal.present();
  }

  async manageAccounts(): Promise<void> {
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Account[]>>(
        `${environment.baseURL}/api/getAccounts`
      )
    );
    this.state.accounts = (response.responseData || []).filter(
      (acc) => acc.name !== this.username
    );
    this.router.navigate(["/accounts-component"]);
  }

  private async fillKeepers(): Promise<void> {
    const response = await firstValueFrom(
      this.http.get<ResponseDto<Account[]>>(
        `${environment.baseURL}/api/getKeepers`
      )
    );
    this.state.keepers = response.responseData || [];
  }
}
