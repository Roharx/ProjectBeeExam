import {Component, OnInit} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Account, ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";
import {JwtService} from "../services/jwt.service";
import {ModalController} from '@ionic/angular';

@Component({
  template: `
    <div>
      <div id="top">
        <p id="title">Create Hive</p>
      </div>
      <div id="middle">
        <ion-list>
          <ion-item>
            <ion-label position="floating">Name</ion-label>
            <ion-input [formControl]="createHiveForm.controls.name" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Location</ion-label>
            <ion-input [formControl]="createHiveForm.controls.location" aria-labelledby></ion-input>
          </ion-item>
          <ion-item
            [ngClass]="{'invalid-input': createHiveForm.controls.placement.invalid && createHiveForm.controls.placement.touched}">
            <ion-label position="floating">Installed (yyyy-mm-dd)</ion-label>
            <ion-input [formControl]="createHiveForm.controls.placement" aria-labelledby
                       (input)="formatDate($event)" maxlength="10"></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Color</ion-label>
            <ion-input [formControl]="createHiveForm.controls.color" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Comment</ion-label>
            <ion-input [formControl]="createHiveForm.controls.comment" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label>Bee</ion-label>
            <ion-select placeholder="Select Bee" [formControl]="createHiveForm.controls.bee"
                        aria-label="Bee">
              <ion-select-option *ngFor="let bee of this.state.bees" [value]="bee.id">
                {{ bee.name }}
              </ion-select-option>
            </ion-select>
          </ion-item>
        </ion-list>
      </div>
      <div id="bottom">
        <button id="accept" (click)="createHive()">Accept</button>
        <button id="cancel" (click)="closeModal()">Cancel</button>
      </div>
    </div>
  `,
  styleUrls: ['../scss/create-field.modal.scss', '../scss/create-hive.modal.scss'],
  selector: 'create-hive-component'
})

export class CreateHiveModal implements OnInit {


  createHiveForm = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required],
    placement: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)]],
    color: ['', Validators.required],
    comment: [''],
    bee: ['', Validators.required]
  })

  constructor(public fb: FormBuilder,
              public http: HttpClient,
              public state: State,
              private jwtService: JwtService,
              public toastController: ToastController,
              private router: Router,
              private tokenService: TokenService,
              private modalController: ModalController)
  {  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token)
      this.router.navigate(['login-component']);
  }

  async createHive() {
    try {
      if (this.createHiveForm.valid) {
        const formValue = this.createHiveForm.value;
        const decodedToken = this.jwtService.decodeToken(this.tokenService.getToken()!);

        const requestBody = {
          fieldId: this.state.selectedField.id,
          name: formValue.name,
          location: formValue.location,
          placementDate: formValue.placement,
          lastCheck: formValue.placement + " 00:00:00",
          readyToHarvest: false,
          color: formValue.color,
          comment: formValue.comment,
          beeId: formValue.bee
        };

        const createResponse = await firstValueFrom(
          this.http.post<ResponseDto<number>>(environment.baseURL + '/api/createHive', requestBody));

        if (createResponse !== -1!) {
          this.closeModal();
          window.location.reload();
          this.showPopupMessage("Successfully created Field.", false);
        } else {
          this.showPopupMessage("Failed to create field.");
        }
      } else {
        this.showPopupMessage("Please fill out all the fields correctly.");
      }

    } catch (ex) {
      this.showPopupMessage("Failed to create field.");
    }
  }

  formatDate(event: any) {
    let value = event.target.value.replace(/[^0-9]/g, '');

    let formattedValue = '';
    for (let i = 0; i < value.length; i++) {
      if (i === 4 || i === 6) {
        formattedValue += '-';
      }
      formattedValue += value[i];
    }

    // Check if the formatted value exceeds the maxlength
    if (formattedValue.length <= 10) {
      this.createHiveForm.controls.placement.setValue(formattedValue);
    }
  }


  closeModal() {
    this.modalController.dismiss();
  }

  async showPopupMessage(errorMessage: string, error: boolean = true) {
    const toast = await this.toastController.create({
      message: errorMessage,
      duration: 4500,
      color: error ? "danger" : "success"

    })
    await toast.present();
  }
}
