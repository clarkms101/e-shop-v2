import { Injectable } from "@angular/core"
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Client, QueryCartRequest } from 'src/shared/api client/service-proxies';
import { switchMap, catchError, map, mergeMap, tap, concatMap, exhaustMap } from 'rxjs/operators';
import { CallApiGetShoppingCartInfo, UpdateShoppingCartItemCountStore as UpdateShoppingCarInfoStore } from "./shopping-cart.action";

@Injectable()
export class ShoppingCart_RootEffects {
  constructor(private actions$: Actions, private _client: Client) { }

  CallApiGetShoppingCartItemCount$ = createEffect(
    () => this.actions$.pipe(
      ofType(CallApiGetShoppingCartInfo),
      tap(() => { console.log('get shopping cart item info start') }),
      mergeMap(() => {
        console.log('get shopping cart item info running');
        let request = new QueryCartRequest();
        return this._client.cartGET(request).pipe(
          map(response => UpdateShoppingCarInfoStore({ payload: response })),
          tap(() => { console.log('get shopping cart item info end') })
        )
      })
    )
  );
}
