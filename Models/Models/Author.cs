﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int NewsId { get; set; }

    public virtual News News { get; set; }
}