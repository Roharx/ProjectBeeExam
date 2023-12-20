import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { JwtService } from './jwt.service';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor(private cookieService: CookieService, private jwtService: JwtService) {}

  getToken(): string | null {
    return this.cookieService.get(TOKEN_KEY);
  }

  saveToken(token: string): void {
    const tokenData = this.jwtService.decodeToken(token);
    const expirationTime = tokenData ? tokenData['exp'] : 0;

    const expirationDate = new Date(expirationTime * 1000); //exp time to date (later reference as I'll definitely forget)

    // Save the token as an HttpOnly cookie to protect from XSS (Cross Site Scripting) attack (most likely I'll forget about this one as well)
    this.cookieService.set(
      TOKEN_KEY,
      token,
      expirationDate,
      null!,
      null!,
      true,  // HttpOnly flag
      'Strict'
    );
  }

  removeToken(): void {
    this.cookieService.delete(TOKEN_KEY);
  }
}
