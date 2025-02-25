import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  template: `
    <h4>Welcome to the <strong>Order Management</strong> site!</h4>
    <div class="clr-row">
      <div class="clr-col-lg-4 clr-col-12">
        <div class="card">
          <div class="card-block">
            <div class="card-title">Products</div>
            <p class="card-text">
              Click the link below to view the list of products. From there you can add, remove and edit products.
            </p>
          </div>
          <div class="card-footer">
            <a routerLink="/products" class="btn btn-sm btn-link">View Products</a>
          </div>
        </div>
      </div>
      <div class="clr-col-lg-4 clr-col-12">
        <div class="card">
          <div class="card-block">
            <div class="card-title">Orders</div>
            <p class="card-text">
              Click the link below to view the list of orders. From there you can add, remove and edit orders.
            </p>
          </div>
          <div class="card-footer">
            <a routerLink="/orders" class="btn btn-sm btn-link">View Orders</a>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: ``
})
export class HomeComponent {

}
