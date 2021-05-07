import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentNewComponent } from './component-new.component';

describe('ComponentNewComponent', () => {
  let component: ComponentNewComponent;
  let fixture: ComponentFixture<ComponentNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComponentNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
