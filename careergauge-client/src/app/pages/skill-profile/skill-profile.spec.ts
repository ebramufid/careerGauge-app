import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillProfile } from './skill-profile';

describe('SkillProfile', () => {
  let component: SkillProfile;
  let fixture: ComponentFixture<SkillProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkillProfile],
    }).compileComponents();

    fixture = TestBed.createComponent(SkillProfile);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
