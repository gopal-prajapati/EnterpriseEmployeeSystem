import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreatePaymentResponse {
  paymentId: number;
  purchaseId: number;
  gatewayOrderId: string;
  amount: number;
  currency: string;
  keyId: string;
}

export interface VerifyPaymentRequest {
  paymentId: number;
  razorpayPaymentId: string;
  razorpayOrderId: string;
  razorpaySignature: string;
}

@Service()
export class PaymentService {

  private http = inject(HttpClient);

  private apiUrl = '/api/payments';

  createPayment(purchaseId: number): Observable<CreatePaymentResponse> {
    return this.http.post<CreatePaymentResponse>(
      this.apiUrl,
      {
        purchaseId: purchaseId
      }
    );
  }

  verifyPayment(request: VerifyPaymentRequest) {
  return this.http.post(
    `${this.apiUrl}/verify`,
    request
  );
}
}