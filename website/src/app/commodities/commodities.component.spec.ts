import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CommoditiesComponent } from './commodities.component';

describe('CommoditiesComponent', () => {
  let component: CommoditiesComponent;
  let fixture: ComponentFixture<CommoditiesComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CommoditiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommoditiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
