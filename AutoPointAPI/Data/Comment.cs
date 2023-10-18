using System;
using System.Collections.Generic;

namespace AutoPointAPI.Data;

public partial class Comment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? FullName { get; set; }

    public string? Message { get; set; }
}
