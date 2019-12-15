import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShiplistPage } from './shiplist.component';

describe('ShiplistComponent', () => {
	let component: ShiplistPage;
	let fixture: ComponentFixture<ShiplistPage>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [ShiplistPage]
		})
			.compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(ShiplistPage);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
