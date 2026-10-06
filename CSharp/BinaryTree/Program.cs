namespace BinaryTree;


public class Node
{

    public int Data; 
    public Node Left;
    public Node Right; 

    public Node(int data) 
    {
        Data = data;
        Left = null;
        Right = null;
    }

}

public class BinaryTree
{
    public Node Root {get; set;}

    public BinaryTree(int data){

        this.Root = new Node(data);

    }

    public void InsertNode(int data) {

        Root = InsertRecursive(Root, data);

    }

    public Node InsertRecursive(Node current, int data) {


        //base case

        if(current == null) {

            return new Node(data);
        }

        if (data <= current.Data)
            {
                current.Left = InsertRecursive(current.Left, data);
            }


        else 
            {
                current.Right = InsertRecursive(current.Right, data);
            }

            // Return the unmodified node pointer back up the call stack
            return current;

    }


}

class Program
{
    static void Main(string[] args)
    {
        //root
        BinaryTree tree = new BinaryTree(100);
        tree.InsertNode(99);
        tree.InsertNode(98);
        tree.InsertNode(200);
        tree.InsertNode(198);

        Console.WriteLine(tree.Root.Left.Data.ToString());
        Console.WriteLine(tree.Root.Right.Data.ToString());
    }
}
