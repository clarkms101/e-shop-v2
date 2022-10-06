import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  currentMenuItem: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  selectMenuItem(MenuItemName: string): void {
    this.currentMenuItem = MenuItemName;
  }
}
