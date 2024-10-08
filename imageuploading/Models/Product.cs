using System;
using System.Collections.Generic;

namespace imageuploading.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public string ImagePath { get; set; } = null!;
}
