using System;
using System.Collections.Generic;

namespace AutoPointAPI.Data;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? ProductIds { get; set; }

    public string? ProductQuantities { get; set; }

    public int ProductsCount { get; set; }

    public string? FullName { get; set; }

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? AddressOne { get; set; }

    public string? AddressTwo { get; set; }

    public string? City { get; set; }

    public string? Postcode { get; set; }

    public string? Details { get; set; }

    public string? PaymentMethod { get; set; }

    public string? DeliveryType { get; set; }

    public double Total { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string? Status { get; set; }
}
