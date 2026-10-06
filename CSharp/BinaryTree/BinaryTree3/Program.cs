namespace BinaryTree3;

class TreeNode
{


    public int Data { get; }
    public int Left { get; private set; }
    public int Right { get; private set; }

    public TreeNode(int value)
    {

        Data = value;

    }

    public TreeNode InsertRight(int valueRight)
    {

        TreeNode Right = new TreeNode(valueRight);
        return Right;
    }

    public TreeNode InsertLeft(int valueLeft)
    {

        TreeNode Left = new TreeNode(valueLeft);
        return Left;
    }


    public TreeNode()

}


class Program
{
    static void Main(string[] args)
    {
        int[] ArrayOfNumbers = new int[] { 1, 2, 4, 5, 3, 6, 7, 8, 9 }
    }
}
