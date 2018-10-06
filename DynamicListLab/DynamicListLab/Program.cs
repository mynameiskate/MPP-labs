using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicListLab
{
    class Program
    {
        private delegate void TestOperation();
        private static TestItem addItem = new TestItem()
        {
            ItemId = 10,
            Name = "Sydney",
            Location = "Australia"
        };

        private static TestItem removeItem = new TestItem()
        {
            ItemId = 1,
            Name = "NYC",
            Location = "USA"
        };

        private static DynamicList<TestItem> testItems;

        private static List<TestOperation> testOperations = new List<TestOperation>()
        {
            () => PrintOperation(() => CreateList(), "Creating new list.."),
            () => PrintOperation(() => testItems.Add(addItem), "Adding new item.."),
            () => PrintOperation(() => testItems.Remove(removeItem), "Removing item.."),
            () => PrintOperation(() => testItems.RemoveAt(2), "Removing item at index 2.."),
            () => PrintOperation(() => testItems.Clear(), "Clearing list..")
        };

        static void Main(string[] args)
        {
            foreach(var operation in testOperations)
            {
                operation();
            }
            Console.ReadLine();
        }

        private static void PrintOperation(TestOperation operation, string msg)
        {
            Console.WriteLine(msg);
            try
            {
                operation();
                PrintList();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception thrown: {ex.Message} at operation {msg}");
            }
            
        }

        private static void CreateList()
        {
            TestItem[] itemArray = new TestItem[]
            {
                new TestItem()
                {
                    ItemId = 0,
                    Name = "Minsk",
                    Location = "Belarus"
                },
                removeItem,
                new TestItem
                {
                    ItemId = 2,
                    Name = "Reykjavík",
                    Location = "Iceland"
                },
                new TestItem
                {
                    ItemId = 3,
                    Name = "Copenhagen",
                    Location = "Denmark"
                }
            };

            testItems = new DynamicList<TestItem>(itemArray);

        }

        private static void PrintList()
        {
            Console.WriteLine("Current list state:");

            if (testItems.Count == 0)
            {
                Console.WriteLine("Array is empty.");
            }
            else
            {
                foreach (var item in testItems)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
