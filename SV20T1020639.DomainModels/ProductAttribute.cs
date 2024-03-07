﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020639.DomainModels
{
    public class ProductAttribute
    {
        public long AttributeId { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = "";
        public string AtttributeValue { get; set; } = "";
        public int DisplayOrder {  get; set; }

    }
}