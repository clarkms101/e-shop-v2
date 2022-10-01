import { createReducer, on } from "@ngrx/store";
import { UpdateShoppingCarItemCountStore } from "./shopping-car.action";

export interface ShoppingCar_RootState {
  shoppingCarItemCount: number;
}

export const initialState: ShoppingCar_RootState = {
  shoppingCarItemCount: 0
}

export const shoppingCarItemCountReducer =
  createReducer(
    initialState.shoppingCarItemCount,
    on(UpdateShoppingCarItemCountStore, (state, action) => action.payload),
  );
