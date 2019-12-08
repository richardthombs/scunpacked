import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PowerplantComponent } from './powerplant.component';

describe('PowerplantComponent', () => {
  let component: PowerplantComponent;
  let fixture: ComponentFixture<PowerplantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PowerplantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PowerplantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
