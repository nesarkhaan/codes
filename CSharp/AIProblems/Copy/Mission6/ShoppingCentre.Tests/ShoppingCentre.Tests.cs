using Xunit;
using ShoppingCentre;

namespace ShoppingCentre.Tests;

public class StoreTests
{
    [Fact]
    public void AddToCart_FindsValidItem_ReturnsTrue()
    {
        Store westfield = new Store();
        
        bool result = westfield.AddToCart("Milk", "Pura");
        
        Assert.True(result);
        Assert.Equal(3.20, westfield.CalculateTotal());
    }

    [Fact]
    public void AddToCart_InvalidItem_ReturnsFalse()
    {
        Store westfield = new Store();
        
        bool wrongBrand = westfield.AddToCart("Milk", "Rolex");
        bool wrongCategory = westfield.AddToCart("Electronics", "Sony");
        
        Assert.False(wrongBrand);
        Assert.False(wrongCategory);
        Assert.Equal(0, westfield.CalculateTotal());
    }

    [Fact]
    public void CalculateTotal_SumsMultipleItems()
    {
        Store westfield = new Store();
        
        westfield.AddToCart("Milk", "Dairy Farmers"); // $3.50
        westfield.AddToCart("Bread", "Tip Top");      // $4.00
        
        Assert.Equal(7.50, westfield.CalculateTotal());
    }

    [Fact]
    public void Checkout_Paying_ClearsTrolleyAndReturnsMessage()
    {
        Store westfield = new Store();
        westfield.AddToCart("Bread", "Wonder White"); // $4.50
        
        string receipt = westfield.Checkout(false);
        
        Assert.Contains("Thank you for paying", receipt);
        Assert.Equal(0, westfield.CalculateTotal()); // Trolley should be empty!
    }
}
