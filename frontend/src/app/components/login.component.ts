import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {Account, Field, ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";
import {JwtService} from "../services/jwt.service";

@Component({
  template: `
    <div id="page-container">

      <div id="content-container">

        <div id="title-container">
          <p class="title" style="padding-bottom: 0; margin-bottom: 0;">Welcome to</p>
          <p class="title" style="padding-left: 90px; padding-top: 0; margin-top: 0;">Project <span
            style="color: var(--effect-color)">Bee</span></p>
        </div>

        <div id="login-container">
          <p>Username</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="loginForm.controls.username"
                 (keydown.enter)="focusPasswordInput()">
          <p>Password</p>
          <input type="password" class="text-bar" id="password-bar" [formControl]="loginForm.controls.password"
                 (keydown.enter)="login()">
          <div id="button-container">
            <button class="button" id="login-button" (click)="login()">Login</button>
            <button class="button" id="recover-button">Recover</button>
          </div>

        </div>

      </div>
      <div id="picture-container">
        <img src="../../assets/images/hive.png" alt="Hive Image">
      </div>
    </div>


  `,
  styleUrls: ['../css/login.component.scss'],
  selector: 'login-component'
})

export class LoginComponent {
  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State, private jwtService: JwtService,
              public toastController: ToastController, private router: Router, private tokenService: TokenService) {
  }

  async login() {
    try {
      const response =
        await firstValueFrom(this.http.put<ResponseDto<string>>(environment.baseURL + '/api/checkPassword', this.loginForm.getRawValue()));

      if (response.responseData != null) {
        this.onSuccessfulLogin(response.responseData)
      } else {
        const toast = await this.toastController.create({
          message: "Wrong username or password!",
          duration: 4500,
          color: "danger"
        })
        await toast.present();
      }
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        const toast = await this.toastController.create({
          message: error.error.messageToClient,
          duration: 4500,
          color: "danger"

        })
        await toast.present();
      }
    }


  }

  async onSuccessfulLogin(token: string) {
    const username = this.loginForm.getRawValue().username!.toString(); // Assuming your API response directly contains the token

    // Save the token to the local storage using your TokenService
    this.tokenService.saveToken(token);
    const decodedToken = this.jwtService.decodeToken(token);
    const rank = decodedToken ? decodedToken['rank'] : null;
    if (rank > 0)
      this.router.navigate(['/bees-feed']);
    else {
      var response = await firstValueFrom(
        this.http.get<ResponseDto<Account[]>>(environment.baseURL + '/api/getAccounts/'));
      this.state.accounts = response.responseData!;
      this.router.navigate(['/accounts-component']);
    }

  }

  focusPasswordInput() {
    const passwordInput = document.getElementById('password-bar') as HTMLInputElement;
    if (passwordInput) {
      passwordInput.focus();
    }
  }
}
