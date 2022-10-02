import { createReducer, on } from "@ngrx/store";
import { QueryCartResponse } from "../api client/service-proxies";
import { UpdateShoppingCarItemCountStore } from "./shopping-car.action";

export interface ShoppingCar_RootState {
  shoppingCarInfo: QueryCartResponse;
}

export const initialState: ShoppingCar_RootState = {
  shoppingCarInfo: new QueryCartResponse()
}

export const shoppingCarItemCountReducer =
  createReducer(
    initialState.shoppingCarInfo,
    on(UpdateShoppingCarItemCountStore, (state, action) => action.payload),
  );
