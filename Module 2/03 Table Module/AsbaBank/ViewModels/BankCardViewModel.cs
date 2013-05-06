using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsbaBank.Presentation.Mvc.ViewModels
{
    public class BankCardViewModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string Status { get; set; }
        public bool Disabled { get; set; }
    }
}