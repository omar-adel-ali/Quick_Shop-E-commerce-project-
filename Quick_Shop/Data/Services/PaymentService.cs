using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private PayPalHttpClient GetClient()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var clientSecret = _configuration["PayPal:ClientSecret"];
            var mode = _configuration["PayPal:Mode"];

            PayPalEnvironment environment = mode == "sandbox"
                ? new SandboxEnvironment(clientId, clientSecret)
                : new LiveEnvironment(clientId, clientSecret);

            return new PayPalHttpClient(environment);
        }

        public async Task<string> CreateOrderAsync(decimal amount, string currency = "USD")
        {
            try
            {
                var client = GetClient();
                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE", // بديل Intent في الإصدار الحالي
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                CurrencyCode = currency,
                                Value = amount.ToString("F2")
                            }
                        }
                    },
                    ApplicationContext = new ApplicationContext
                    {
                        ReturnUrl = _configuration["PayPal:ReturnUrl"] ?? "https://yourdomain.com/Orders/CaptureOrder",
                        CancelUrl = _configuration["PayPal:CancelUrl"] ?? "https://yourdomain.com/Orders/Checkout"
                    }
                };

                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(order);

                var response = await client.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
                return result.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create PayPal order: " + ex.Message);
            }
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            try
            {
                var client = GetClient();
                var request = new OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");

                var response = await client.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
                return result.Status == "COMPLETED";
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to capture PayPal order: " + ex.Message);
            }
        }
    }
}