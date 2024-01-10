import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { firstValueFrom } from "rxjs";
import { Account, Field, ResponseDto } from "../../models";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ModalController, ToastController } from "@ionic/angular";
import { State } from "../../state";
import { Router } from '@angular/router';
import { TokenService } from "../services/token.service";
import { JwtService } from "../services/jwt.service";
import { ConfirmModal } from "../modals/confirm.modal";
@Component({
  template: `
    <div id="page-container">
      <div id="interface">
        <div id="interface-top">
          <p id="back-button" (click)="back()"> < Back </p>
        </div>
        <div id="interface-middle">
          <p>Name:</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="updateForm.controls.fieldName">
          <p>Location:</p>
          <input type="text" class="text-bar" id="username-bar" [formControl]="updateForm.controls.fieldLocation">

          <div class="table-container">
            <table class="select-table">
              <thead>
              <tr>
                <th>Managers</th>
                <th>Assign</th>
              </tr>
              </thead>
              <tbody>
              <tr *ngFor="let manager of this.state.managers">
                <td>{{ manager.name }}</td>
                <td>
                  <button class="remove-manager-button" [disabled]="!isManagerInField(manager)" (click)="addOrRemoveAccount(manager, false)">-</button>
                  <button class="add-manager-button" [disabled]="isManagerInField(manager)" (click)="addOrRemoveAccount(manager, true)">+</button>
                </td>
              </tr>
              </tbody>
            </table>
            <table class="select-table">
              <thead>
              <tr>
                <th>Bee Keepers</th>
                <th>Assign</th>
              </tr>
              </thead>
              <tbody>
              <tr *ngFor="let keeper of this.state.keepers">
                <td>{{ keeper.name }}</td>
                <td>
                  <button class="remove-manager-button" [disabled]="!isManagerInField(keeper)" (click)="addOrRemoveAccount(keeper, false)">-</button>
                  <button class="add-manager-button" [disabled]="isManagerInField(keeper)" (click)="addOrRemoveAccount(keeper, true)">+</button>
                </td>
              </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div id="interface-bottom">
          <button id="save-button" (click)="save()">Save</button>
          <button id="remove-button" (click)="confirmRemove()">Remove</button>
          <button id="cancel-button" (click)="back()">Cancel</button>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['../scss/user.component.scss', '../scss/edit-field.component.scss'],
  selector: 'field-component'
})

export class EditFieldComponent implements OnInit {
  field: Field = new Field();
  updateForm = this.fb.group({
    fieldId: this.field.id,
    fieldName: this.field.name,
    fieldLocation: this.field.location
  })

  constructor(
    public fb: FormBuilder,
    public http: HttpClient,
    public state: State,
    public toastController: ToastController,
    public modalController: ModalController,
    private router: Router,
    private tokenService: TokenService,
    private jwtService: JwtService
  ) {
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();

    if (!token) {
      this.router.navigate(['login-component']);
      return;
    }

    const decodedToken = this.jwtService.decodeToken(token);
    const rank = decodedToken ? decodedToken['rank'] : null;
    if (rank > 1) {
      this.router.navigate(['bees-feed']);
    }

    this.field = this.state.selectedField;

    this.updateForm.patchValue({
      fieldId: this.field.id,
      fieldName: this.field.name,
      fieldLocation: this.field.location
    });
  }

  async addOrRemoveAccount(account: Account, isAdding: boolean) {
    const body = {
      accountId: account.id,
      fieldId: this.field.id
    };
    const response = await firstValueFrom(
      this.http.request<ResponseDto<boolean>>(
        isAdding ? 'POST' : 'PUT',
        `${environment.baseURL}/api/${isAdding ? 'Connect' : 'Disconnect'}FieldAndAccount`,
        { body }
      )
    );
    const accountsList = this.field.accounts || [];
    if (isAdding) {
      accountsList.push(account);
    } else {
      this.field.accounts = accountsList.filter(item => item.id !== account.id);
    }
  }

  isManagerInField(account: Account): boolean {
    return this.field.accounts?.some(item => item.id === account.id) || false;
  }

  back() {
    this.router.navigate(['bees-feed']);
  }

  async save() {
    try {
      const response = await firstValueFrom(
        this.http.put<ResponseDto<boolean>>(
          `${environment.baseURL}/api/updateField`,
          this.updateForm.getRawValue(),
          { headers: { Authorization: `Bearer ${this.tokenService.getToken()}` } }
        )
      );
      this.showToast("Successfully updated field.", "success");
    } catch (ex) {
      this.showToast("Something went wrong.", "danger");
    }
  }

  async confirmRemove() {
    const modal = await this.modalController.create({
      component: ConfirmModal,
      cssClass: 'custom-confirm-modal'
    });
    modal.present();

    const { data } = await modal.onDidDismiss();

    if (data && data.confirmed) {
      this.remove(this.field.id);
    }
  }

  async remove(id: number) {
    try {
      const response = await firstValueFrom(
        this.http.delete<ResponseDto<boolean>>(
          `${environment.baseURL}/api/DeleteField/${id}`
        )
      );
      this.back();
      this.showToast("Successfully removed field.", "success");
    } catch (ex) {
      this.showToast("Something went wrong.", "danger");
    }
  }

  private async showToast(message: string, color: string) {
    const toast = await this.toastController.create({
      message,
      duration: 5000,
      color
    });
    await toast.present();
  }
}

