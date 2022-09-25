import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  menuItemArray: Array<menuItem> = [];
  @Input() selectCategory: string = "";

  constructor() { }

  ngOnInit(): void {
    let menuItem1 = new menuItem();
    menuItem1.itemName = '金牌專賣店';
    menuItem1.itemCategory = '金牌';
    menuItem1.itemIsActive = false;
    this.menuItemArray.push(menuItem1);

    let menuItem2 = new menuItem();
    menuItem2.itemName = '禮品區';
    menuItem2.itemCategory = '禮品';
    menuItem2.itemIsActive = false;
    this.menuItemArray.push(menuItem2);

    let menuItem3 = new menuItem();
    menuItem3.itemName = '寵物用品';
    menuItem3.itemCategory = '寵物用品';
    menuItem3.itemIsActive = false;
    this.menuItemArray.push(menuItem3);

    this.menuItemArray.forEach(item => {
      if (item.itemCategory == this.selectCategory) {
        item.itemIsActive = true;
      }
    });
  }

  activeItem(itemIndex: number): void {
    this.menuItemArray.forEach(item => {
      item.itemIsActive = false;
    });
    this.menuItemArray[itemIndex].itemIsActive = true;
  }
}

class menuItem {
  itemName?: string;
  itemCategory?: string;
  itemIsActive?: boolean;
}
