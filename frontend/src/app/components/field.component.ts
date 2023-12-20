import {Component, OnInit} from "@angular/core";
import {FormBuilder} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Account, AccountRank, Bee, Field, ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ModalController, ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";
import {CreateHiveComponent} from "./createHive.component";
import {JwtService} from "../services/jwt.service";

@Component({
  template: `
    <div id="page-container">
      <div id="interface">
        <div id="interface-top">
          <p id="back-button" (click)="back()"> < Back </p>
        </div>
        <div id="interface-middle">
          <p>Name:</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="updateForm.controls.fieldName">
          <p>Location:</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="updateForm.controls.fieldLocation">

          <p>Managers:</p>
          <ul id="manager-list">
            <li *ngFor="let manager of this.state.managers">
              <p>{{ manager.name }}</p>
              <button class="remove-button" [disabled]="!isManagerInField(manager)" (click)="removeManager(manager)">-
              </button>
              <button class="add-manager-button" [disabled]="isManagerInField(manager)" (click)="addManager(manager)">
                +
              </button>
            </li>
          </ul>
        </div>
        <div id="interface-bottom">
          <button id="save-button" (click)="save()">Save</button>
          <button id="cancel-button" (click)="back()">Cancel</button>
        </div>
        <button id="remove-button" (click)="remove(this.field.id)">Remove</button>
      </div>
      <button id="add-button" (click)="createHive()">+</button>
    </div>
  `,
  styleUrls: ['../css/user.component.scss', '../css/field.component.scss'],
  selector: 'field-component'
})

export class FieldComponent implements OnInit {
  field: Field = new Field();
  updateForm = this.fb.group({
    fieldId: this.field.id,
    fieldName: this.field.name,
    fieldLocation: this.field.location
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State, public toastController: ToastController,
              public modalController: ModalController, private router: Router, private tokenService: TokenService,
              private jwtService: JwtService) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (token) {
      const decodedToken = this.jwtService.decodeToken(token);
      const rank = decodedToken ? decodedToken['rank'] : null;
      if(rank != 1){
        this.router.navigate(['bees-feed']);
        this.showRankError();
      }


      this.field = this.state.selectedField;

      this.updateForm.patchValue({
        fieldId: this.field.id,
        fieldName: this.field.name,
        fieldLocation: this.field.location
      })

    } else {
      this.router.navigate(['login-component']);
    }
  }

  private async showRankError() {
    const toast = await this.toastController.create({
      message: "Only field managers can modify fields.",
      duration: 5000,
      color: "danger"
    })
    await toast.present();
  }

  async createHive() {
    const modal = await this.modalController.create({
      component: CreateHiveComponent,
      cssClass: 'custom-hive-modal'
    });
    var response = await firstValueFrom(
      this.http.get<ResponseDto<Bee[]>>(environment.baseURL + '/api/getBees'));
    this.state.bees = response.responseData!;

    modal.present();
  }

  async addManager(manager: Account) { //quick but unoptimized solution, optimizing in next patch to only update on save
    const body = {
      accountId: manager.id,
      fieldId: this.field.id
    };
    const response = await firstValueFrom(
      this.http.post<ResponseDto<boolean>>(
        environment.baseURL + '/api/ConnectFieldAndAccount',
        body));
    this.field.managers!.push(manager)
  }

  async removeManager(manager: Account) {
    const body = {
      accountId: manager.id,
      fieldId: this.field.id
    };
    const response = await firstValueFrom(
      this.http.put<ResponseDto<boolean>>(
        environment.baseURL + '/api/DisconnectFieldAndAccount',
        body));
    this.field.managers = this.field.managers!.filter(item => item.id !== manager.id);
  }

  isManagerInField(manager: Account): boolean {
    return this.state.selectedField.managers?.some(m => m.id === manager.id) ?? false;
  }

  back() {
    this.router.navigate(['bees-feed']);
  }

  async save() {
    const headers = {//leave in for later update
      Authorization: `Bearer ${this.tokenService.getToken()}`,
    };
    try {
      const response = await firstValueFrom(
        this.http.put<ResponseDto<boolean>>(
          environment.baseURL + '/api/updateField',
          this.updateForm.getRawValue(),
          {headers}
        )
      );

      const toast = await this.toastController.create({
        message: "Successfully updated account info. Please login with your new details.",
        duration: 5000,
        color: "success"
      })
      await toast.present();
    } catch (ex) {
      const toast = await this.toastController.create({
        message: "Something went wrong.",
        duration: 5000,
        color: "danger"
      })
      await toast.present();
    }
  }

  async remove(id: number) {
    try {
      const response = await firstValueFrom(
        this.http.delete<ResponseDto<boolean>>(
          environment.baseURL + '/api/DeleteField/' + id
        )
      );
      this.back();
      const toast = await this.toastController.create({
        message: "Successfully removed field.",
        duration: 5000,
        color: "success"
      })
      await toast.present();
    }
    catch (ex){
      const toast = await this.toastController.create({
        message: "Something went wrong.",
        duration: 5000,
        color: "danger"
      })
      await toast.present();
    }

  }
}

