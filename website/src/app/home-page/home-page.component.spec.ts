import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomePage } from './home-page.component';

describe('HomePageComponent', () => {
	let component: HomePage;
	let fixture: ComponentFixture<HomePage>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [HomePage]
		})
			.compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(HomePage);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
