import { Injectable } from '@angular/core';
import { Product } from './product';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ListProductsService {
  constructor(private httpClient: HttpClient) {
  }
  
  fetchProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${environment.apiUrl}/products`);
  }
}
