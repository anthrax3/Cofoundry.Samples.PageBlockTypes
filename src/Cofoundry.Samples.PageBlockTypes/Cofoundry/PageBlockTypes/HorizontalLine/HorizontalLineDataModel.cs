﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Cofoundry.Domain;

namespace Cofoundry.Web
{
    /// <summary>
    /// A very simple block type that adds a variable width horizontal line (wrapped hr tag).
    /// </summary>
    public class HorizontalLineDataModel : IPageBlockTypeDataModel, IPageBlockTypeDisplayModel
    {
        [Display(Name = "Percentage width", Description = "Leave blank to use the default width.")]
        [Range(1, 100)]
        public int? PercentageWidth { get; set; }
    }
}