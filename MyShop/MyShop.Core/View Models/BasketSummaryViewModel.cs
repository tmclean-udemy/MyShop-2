using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.View_Models
{
    public class BasketSummaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        //Create contructors
        //Create an Empty constructer to set default values
        public BasketSummaryViewModel()
        {
        }
        public BasketSummaryViewModel(int basketCount, decimal basketTotal)
        {
        this.BasketCount = basketCount;
        this.BasketTotal = basketTotal;
        }
    }
}
