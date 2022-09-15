import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalNavbarComponent } from './portal-navbar.component';

describe('PortalNavbarComponent', () => {
  let component: PortalNavbarComponent;
  let fixture: ComponentFixture<PortalNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PortalNavbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PortalNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
