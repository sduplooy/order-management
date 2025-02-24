import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ClrDatagridModule, ClrIconModule } from '@clr/angular';
import { ListProductsService } from './list-products.service';
import { Observable } from 'rxjs';
import { Product } from './product';

@Component({
  selector: 'app-list-products',
  standalone: true,
  imports: [CommonModule, ClrDatagridModule, ClrIconModule],
  
  template: `
    <clr-datagrid>
      <clr-dg-column>Product ID</clr-dg-column>
      <clr-dg-column>Name</clr-dg-column>
      <clr-dg-column>Price</clr-dg-column>

      <clr-dg-row *ngFor="let product of products$ | async">
        <clr-dg-cell>{{ product.id }}</clr-dg-cell>
        <clr-dg-cell>{{ product.name }}</clr-dg-cell>
        <clr-dg-cell>{{ product.price | number }}</clr-dg-cell>
      </clr-dg-row>

      <clr-dg-footer>{{ (products$ | async)?.length || 0 }} products</clr-dg-footer>
    </clr-datagrid>
  `,
  styles: ``
})
export class ListProductsComponent {

  products$!: Observable<Product[]>;

  constructor(private listProductsService: ListProductsService) {
  }

  ngOnInit(): void {
    this.products$ = this.listProductsService.fetchProducts();  
  }

  products = [
    {
      id: 'id-1',
      name: 'Product 1',
      price: 100
    },
    {
      id: 'id-2',
      name: 'Product 2',
      price: 102
    }
  ]
}
