import { Injectable } from "@angular/core"
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Client, QueryCartRequest } from 'src/shared/api client/service-proxies';
import { switchMap, catchError, map, mergeMap, tap, concatMap, exhaustMap } from 'rxjs/operators';
import { CallApiGetShoppingCarItemCount, UpdateShoppingCarItemCountStore } from "./shopping-car.action";

@Injectable()
export class ShoppingCar_RootEffects {
  constructor(private actions$: Actions, private _client: Client) { }

  CallApiGetShoppingCarItemCount$ = createEffect(
    () => this.actions$.pipe(
      ofType(CallApiGetShoppingCarItemCount),
      tap(() => { console.log('get shopping car item info start') }),
      mergeMap(() => {
        console.log('get shopping car item info running');
        let request = new QueryCartRequest();
        return this._client.cartGET(request).pipe(
          map(response => UpdateShoppingCarItemCountStore({ payload: response.carts?.length as number })),
          tap(() => { console.log('get shopping car item info end') })
        )
      })
    )
  );
}
