import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShipListComponent } from './shiplist.component';

describe('ShiplistComponent', () => {
  let component: ShipListComponent;
  let fixture: ComponentFixture<ShipListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ShipListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShipListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
