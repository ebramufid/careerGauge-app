import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerComparison } from './career-comparison';

describe('CareerComparison', () => {
  let component: CareerComparison;
  let fixture: ComponentFixture<CareerComparison>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CareerComparison],
    }).compileComponents();

    fixture = TestBed.createComponent(CareerComparison);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
