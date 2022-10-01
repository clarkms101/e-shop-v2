import { createAction, props } from "@ngrx/store";

export const CallApiGetShoppingCarItemCount = createAction('[ShoppingCarItemCount] Call API Get ShoppingCar Item Count');
export const UpdateShoppingCarItemCountStore = createAction('[ShoppingCarItemCount] Update ShoppingCar Item Count Store', props<{ payload: number }>());
