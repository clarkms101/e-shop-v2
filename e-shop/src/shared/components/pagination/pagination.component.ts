import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  page: number = 0;
  hasPrePage: boolean = false;
  constructor() { }

  ngOnInit(): void {
  }

  getData(page: number): void {

  }
}
