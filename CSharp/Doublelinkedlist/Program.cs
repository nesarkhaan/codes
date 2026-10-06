namespace Doublelinkedlist;


class Node {

    public string Data;

    public Node Next;

    public Node Previous;


    public Node(string data){

        Data = data;
        Previous = Next = null;

    }



}


class Controller{

    public static Node insertAtFront(Node head, string newdata){


        Node newNode = new Node(newdata);


        newNode.Next = head;


        if (head != null) {

            head.Previous = newNode;


        }

        return newNode;

    }


}

class Program
{
    static void Main(string[] args)
    {
        string[] words = {"the", "fox", "jumps", "over", "the", "dog"};

        
        Node head = new Node("Hello");



        head.Next = new Node("From");

        //creating a pointer to the previous head. 
        head.Next.Previous = head;


        head.Next.Next = new Node ("Ajmal Khan");


        //creating a pointer to the previous node. 
        head.Next.Next.Previous = head.Next;


        //starting temporary head


        string newData = "1. ";

        head = Controller.insertAtFront(head, newData);




        Node temp = head; 

        while(temp != null) {
            Console.Write(temp.Data);

            if(temp.Next != null) {

                Console.Write(" <-> ");

            }

            temp = temp.Next;


        }
        
       


    }

     private static void Display(LinkedList<string> words, string test)
    {
        Console.WriteLine(test);
        foreach (string word in words)
        {
            Console.Write(word + " ");
        }
        Console.WriteLine();
        Console.WriteLine();
    }
}
