using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading
{
    using Identifier = String;
    using Inventory = List<Item>;

    class ItemTransaction : ITransaction
    {
        private bool isDone = false;

        public ItemTransaction(Inventory sellerInventory, string[] sellerItemIds)
        {
            SellerInventory = sellerInventory;
            SellerItemIds = sellerItemIds;
        }

        private Inventory SellerInventory { get; set; }
        private Identifier[] SellerItemIds { get; set; }

        public Inventory BuyerInventory { private get; set; }
        public Identifier[] BuyerItemIds { private get; set; }


        public void Dispose()
        {
            SellerInventory = BuyerInventory = null;
            SellerItemIds = BuyerItemIds = null;
        }

        public void Commit()
        {
            var sellersItems = SellerInventory.Where(x =>
            {
                return SellerItemIds.Contains(x.Id);
            }).ToArray();
            var buyersItems = BuyerInventory.Where(x =>
            {
                return BuyerItemIds.Contains(x.Id);
            }).ToArray();
            foreach (var item in sellersItems)
            {
                SellerInventory.Remove(item);
            }
            foreach (var item in buyersItems)
            {
                BuyerInventory.Remove(item);
            }
            BuyerInventory.AddRange(sellersItems);
            SellerInventory.AddRange(buyersItems);
        }

        public void Rollback()
        {
            Dispose();
        }
    }

    class Item
    {
        public Identifier Id;
        public string Name;
        public Item(string name) { Name = Id = name; }
    }

    abstract class Character
    {
        protected Inventory inventory = new Inventory();
    }

    class Seller : Character, ISeller<ItemTransaction>
    {
        public Seller()
        {
            inventory.Add(new Item("Banana"));
        }

        public ItemTransaction BeginTransaction()
        {
            return new ItemTransaction(sellerInventory: inventory, sellerItemIds: new Identifier[] { "Banana" });
        }
    }

    class Buyer : Character, IBuyer<ItemTransaction>
    {
        public Buyer()
        {
            inventory.Add(new Item("Apple"));
            inventory.Add(new Item("Orange"));
        }
        public void Approve(ItemTransaction transaction)
        {
            transaction.BuyerItemIds = new Identifier[] { "Orange" };
            transaction.BuyerInventory = inventory;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var seller = new Seller();
            var buyer = new Buyer();

            // 유저간의 1:1 거래의 경우
            // 협상 테이블과같은 다른 클래스에 서로 아이템을 올려두고
            // 거래 버튼을 누르면 BeginTransaction을 호출한다.

            // Transaction객체 내부를 제외하면 Buyer와 Seller의 Iventory의 참조가 전혀 존재 하지 않는다.

            using (var transaction = seller.BeginTransaction())
            {
                try
                {
                    buyer.Approve(transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }


        }
    }
}
