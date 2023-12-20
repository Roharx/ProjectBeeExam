import {Component, numberAttribute, OnInit} from "@angular/core";
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
        <p id="title">Modify Rank</p>
      </div>
      <div id="middle">
        <ion-list>
          <ion-item>
            <p>Name: {{ this.state.selectedAccount.name }}</p>
          </ion-item>
          <ion-item>
            <p>Email: {{ this.state.selectedAccount.email }}</p>
          </ion-item>
          <ion-item>
            <ion-select placeholder="Select Rank" [(ngModel)]="selectedRank" name="selectedRank"
                        aria-label="'Rank'" [ngModelOptions]="{standalone: true}">
              <ion-select-option *ngFor="let rank of accountRanks" [value]="rank">{{ rank }}</ion-select-option>
            </ion-select>
          </ion-item>
        </ion-list>
      </div>
      <div id="bottom">
        <button id="accept" (click)="save()">Accept</button>
        <button id="cancel" (click)="closeModal()">Cancel</button>
      </div>
    </div>

  `,
  styleUrls: ['../css/createField.component.scss'],
  selector: 'rank-modal-component'
})

export class RankModalComponent implements OnInit {
  selectedRank: AccountRank = 0;
  accountRanks = Object.values(AccountRank).filter(value => typeof value === 'string');
  constructor(public http: HttpClient, public state: State,
              public toastController: ToastController, private router: Router, private tokenService: TokenService,
              private modalController: ModalController) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
    }
  }

  async save() {
    try {
      const rankNumber: number = this.selectedRank;
      const requestBody = {
        accountId: this.state.selectedAccount.id,
        rank: AccountRank[this.selectedRank]
      };
      console.log(requestBody);
      const createResponse = await firstValueFrom(
        this.http.put<ResponseDto<boolean>>(environment.baseURL + '/api/modifyRank', requestBody));

      if (createResponse) {
        this.closeModal();

        const toast = await this.toastController.create({
          message: "Successfully created account.",
          duration: 5000,
          color: "success"
        })
        await toast.present();
      } else {
        const toast = await this.toastController.create({
          message: "Failed to create account.",
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
