namespace SlidingWindow;

class Window
{

    private int[] array;
    private int StartPointer;
    private int EndPointer;

    public Window(int startpointer, int endpointer) {
        
        StartPointer = startpointer;
        EndPointer = endpointer;
    }

    public void Scope(int[] array) {

        for(int i = StartPointer; i < EndPointer; i++)
        {

            Console.Write(array[i]);

        }

    }





}

class Program
{
    static void Main(string[] args)
    {
        int[] array = new int[10]{1, 2, 5, 3, 4, 6, 7, 8, 9, 10};

        Window w = new Window(2, 5);

        w.Scope(array);

    }
}
