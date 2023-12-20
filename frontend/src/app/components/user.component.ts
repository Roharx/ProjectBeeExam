import {Component, OnInit } from "@angular/core";
import {Router} from "@angular/router";
import {State} from "../../state";
import {TokenService} from "../services/token.service";
import {JwtService} from "../services/jwt.service";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Account, AccountCreateUpdateDto, ResponseDto} from "../../models";
import {environment} from "../../environments/environment";
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";

@Component({
  template: `
    <div id="page-container">
      <div id="interface">
        <div id="interface-top">
          <p id="back-button" (click)="back()">< Back</p>
        </div>
        <div id="interface-middle">
          <p>Username:</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="updateForm.controls.username">
          <p>Email:</p>
          <input type="text" class="text-bar" id="email-bar" [formControl]="updateForm.controls.email">


          <!-- Password -->
          <p>Password:</p>
          <input type="password" class="text-bar" id="password-bar" [formControl]="updateForm.controls.password"
                 [ngClass]="{'red-border': updateForm.controls.password.invalid && updateForm.controls.password.touched}">
          <div *ngIf="updateForm.controls.password.invalid && updateForm.controls.password.touched"
               style="color: var(--red-color);">
            Minimum 6 characters.
          </div>

          <!-- Repeat Password -->
          <p>Repeat Password:</p>
          <input type="password" class="text-bar" id="password-repeat-bar"
                 [formControl]="updateForm.controls.repeatPassword"
                 [ngClass]="{'red-border': updateForm.controls.repeatPassword.touched && updateForm.controls.repeatPassword.value !== updateForm.controls.password.value}">
          <div
              *ngIf="updateForm.controls.repeatPassword.touched && updateForm.controls.repeatPassword.value !== updateForm.controls.password.value"
              style="color: var(--red-color);">
            Passwords do not match.
          </div>
        </div>
        <div id="interface-bottom">
          <button id="save-button" (click)="save()">Save</button>
        </div>

      </div>


    </div>
  `,
  styleUrls: ['../css/user.component.scss'],
  selector: 'user-component'
})

export class UserComponent implements OnInit {
  id: any;
  rank: any;
  updateAccount: AccountCreateUpdateDto = new AccountCreateUpdateDto();

  updateForm = this.fb.group({
    username: [''],
    email:['', Validators.email],
    password: [
      '',
      [
        Validators.required,
        Validators.minLength(6),
      ]
    ],
    repeatPassword:['']
  })
  constructor(private router: Router, private jwtService: JwtService, public fb: FormBuilder,
              public toastController: ToastController, public state: State, public tokenService: TokenService,
              public http: HttpClient) {
  }
  ngOnInit(): void {

    const token = this.tokenService.getToken();

    if (token) {
      const decodedToken = this.jwtService.decodeToken(token);

      //editable fields
      this.updateForm.patchValue({
        username: decodedToken ? decodedToken['sub'] : null,
        email: decodedToken ? decodedToken['email'] : null
      })

      //non-editable fields
      this.id = decodedToken ? decodedToken['id'] : null;
      this.rank = decodedToken ? decodedToken['rank'] : null;

    } else {
      this.router.navigate(['login-component']);
    }
  }

  back() {
    this.router.navigate(['bees-feed']);
  }
  async save() {
    //update account
    if (this.updateForm.get('password')?.value === this.updateForm.get('repeatPassword')?.value)
    {

      this.updateAccount.id = this.id;
      this.updateAccount.name = this.updateForm.getRawValue().username!;
      this.updateAccount.email = this.updateForm.getRawValue().email!;
      this.updateAccount.rank = this.rank;
      this.updateAccount.password = this.updateForm.getRawValue().password!;


      const headers = {//leave in for later update
        Authorization: `Bearer ${this.tokenService.getToken()}`,
      };
      try{
        const response = await firstValueFrom(
          this.http.put<ResponseDto<boolean>>(
            environment.baseURL + '/api/updateAccount',
            this.updateAccount,
            { headers }
          )
        );
        this.tokenService.removeToken();
        await this.router.navigate(['login-component']);
        const toast = await this.toastController.create({
          message: "Successfully updated account info. Please login with your new details.",
          duration: 5000,
          color: "success"
        })
        await toast.present();
      }
      catch (ex){
        if (ex instanceof HttpErrorResponse) {
          const toast = await this.toastController.create({
            message: "Couldn't perform update.",
            duration: 4500,
            color: "danger"

          })
          await toast.present();
        }
      }
    }
    else{
      const toast = await this.toastController.create({
        message: "The password and repeat password contents are different.",
        duration: 4500,
        color: "danger"
      })
      await toast.present();
    }

  }
}
