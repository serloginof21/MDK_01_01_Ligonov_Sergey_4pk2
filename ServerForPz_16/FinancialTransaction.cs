using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerForPz_16
{
    public class FinancialTransaction
    {
        private string transactionType;
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType
        {
            get
            {
                return transactionType;
            }
            set
            {
                transactionType = value?.Split(':').Last().Trim();
            }
        }
    }
}
