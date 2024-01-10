import {Component, OnInit} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Account, AccountRank, ResponseDto} from "../../models";
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
        <p id="title">Create Account</p>
      </div>
      <div id="middle">
        <ion-list>
          <ion-item>
            <ion-input [formControl]="createAccountForm.controls.name" label="Name:"></ion-input>
          </ion-item>
          <ion-item>
            <ion-input [formControl]="createAccountForm.controls.email" label="Email:"></ion-input>
          </ion-item>
        </ion-list>
      </div>
      <div id="bottom">
        <button id="accept" (click)="createAccount()">Accept</button>
        <button id="cancel" (click)="closeModal()">Cancel</button>
      </div>
    </div>
  `,
  styleUrls: ['../scss/create-field.modal.scss'],
  selector: 'create-account-component'
})

export class CreateAccountModal implements OnInit {


  createAccountForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', Validators.required],
    password: ['', Validators.required]
  })

  constructor(public fb: FormBuilder,
              public http: HttpClient,
              public state: State,
              public toastController: ToastController,
              private router: Router, private tokenService: TokenService,
              private modalController: ModalController) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
    }
  }

  async createAccount() {
    try {
      const formValue = this.createAccountForm.value;

      const requestBody = {
        name: formValue.name,
        email: formValue.email,
        password: '123456', //placeholder
        rank: AccountRank.Guest
      };

      const createResponse = await firstValueFrom(
        this.http.post<ResponseDto<number>>(environment.baseURL + '/api/createAccount', requestBody)
      );

      if (createResponse != -1!) {
        this.closeModal();

        const newAccount: Account = {
          email: requestBody.email!,
          id: createResponse.responseData!,
          name: requestBody.name!,
          rank: requestBody.rank
        }

        this.state.accounts.push(newAccount)
        this.showPopupMessage("Successfully created account.", false);
      } else {
        this.showPopupMessage("Failed to create account.", true);
      }

    } catch (ex) {
      this.showPopupMessage("Failed to create field.", true);
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
