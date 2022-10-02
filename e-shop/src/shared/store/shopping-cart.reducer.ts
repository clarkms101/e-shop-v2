import { createReducer, on } from "@ngrx/store";
import { QueryCartResponse } from "../api client/service-proxies";
import { UpdateShoppingCartItemCountStore } from "./shopping-cart.action";

export interface ShoppingCart_RootState {
  shoppingCarInfo: QueryCartResponse;
}

export const initialState: ShoppingCart_RootState = {
  shoppingCarInfo: new QueryCartResponse()
}

export const shoppingCartItemCountReducer =
  createReducer(
    initialState.shoppingCarInfo,
    on(UpdateShoppingCartItemCountStore, (state, action) => action.payload),
  );
