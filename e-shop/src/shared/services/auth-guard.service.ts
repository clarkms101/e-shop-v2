import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { Client, LoginCheckRequest } from '../api client/service-proxies';
import { JwtHelper } from '../helpers/JwtHelper';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(
    private _apiClient: Client,
    private _router: Router
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    let apiAccessKey = JwtHelper.parseJwt().JwtKeyApiAccessKey;
    let request = new LoginCheckRequest();
    request.apiAccessKey = apiAccessKey;

    return this._apiClient.loginCheck(request).pipe(map(response => {
      // console.log(`login check result ${response.success}`);
      // console.log(`login check result ${response.message}`);

      if (response.success) {
        return true;
      } else {
        this._router.navigate(['admin/login']);
        return false;
      }
    }));
  }
}
