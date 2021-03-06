﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FancyTodoList.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AccessoryType
	{
		Headwear,
        Glasses,
        Mask
	}

    public class Accessory
    {
		public AccessoryType Type { get; set; }
		public double Confidence { get; set; }
    }
}
