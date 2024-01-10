import {Component, OnInit} from "@angular/core";
import {FormBuilder} from "@angular/forms";
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
            <p>Name: {{ state.selectedAccount.name }}</p>
          </ion-item>
          <ion-item>
            <p>Email: {{ state.selectedAccount.email }}</p>
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
  styleUrls: ['../scss/create-field.modal.scss'],
  selector: 'rank-modal-component'
})

export class ChangeRankModal implements OnInit {
  selectedRank: AccountRank = 0;
  accountRanks = Object.values(AccountRank).filter(value => typeof value === 'string');

  constructor(
    public http: HttpClient,
    public state: State,
    public toastController: ToastController,
    private router: Router,
    private tokenService: TokenService,
    private modalController: ModalController,
  ) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
    }
  }

  async save() {
    try {
      const requestBody = {
        accountId: this.state.selectedAccount.id,
        rank: AccountRank[this.selectedRank]
      };
      const createResponse = await firstValueFrom(
        this.http.put<ResponseDto<boolean>>(`${environment.baseURL}/api/modifyRank`, requestBody)
      );

      if (createResponse) {
        this.state.selectedAccount.rank = AccountRank[this.selectedRank] as unknown as AccountRank;
        this.closeModal();

        this.showPopupMessage("Successfully modified rank.", false);
      } else {
        this.showPopupMessage("Failed to modify rank.");
      }
    } catch (ex) {
      this.showPopupMessage("Failed to modify rank.");
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
