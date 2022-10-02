import { createAction, props } from "@ngrx/store";
import { QueryCartResponse } from "../api client/service-proxies";

export const CallApiGetShoppingCarInfo = createAction('[ShoppingCarItemCount] Call API Get ShoppingCar Info');
export const UpdateShoppingCarItemCountStore = createAction('[ShoppingCarItemCount] Update ShoppingCar Info Store', props<{ payload: QueryCartResponse }>());
