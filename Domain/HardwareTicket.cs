﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.BL.Domain
{
    [Serializable]
    public class HardwareTicket : Ticket
  {
    [RegularExpression("^(PC-)[0-9]+")]
    public string DeviceName { get; set; }
  }
}
