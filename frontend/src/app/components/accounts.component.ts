import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { State } from "../../state";
import { TokenService } from "../services/token.service";
import { JwtService } from "../services/jwt.service";
import { Account, AccountRank, ResponseDto } from "../../models";
import { firstValueFrom } from "rxjs";
import { environment } from "../../environments/environment";
import { ModalController, ToastController } from "@ionic/angular";
import { HttpClient } from "@angular/common/http";
import { CreateAccountModal } from "../modals/create-account.modal";
import { ChangeRankModal } from "../modals/change-rank.modal";

@Component({
  template: `
    <div id="page-container">
      <div id="top-bar">
        <div id="user-container" (click)="editAccount()">
          <div id="user-background">
            <img src="../assets/images/hive.png" height="50" width="50">
          </div>
          <div id="user-controls">
            <p id="username" (click)="editAccount()">{{ username }}</p>
            <p id="log-out" (click)="logout()">Log <span id="out">out</span></p>
          </div>
        </div>
      </div>
      <div id="middle-container">
        <p id="back-button" (click)="back()">< Back</p>
        <div id="accounts-container">
          <div *ngFor="let account of accounts">
            <div class="account-container">
              <p class="name">Name: {{ account.name }}</p>
              <p class="email">Email: {{ account.email }}</p>
              <p class="rank">Rank: {{ getRankLabel(account.rank) }}</p>
              <div class="buttons">
                <button class="reset-button" (click)="modifyAccount(account)">Change Rank</button>
                <button class="remove-button" (click)="removeAccount(account)">Remove</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <button id="add-button" (click)="createAccount()">+</button>
    </div>
  `,
  styleUrls: ['../scss/accounts.component.scss', '../scss/bees-feed.component.scss'],
  selector: 'accounts-component'
})

export class AccountsComponent implements OnInit {
  accounts: Account[] = [];
  username: string = '';

  constructor(
    private router: Router,
    private jwtService: JwtService,
    public toastController: ToastController,
    public modalController: ModalController,
    public http: HttpClient,
    public state: State,
    public tokenService: TokenService
  ) {}

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (token) {
      this.accounts = this.state.accounts;
      const decodedToken = this.jwtService.decodeToken(token);
      this.username = decodedToken ? decodedToken['sub'] : null;
    } else {
      this.router.navigate(['login-component']);
    }
  }

  back() {
    this.router.navigate(['bees-feed']);
  }

  async modifyAccount(account: Account) {
    this.state.selectedAccount = account;
    const modal = await this.modalController.create({
      component: ChangeRankModal,
      cssClass: 'custom-rank-modal'
    });
    modal.present();
  }

  async removeAccount(account: Account) {
    try {
      const response = await firstValueFrom(
        this.http.delete<ResponseDto<boolean>>(
          `${environment.baseURL}/api/DeleteAccount/${account.id}`
        )
      );
      this.accounts = this.accounts.filter(acc => acc.id !== account.id);
      await this.presentToast("Successfully removed field.", "success");
    } catch (ex) {
      await this.presentToast("Something went wrong.", "danger");
    }
  }

  getRankLabel(rank: number): string {
    return AccountRank[rank];
  }

  logout() {
    this.tokenService.removeToken();
    this.router.navigate(['login-component']);
  }

  async createAccount() {
    const modal = await this.modalController.create({
      component: CreateAccountModal,
      cssClass: 'custom-create-account-modal'
    });
    modal.present();
  }

  editAccount() {
    this.router.navigate(['user-component']);
  }

  private async presentToast(message: string, color: string) {
    const toast = await this.toastController.create({
      message,
      duration: 5000,
      color
    });
    await toast.present();
  }
}
