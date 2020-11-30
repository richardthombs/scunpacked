import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CargoComponent } from './cargo.component';

describe('CargoComponent', () => {
  let component: CargoComponent;
  let fixture: ComponentFixture<CargoComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CargoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
