using Microsoft.AspNetCore.Mvc;
using Payment.Core.Models;
using Payment.Service.Interfaces;

namespace Payment.API.Controllers;

public class PaymentController: ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    public async Task<IActionResult> CreatePayment([FromBody] PaymentEntity payment)
    {
        await _paymentService.ProcessPaymentAsync(payment);
        return Ok(new
        {
            Message = "Payment successfully created"
        });
    }
}