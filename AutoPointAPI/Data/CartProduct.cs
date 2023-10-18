using System;
using System.Collections.Generic;

namespace AutoPointAPI.Data;

public partial class CartProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int ProductQuantity { get; set; }
}
