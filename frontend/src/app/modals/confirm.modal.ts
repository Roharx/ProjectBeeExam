import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { firstValueFrom } from "rxjs";
import { ResponseDto } from "../../models";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ToastController, ModalController } from "@ionic/angular";
import { State } from "../../state";
import { Router } from '@angular/router';
import { TokenService } from "../services/token.service";

@Component({
  template: `
    <div id="container">
      <ion-header>
        <ion-toolbar>
          <ion-title>Confirmation</ion-title>
        </ion-toolbar>
      </ion-header>
      <ion-content class="ion-padding">
        <p>Are you sure you want to remove the field?</p>
      </ion-content>
      <div id="bottom">
        <button id="accept" (click)="confirm()">Accept</button>
        <button id="cancel" (click)="cancel()">Cancel</button>
      </div>
    </div>
  `,
  styleUrls: ['../scss/confirm.modal.scss'],
  selector: 'confirm-modal'
})

export class ConfirmModal implements OnInit {

  @Output() confirmed: EventEmitter<boolean> = new EventEmitter<boolean>();

  createFieldForm = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required],
    manager: ['', Validators.required]
  });

  constructor(
    public fb: FormBuilder,
    public http: HttpClient,
    public state: State,
    private router: Router,
    private tokenService: TokenService,
    private modalController: ModalController,
    public toastController: ToastController
  ) { }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
    }
  }

  confirm() {
    this.modalController.dismiss({ confirmed: true });
  }

  cancel() {
    this.modalController.dismiss({ confirmed: false });
  }
}
