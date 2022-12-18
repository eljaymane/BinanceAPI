using BinanceBOT.Core.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT.Core.Model.WalletContext
{
    public class UserAsset : AbstractEntity
    {
        public string Asset { get; set; }
        public double FreeQuantity { get; set; }
        public double LockedQuantity { get; set; }
        public double FreezeQuantity { get; set; }
        public double WithdrawingQuantity { get; set; }
        public double IpoableQuantity { get; set; }

        public UserAsset(string asset, string lockedQuantity, string freeQuantity, string freezeQuantity, string withdrawingQuantity, string ipoableQuantity)
        {
            Asset = asset;
            FreeQuantity = double.Parse(freeQuantity.Replace(".", ","));
            LockedQuantity = double.Parse(lockedQuantity.Replace(".", ","));
            FreezeQuantity = double.Parse(freezeQuantity.Replace(".", ","));
            WithdrawingQuantity = double.Parse(withdrawingQuantity.Replace(".", ","));
            IpoableQuantity = double.Parse(ipoableQuantity.Replace(".", ","));
        }

    }
}
