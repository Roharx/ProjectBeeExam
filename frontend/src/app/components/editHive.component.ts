import {Component, OnInit} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Hive, ResponseDto} from "../../models";
import {HttpClient} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";
import {JwtService} from "../services/jwt.service";
import {ModalController} from '@ionic/angular';
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";

@Component({
  template: `
    <div>
      <div id="top">
        <p id="title">Edit Hive</p>
      </div>
      <div id="middle">
        <ion-list>
          <ion-item>
            <ion-label position="floating">Field ID:</ion-label>
            <ion-input [formControl]="editHiveForm.controls.field_Id" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Name</ion-label>
            <ion-input [formControl]="editHiveForm.controls.name" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Location</ion-label>
            <ion-input [formControl]="editHiveForm.controls.location" aria-labelledby></ion-input>
          </ion-item>
          <ion-item
            [ngClass]="{'invalid-input': editHiveForm.controls.placement.invalid && editHiveForm.controls.placement.touched}">
            <ion-label position="floating">Installed (yyyy-mm-dd)</ion-label>
            <ion-input [formControl]="editHiveForm.controls.placement" aria-labelledby></ion-input>
          </ion-item>
          <ion-item
            [ngClass]="{'invalid-input': editHiveForm.controls.last_Check.invalid && editHiveForm.controls.last_Check.touched}">
            <ion-label position="floating">Last Checked (yyyy-mm-dd hh:mm:ss)</ion-label>
            <ion-input [formControl]="editHiveForm.controls.last_Check" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label>Ready to Harvest</ion-label>
            <button id="ready" (click)="toggleReady()">
              {{ this.hive.ready ? 'Ready' : 'Unready' }}
            </button>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Color</ion-label>
            <ion-input [formControl]="editHiveForm.controls.color" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label position="floating">Comment</ion-label>
            <ion-input [formControl]="editHiveForm.controls.comment" aria-labelledby></ion-input>
          </ion-item>
          <ion-item>
            <ion-label>Bee</ion-label>
            <ion-select placeholder="Select Bee" [formControl]="editHiveForm.controls.bee_Type"
                        label="Bee">
              <ion-select-option *ngFor="let bee of this.state.bees" [value]="bee.id">
                {{ bee.name }}
              </ion-select-option>
            </ion-select>
          </ion-item>
        </ion-list>
      </div>
      <div id="bottom">
        <button id="accept" (click)="updateHive()">Accept</button>
        <button id="remove" (click)="removeHive(this.hive.id!)">Remove</button>
        <button id="cancel" (click)="closeModal()">Cancel</button>
      </div>
    </div>
  `,
  styleUrls: ['../css/createField.component.scss', '../css/hive.component.scss'],
  selector: 'edit-hive-component'
})

export class EditHiveComponent implements OnInit {
  hive: Hive = new Hive();

  editHiveForm = this.fb.group({//naming is different, fix in later update
    id: this.hive.id,
    field_Id: this.hive.field_Id,
    name: ['', Validators.required],
    location: ['', Validators.required],
    placement: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)]],
    last_Check: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/)]],//this
    ready: this.hive.ready,
    color: ['', Validators.required],
    comment: [''],
    bee_Type: this.hive.bee_Type
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State, private jwtService: JwtService,
              public toastController: ToastController, private router: Router, private tokenService: TokenService,
              private modalController: ModalController) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token)
      this.router.navigate(['login-component']);
    else {
      this.hive = this.state.selectedHive;
      this.editHiveForm.patchValue({
        id: this.hive.id,
        field_Id: this.hive.field_Id,
        name: this.hive.name,
        location: this.hive.location,
        placement: this.hive.placement,
        last_Check: this.hive.last_Check,
        ready: this.hive.ready,
        color: this.hive.color,
        bee_Type: this.hive.bee_Type,
        comment: this.hive.comment
      })

    }
  }

  async updateHive() {
    try {
      this.editHiveForm.patchValue({//fine for now, optimize in patch (the initial value of the form ready is acting weirdly)
        ready: this.hive.ready
      })

      if (this.editHiveForm.valid) {
        const headers = {//leave in for later update
          Authorization: `Bearer ${this.tokenService.getToken()}`,
        };
        try {
          const response = await firstValueFrom(
            this.http.put<ResponseDto<boolean>>(
              environment.baseURL + '/api/updateHive',
              this.editHiveForm.getRawValue(),
              {headers}
            )
          );
          if (response) {
            this.closeModal();
            //window.location.reload();
            const toast = await this.toastController.create({
              message: "Successfully updated hive.",
              duration: 5000,
              color: "success"
            })
            await toast.present();
          } else {
            const toast = await this.toastController.create({
              message: "Failed to update hive.",
              duration: 4500,
              color: "danger"

            })
            await toast.present();
          }
        } catch (ex) {

        }

      } else {
        const toast = await this.toastController.create({
          message: "Please fill out all the fields correctly.",
          duration: 4500,
          color: "danger"

        })
        await toast.present();
        Object.keys(this.editHiveForm.controls).forEach(controlName => {
          const control = this.editHiveForm.get(controlName);
          if (control && control.invalid) {
            console.log(`Control "${controlName}" is invalid. Errors:`, control.errors);
          }
        });
      }

    } catch (ex) {
      const toast = await this.toastController.create({
        message: "Failed to update hive.",
        duration: 4500,
        color: "danger"

      })
      await toast.present();
    }
  }

  closeModal() {
    this.modalController.dismiss();
  }

  async removeHive(id: number) {
    try {
      const response = await firstValueFrom(
        this.http.delete<ResponseDto<boolean>>(
          environment.baseURL + '/api/DeleteHive/' + id
        )
      );
      this.closeModal();
      window.location.reload();
      const toast = await this.toastController.create({
        message: "Successfully removed field.",
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

  toggleReady() {
    this.hive.ready = !this.hive.ready;
    console.log(this.hive.ready);
  }
}
