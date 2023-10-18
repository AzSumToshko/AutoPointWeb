using System;
using System.Collections.Generic;

namespace AutoPointAPI.Data;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double Price { get; set; }

    public string? TypeOfProduct { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }
}
