namespace BinaryTree2;

public class BinaryTree
{

    public int Value { get; }

    public BinaryTree? Left { get; private set; }
    public BinaryTree? Right { get; private set; }

    public BinaryTree(int value)
    {

        Value = value;
    }

    public BinaryTree InsertLeft(int leftValue)
    {
        Left = new BinaryTree(leftValue);
        return Left;
    }

    public BinaryTree InsertRight(int rightValue)
    {

        Right = new BinaryTree(rightValue);

        return Right;

    }




}

class Program
{
    public static void PreOrderTraversal(BinaryTree? leftright)
    {
        if (leftright == null)
        {
            return;
        }

        Console.WriteLine(string.Join(" ", leftright.Value));
        //if (leftright.Left != null || leftright.Right != null)
        //{
        //   Console.Write(string.Join(" ", " is the parent of Left child " + leftright.Left.Value + " and right child " + leftright.Right.Value));
        //  Console.WriteLine("\n");
        //}
        PreOrderTraversal(leftright.Left);
        PreOrderTraversal(leftright.Right);




    }

    public static void InOrderTraversal(BinaryTree? leftright)
    {
        if (leftright == null)
        {

            return;

        }

        InOrderTraversal(leftright.Left);
        Console.WriteLine(" " + leftright.Value);
        InOrderTraversal(leftright.Right);


    }

    public static void PostOrderTraversal(BinaryTree? leftright)
    {
        if (leftright == null)
        {

            return;

        }

        InOrderTraversal(leftright.Left);
        InOrderTraversal(leftright.Right);
        Console.WriteLine(" " + leftright.Value);


    }


    static void Print(int num, string side)
    {
        Console.WriteLine(string.Join(" ", num, side));


    }
    static void Main(string[] args)
    {
        BinaryTree smalltree = new BinaryTree(1000);
        Print(smalltree.Value, "root");

        var baby1 = smalltree.InsertLeft(950);
        var baby2 = smalltree.InsertRight(500);

        var baby3 = baby1.InsertLeft(900);
        var baby4 = baby1.InsertRight(850);

        var baby5 = baby2.InsertLeft(600);
        var baby6 = baby2.InsertRight(550);

        Console.WriteLine("PreOrderTraversal");
        PreOrderTraversal(smalltree);


        Console.WriteLine("InOrderTraversal");

        InOrderTraversal(smalltree);

        Console.WriteLine("PostOrderTraversal");

        PostOrderTraversal(smalltree);
    }
}


//                  1000
//                /      \
//               /        \
//             950         500
//            /    \       /   \
//           /      \     /     \
//          900     850  600     550
//
//PreOrderTraversal
//1000
//950
//900
//850
//500
//600
// 550
//
//
//
// In order Traversal
//  900
//  950
//  850
//  1000
//  600
//  500
//  550
//

//  PostOrderTraversal
//  900
// 950
// 850
// 600
// 500
// 550
// 1000



