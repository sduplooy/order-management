import { CommonModule } from '@angular/common';
import { Component, model } from '@angular/core';
import { ClarityModule, ClrDatagridModule, ClrFormsModule, ClrIconModule, ClrModalModule } from '@clr/angular';
import { FormControl, FormGroup, NgModelGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductsService } from './products.service';
import { catchError, Observable, of } from 'rxjs';
import { Product } from './product';

@Component({
  selector: 'app-products',
  imports: [CommonModule, ClrDatagridModule, ClrIconModule, ClrModalModule, ClrFormsModule, ReactiveFormsModule, ClarityModule],
  
  template: `
    <clr-alert [clrAlertType]="'danger'" *ngIf="error">
      <clr-alert-item>
        <span class="alert-text">An unexpected error ocurred: {{error}}</span>
      </clr-alert-item>
    </clr-alert>

    <button class="btn btn-primary" (click)="onCreate()">New product...</button>
    
    <clr-modal [(clrModalOpen)]="modalOpen">
      <h3 class="modal-title">Product</h3>
      <div class="modal-body">
        <form clrForm [formGroup]="productForm">
          <clr-input-container>
            <label>Name</label>
            <input clrInput type="text" formControlName="name" />
            <clr-control-helper>What would you like to call this product?</clr-control-helper>
            <clr-control-error>A product name is required.</clr-control-error>
          </clr-input-container>
          <clr-number-input-container>
            <label>Price</label>
            <input clrNumberInput type="number" formControlName="price" placeholder="0.00" min="0" required />
            <clr-control-helper>What is the price of this product?</clr-control-helper>
            <clr-control-error>A product price is required that is greater than or equal to 0.</clr-control-error>
          </clr-number-input-container>
        </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-outline" (click)="modalOpen = false">Cancel</button>
        <button type="button" class="btn btn-primary" (click)="saveProduct()">Ok</button>
      </div>
    </clr-modal>

    <clr-datagrid>
      <clr-dg-column>Product ID</clr-dg-column>
      <clr-dg-column>Name</clr-dg-column>
      <clr-dg-column>Price</clr-dg-column>

      <clr-dg-row *clrDgItems="let product of (products$ | async) || []" [clrDgItem]="product">
        <clr-dg-action-overflow>
          <button class="action-item" (click)="onEdit(product)">Edit</button>
          <button class="action-item" (click)="onDelete(product)">Delete</button>
        </clr-dg-action-overflow>
        <clr-dg-cell>{{ product.id }}</clr-dg-cell>
        <clr-dg-cell>{{ product.name }}</clr-dg-cell>
        <clr-dg-cell>{{ product.price | number: '.2' }}</clr-dg-cell>
      </clr-dg-row>

      <clr-dg-footer>{{ (products$ | async)?.length || 0 }} products</clr-dg-footer>
    </clr-datagrid>
  `,
  styles: ``
})
export class ProductsComponent {

  product: Product | undefined;
  products$!: Observable<Product[]>;

  modalOpen = false;
  error: string | null = null;

  productForm = new FormGroup({
    id: new FormControl(''),
    name: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    price: new FormControl('', [Validators.required, Validators.min(0)]),
  });

  constructor(private productsService: ProductsService) {
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  onCreate() {
    this.productForm.reset();
    this.modalOpen = true;
  }

  onEdit(product: Product) {
    this.productForm.setValue({
      id: product.id,
      name: product.name,
      price: (product.price ?? 0).toString(),
    });

    this.modalOpen = true;
  }

  onDelete(product: Product) {
    this.product = product;
    this.productsService.deleteProduct(product).pipe(
      catchError(err => {
        this.handleError(err);
        return of([]);
      })).subscribe(() => {
        this.loadProducts();
      });
  }

  loadProducts() {
    this.products$ = this.productsService.fetchProducts().pipe(
      catchError(err => {
        this.handleError(err);
        return of([]);
      })
    );
  }

  saveProduct() {
    if (this.productForm.valid) {
      const product: Product = {
        id: this.productForm.value.id || null,
        name: this.productForm.value.name!,
        price: parseFloat(this.productForm.value.price!)
      };

      this.productsService.saveProduct(product).pipe(
        catchError(err => {
          this.handleError(err);
          return of([]);
        })).subscribe(() => {
          this.loadProducts();
        });

      this.modalOpen = false;
    }
  }

  handleError(error: any) {
    this.error = error.message;
  }
}
