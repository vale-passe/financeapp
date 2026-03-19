import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [NgOptimizedImage],
  template: `
    <div class="card bg-base-100 w-1/2 mx-auto p-10 mt-10 shadow-xl">
      <h1 class="text-3xl text-red-500">{{ title() }}</h1>
      <ul class="list">
        @for (user of users(); track user.id) {
          <li class="list-row items-center">
            <img ngSrc="/images/male_avatar.svg" alt="user avatar" width="48" height="48" />
            <div>{{ user.firstname }} {{ user.lastname }} ({{ user.email }})</div>
          </li>
        }
      </ul>
    </div>
  `,
})
export class App implements OnInit {
  private httpClient = inject(HttpClient);
  protected readonly title = signal('frontend');
  protected users = signal<any>([]);

  ngOnInit(): void {
    this.httpClient.get('https://localhost:5001/api/users').subscribe({
      next: (response) => this.users.set(response),
      error: (error) => console.log(error),
      complete: () => console.log('Successfully completed'),
    });
  }
}
