import { Component, inject, signal } from '@angular/core';
import { Product } from '../../models/Product';
import { ProductService } from '../../services/product.service';
import { PurchaseService } from '../../services/purchase.service';
import { PaymentService } from '../../services/payment.service';

declare var Razorpay: any;

@Component({
  selector: 'app-purchase',
  imports: [],
  templateUrl: './purchase.html',
  styleUrl: './purchase.css',
})
export class Purchase {

  private productService = inject(ProductService);
  private purchaseService = inject(PurchaseService);
  private paymentService = inject(PaymentService);

  product = signal<Product | null>(null);
  purchaseId = signal<number | null>(null);


  ngOnInit() {
    this.loadProduct();
  }

  private loadProduct(): void {

    this.productService
      .getProduct('PREMIUM_REPORT')
      .subscribe({

        next: (product) => {
          this.product.set(product);
        },

        error: (error) => {
          console.error(error);
        }

      });
  }

  createPurchase(): void {

    const currentProduct = this.product();

    if (!currentProduct) {
      return;
    }

    this.purchaseService
      .createPurchase(
        1,
        currentProduct.itemCode
      )
      .subscribe({

        next: (purchase) => {

          this.purchaseId.set(purchase.id);

          console.log(
            'Purchase created:',
            purchase
          );
        },

        error: (error) => {

          console.error(
            'Unable to create purchase',
            error
          );
        }

      });
  }

  payNow(): void {

    const currentPurchaseId = this.purchaseId();

    if (!currentPurchaseId) {
      return;
    }

    this.paymentService
      .createPayment(currentPurchaseId)
      .subscribe({

        next: (response) => {

          const options = {

            key: response.keyId,

            amount: Math.round(response.amount * 100),

            currency: response.currency,

            name: 'Enterprise Employee System',

            description: 'Premium Employee Report',

            order_id: response.gatewayOrderId,

            handler: (razorpayResponse: any) => {

              const verifyRequest = {

                paymentId: response.paymentId,

                razorpayPaymentId:
                  razorpayResponse.razorpay_payment_id,

                razorpayOrderId:
                  razorpayResponse.razorpay_order_id,

                razorpaySignature:
                  razorpayResponse.razorpay_signature
              };

              this.paymentService
                .verifyPayment(verifyRequest)
                .subscribe({

                  next: (verifyResponse) => {

                    console.log(
                      'Payment verified successfully',
                      verifyResponse
                    );

                    alert('Payment successful and verified.');
                  },

                  error: (error) => {

                    console.error(
                      'Payment verification failed',
                      error
                    );

                    alert(
                      'Payment completed, but verification failed.'
                    );
                  }

                });
            }
          };

          const razorpay =
            new Razorpay(options);

          razorpay.open();
        },

        error: (error) => {

          console.error(
            'Unable to create payment',
            error
          );
        }

      });
  }


}
