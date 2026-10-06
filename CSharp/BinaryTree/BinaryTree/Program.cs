namespace BinaryTree;

public class TreeNode
{

    public int Value;
    public TreeNode Left;
    public TreeNode Right;

    public TreeNode(int value)
    {

        this.Value = value;
        this.Left = null;
        this.Right = null;


    }
    // Tree is created here. value is the root, left and right the nodes.
    public TreeNode(int value, TreeNode left, TreeNode right)
    {
        this.Value = value;
        this.Left = left;
        this.Right = right;
    }



}


class Program
{
    public static List<int> InorderTraversal(TreeNode root)
    {

        List<int> result = new List<int>();
        InorderHelper(root, result);
        return result;

    }

    private static void InorderHelper(TreeNode node, List<int> result)
    {

        if (node == null)
        {

            return;

        }
        InorderHelper(node.Left, result);
        result.Add(node.Value);
        InorderHelper(node.Right, result);

    }
    static void Main(string[] args)
    {
        //this creates the Treenode from the TreeNode class. 
        TreeNode root = new TreeNode(50, null, new TreeNode(14, new TreeNode(390), null));

        Console.WriteLine(string.Join(", ", InorderTraversal(root))); //This takes the value from TreeNode root and creates another list. Within this is InorderHelper which is the function that takes the Treenode and places it into the list we just created inside inordertraversal. It valides data.. with left, value and right data.  
    }
}
