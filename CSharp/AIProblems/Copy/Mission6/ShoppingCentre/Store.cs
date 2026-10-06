using System;
using System.Collections.Generic;

namespace ShoppingCentre;

public class Store
{
    // The Key is a string ("Milk"), the Value is a List of Product objects!
    private Dictionary<string, List<Product>> Inventory;
    private List<Product> Trolley;

    public Store()
    {
        Inventory = new Dictionary<string, List<Product>>();
        Trolley = new List<Product>();
        StockShelves();
    }

    private void StockShelves()
    {
        // Setting up the Milk Aisle
        Inventory.Add("Milk", new List<Product>());
        Inventory["Milk"].Add(new Product("Milk", "Dairy Farmers", 3.50, 4));
        Inventory["Milk"].Add(new Product("Milk", "Pura", 3.20, 4));
        
        // Setting up the Bread Aisle
        Inventory.Add("Bread", new List<Product>());
        Inventory["Bread"].Add(new Product("Bread", "Tip Top", 4.00, 2));
        Inventory["Bread"].Add(new Product("Bread", "Wonder White", 4.50, 2));
    }

    public bool AddToCart(string category, string brand)
    {
        // TODO: Check if the category exists in the Inventory dictionary.
        // TODO: If it does, loop through that list to find the matching brand.
        // TODO: If found, add it to the Trolley and return true.
        // TODO: If not found (or category doesn't exist), return false.
        return false;
    }

    public double CalculateTotal()
    {
        // TODO: Loop through the Trolley, add up all the prices, and return the sum.
        return 0.0;
    }

    public string Checkout(bool attemptSteal)
    {
        double total = CalculateTotal();
        
        // TODO: Write the logic for paying vs stealing.
        // Hint: Don't forget to clear the trolley at the end of the transaction! ( Trolley.Clear(); )
        
        return "Transaction Pending...";
    }
}
