import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {State} from "../../state";
import {Router} from '@angular/router';
import {TokenService} from "../services/token.service";

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
  styleUrls: ['../scss/login.component.scss'],
  selector: 'login-component'
})

export class LoginComponent {
  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  })

  constructor(
    public fb: FormBuilder,
    public http: HttpClient,
    public state: State,
    public toastController: ToastController,
    private router: Router,
    private tokenService: TokenService
  ) {}

  async login() {
    try {
      const response = await firstValueFrom(
        this.http.put<ResponseDto<string>>(
          environment.baseURL + '/api/checkPassword',
          this.loginForm.getRawValue()
        )
      );

      response.responseData !== null
        ? this.onSuccessfulLogin(response.responseData!)
        : await this.showErrorMessage("Wrong username or password!");
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        await this.showErrorMessage(error.error.messageToClient);
      }
    }
  }

  async onSuccessfulLogin(token: string) {
    this.tokenService.saveToken(token);
      this.router.navigate(['/bees-feed']);
  }

  focusPasswordInput() {
    const passwordInput = document.getElementById('password-bar') as HTMLInputElement;
    if (passwordInput) {
      passwordInput.focus();
    }
  }

  private async showErrorMessage(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 4500,
      color: "danger",
    });
    await toast.present();
  }
}
