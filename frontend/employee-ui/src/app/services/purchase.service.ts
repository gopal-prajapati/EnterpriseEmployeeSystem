import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreatePurchaseResponse {
  id: number;
  employeeId: number;
  itemCode: string;
  description: string;
  amount: number;
  currency: string;
  status: number;
}

@Service()
export class PurchaseService {

  private http = inject(HttpClient);

  private apiUrl = '/api/purchases';

  createPurchase(
    employeeId: number,
    itemCode: string
  ): Observable<CreatePurchaseResponse> {

    return this.http.post<CreatePurchaseResponse>(
      this.apiUrl,
      {
        employeeId: employeeId,
        itemCode: itemCode
      }
    );
  }
}