using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Frontend
{
    /*
    public class MauiProgramViewModel : BindableObject
    {
        private DataService _dataService;

        public MauiProgramViewModel(DataService dataService)
        {
            _dataService = dataService;
            Items = new ObservableCollection<Item>();
        }

        private IEnumerable<Item> _items;

        public IEnumerable<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }


        public async Task LoadItemsAsync()
        {
            Items.Clear();
            var items = await _dataService.GetItemsAsync();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async Task PostItemAsync()
        {
            Item newItem = new Item
            {
                Name = "New Item",
                Price = 10.0m // Set whatever values you want for the new item
            };

            var response = await _dataService.PostItemAsync(newItem);
            if (response != null)
            {
                // Handle success response
                Console.WriteLine("Item posted successfully.");
            }
            else
            {
                // Handle error
                Console.WriteLine("Failed to post item.");
            }
        }
    }
    */
}
