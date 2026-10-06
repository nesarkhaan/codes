namespace ShoppingCentre;

public class Product
{
    private string Category;
    private string BrandName;
    private double Price;
    private int Aisle;

    public Product(string category, string brand, double price, int aisle)
    {
        Category = category;
        BrandName = brand;
        Price = price;
        Aisle = aisle;
    }

    public string GetCategory() { return Category; }
    public string GetBrand() { return BrandName; }
    public double GetPrice() { return Price; }
    public int GetAisle() { return Aisle; }
}
