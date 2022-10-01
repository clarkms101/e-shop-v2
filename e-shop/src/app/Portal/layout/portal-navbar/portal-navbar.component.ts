import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-portal-navbar',
  templateUrl: './portal-navbar.component.html',
  styleUrls: ['./portal-navbar.component.css']
})
export class PortalNavbarComponent implements OnInit {
  shoppingCarItemCount: number = 0

  constructor(
    private store: Store<{
      shoppingCarItemCount: number
    }>
  ) { }

  ngOnInit(): void {
    this.store.select('shoppingCarItemCount').subscribe(data => {
      this.shoppingCarItemCount = data;
    });
  }
}
