import {Component, OnInit} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";
import {ModalController} from '@ionic/angular';

@Component({
  template: `
    <div>
      <div id="top">
        <p id="title">Create Field</p>
      </div>
      <div id="middle">
        <ion-list>
          <ion-item>
            <ion-input [formControl]="createFieldForm.controls.name" label="Field Name:"></ion-input>
          </ion-item>
          <ion-item>
            <ion-input [formControl]="createFieldForm.controls.location"
                       label="Field Location:"></ion-input>
          </ion-item>
          <ion-item>
            <ion-label>Manager</ion-label>
            <ion-select placeholder="Select Manager" [formControl]="createFieldForm.controls.manager"
                        label="Manager">
              <ion-select-option *ngFor="let manager of this.state.managers" [value]="manager.id">
                {{ manager.name }}
              </ion-select-option>
            </ion-select>
          </ion-item>
        </ion-list>
      </div>
      <div id="bottom">
        <button id="accept" (click)="createField()">Accept</button>
        <button id="cancel" (click)="closeModal()">Cancel</button>
      </div>
    </div>
  `,
  styleUrls: ['../css/createField.component.scss'],
  selector: 'create-field-component'
})

export class CreateFieldComponent implements OnInit {


  createFieldForm = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required],
    manager: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State,
              public toastController: ToastController, private router: Router, private tokenService: TokenService,
              private modalController: ModalController) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
    }
  }

  async createField() {
    try {
      const formValue = this.createFieldForm.value;

      const requestBody = {
        fieldName: formValue.name,
        fieldLocation: formValue.location
      };

      const createResponse = await firstValueFrom(
        this.http.post<ResponseDto<number>>(environment.baseURL + '/api/createField', requestBody));

      const connectBody = {
        accountId: formValue.manager,
        fieldId: createResponse.responseData
      }
      const connectResponse = await firstValueFrom(this.http.post<ResponseDto<Boolean>>(
        environment.baseURL + '/api/ConnectFieldAndAccount', connectBody))

      if (connectResponse) {
        this.closeModal();
        window.location.reload();
        const toast = await this.toastController.create({
          message: "Successfully created Field.",
          duration: 5000,
          color: "success"
        })
        await toast.present();
      } else {
        const toast = await this.toastController.create({
          message: "Failed to create field.",
          duration: 4500,
          color: "danger"

        })
        await toast.present();
      }

    } catch (ex) {
      const toast = await this.toastController.create({
        message: "Failed to create field.",
        duration: 4500,
        color: "danger"

      })
      await toast.present();
    }
  }

  closeModal() {
    this.modalController.dismiss();
  }
}
