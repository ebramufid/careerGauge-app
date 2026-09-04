import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerDetails } from './career-details';

describe('CareerDetails', () => {
  let component: CareerDetails;
  let fixture: ComponentFixture<CareerDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CareerDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(CareerDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
