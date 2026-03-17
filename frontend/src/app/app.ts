import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [],
  template: `<h1>{{ title() }}</h1>`
})
export class App implements OnInit {
  private httpClient = inject(HttpClient);
  protected readonly title = signal('frontend');

  ngOnInit(): void {
    this.httpClient.get('https://localhost:5001/api/users')
      .subscribe({
        next: response => console.log(response),
        error: error => console.log(error),
        complete: () => console.log('Successfully completed')
      });
  }
}
