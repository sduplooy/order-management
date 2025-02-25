import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, RouterLink, RouterLinkActive],
    standalone: true,
    template: `
    <div class="main-container">
      <header class="header-6">
        <div class="branding">
          <a routerLink="home">
            <span class="title">{{this.title}}</span>
          </a>
        </div>
      </header>
      <nav class="subnav">
        <ul class="nav">
        <li class="nav-item">
            <a class="nav-link" routerLink="home" routerLinkActive="active">Home</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" routerLink="products" routerLinkActive="active">Products</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" routerLink="orders" routerLinkActive="active">Orders</a>
          </li>
        </ul>
      </nav>
      <div class="content-container">
        <div class="content-area">
          <router-outlet></router-outlet>
        </div>
      </div>
    </div>
  `,
    styles: []
})
export class AppComponent {
  
  title = 'Order Management';

}
