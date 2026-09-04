import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/Product';

@Service()
export class ProductService {

  private http = inject(HttpClient);

  private apiUrl = '/api/products';

  getProduct(itemCode: string): Observable<Product> {
    return this.http.get<Product>(
      `${this.apiUrl}/${itemCode}`
    );
  }
}