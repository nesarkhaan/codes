# Mission 6: The Westfield Shakedown (Dictionaries & State)

**The Goal:** Build the backend for a supermarket. You must manage a complex inventory using a Dictionary, allow a user to add specific brands to their trolley, and handle a risky checkout process.

**Your Tasks:**
1. Review the `Product` class.
2. Complete the `Store` class methods:
   - `StockShelves()`: (Already completed) Populates the Dictionary.
   - `AddToCart(string category, string brand)`: Look up the category in the dictionary. If it exists, search that list for the specific brand. If found, add it to the `Trolley` list and return `true`. Otherwise, return `false`.
   - `CalculateTotal()`: Loop through the `Trolley` and return the total price of all items.
   - `Checkout(bool attemptSteal)`: 
     - If `attemptSteal` is false, empty the trolley and return: "Thank you for paying [Total]."
     - If `attemptSteal` is true, roll a random number from 1 to 10.
     - If the roll is 6 or higher, empty the trolley and return: "SECURITY! YOU ARE UNDER ARREST!"
     - If the roll is 5 or lower, empty the trolley and return: "You slipped out the back with [Total] worth of goods."
