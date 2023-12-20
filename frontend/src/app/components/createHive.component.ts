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
            <ion-input [formControl]="createFieldForm.controls.name" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Location</ion-label>
            <ion-input [formControl]="createFieldForm.controls.location" aria-labelledby></ion-input>
          </ion-item>
          <ion-item
            [ngClass]="{'invalid-input': createFieldForm.controls.placement.invalid && createFieldForm.controls.placement.touched}">
            <ion-label position="floating">Installed (yyyy-mm-dd hh:mm)</ion-label>
            <ion-input [formControl]="createFieldForm.controls.placement" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Color</ion-label>
            <ion-input [formControl]="createFieldForm.controls.color" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Comment</ion-label>
            <ion-input [formControl]="createFieldForm.controls.comment" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label>Bee</ion-label>
            <ion-select placeholder="Select Bee" [formControl]="createFieldForm.controls.bee"
                        label="Bee">
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
  styleUrls: ['../css/createField.component.scss', '../css/createHiveExtraStyling.component.scss'],
  selector: 'create-hive-component'
})

export class CreateHiveComponent implements OnInit {


  createFieldForm = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required],
    placement: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$/)]],
    color: ['', Validators.required],
    comment: [''],
    bee: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State, private jwtService: JwtService,
              public toastController: ToastController, private router: Router, private tokenService: TokenService,
              private modalController: ModalController) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token)
      this.router.navigate(['login-component']);
  }

  async createHive() {
    try {
      if (this.createFieldForm.valid) {
        const formValue = this.createFieldForm.value;
        const decodedToken = this.jwtService.decodeToken(this.tokenService.getToken()!);

        const requestBody = {
          fieldId: this.state.selectedField.id,
          name: formValue.name,
          location: formValue.location,
          placementDate: formValue.placement!.split(' ')[0],
          lastCheck: formValue.placement + ":00",
          readyToHarvest: false,
          color: formValue.color,
          comment: formValue.comment,
          beeId: formValue.bee
        };

        const createResponse = await firstValueFrom(
          this.http.post<ResponseDto<number>>(environment.baseURL + '/api/createHive', requestBody));

        if (createResponse !== -1!) {
          this.closeModal();
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
      } else {
        const toast = await this.toastController.create({
          message: "Please fill out all the fields correctly.",
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
