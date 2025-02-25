import { Injectable } from '@angular/core';
import { Product } from './product';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  
  constructor(private httpClient: HttpClient) {
  }
  
  fetchProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${environment.apiUrl}/products`);
  }

  saveProduct(product: Product): Observable<Product> {
    if(!product.id) {
      return this.httpClient.post(`${environment.apiUrl}/products`, product) as Observable<Product>;
    }

    return this.httpClient.put(`${environment.apiUrl}/products`, product) as Observable<Product>;
  }

  deleteProduct(product: Product): Observable<void> {
    return this.httpClient.delete<void>(`${environment.apiUrl}/products/${product.id}`);
  }
}
