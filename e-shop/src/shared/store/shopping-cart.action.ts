import { createAction, props } from "@ngrx/store";
import { QueryCartResponse } from "../api client/service-proxies";

export const CallApiGetShoppingCartInfo = createAction('[ShoppingCartItemCount] Call API Get ShoppingCart Info');
export const UpdateShoppingCartItemCountStore = createAction('[ShoppingCartItemCount] Update ShoppingCart Info Store', props<{ payload: QueryCartResponse }>());
