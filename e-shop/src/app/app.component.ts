import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { CallApiGetShoppingCartInfo } from 'src/shared/store/shopping-cart.action';
import { ShoppingCart_RootState } from 'src/shared/store/shopping-cart.reducer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private _store: Store<{ rootState: ShoppingCart_RootState }>
  ) { }

  title = 'e-shop';

  ngOnInit(): void {
    this._store.dispatch(CallApiGetShoppingCartInfo());
  }
}
