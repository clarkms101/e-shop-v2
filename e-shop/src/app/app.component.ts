import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { CallApiGetShoppingCarInfo } from 'src/shared/store/shopping-car.action';
import { ShoppingCar_RootState } from 'src/shared/store/shopping-car.reducer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private _store: Store<{ rootState: ShoppingCar_RootState }>
  ) { }

  title = 'e-shop';

  ngOnInit(): void {
    this._store.dispatch(CallApiGetShoppingCarInfo());
  }
}
