import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [],
  template: `<h1>{{ title() }}</h1>`
})
export class App {
  protected readonly title = signal('frontend');
}
