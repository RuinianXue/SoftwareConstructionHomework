// See https://aka.ms/new-console-template for more information
public class Node<T>
{
    public T data; 
    public Node<T> next; 
    public Node(T data) 
    {
        this.data = data;
        this.next = null;
    }
}

public class LinkedList<T>
{
    public Node<T> head;
    public int count;

    public LinkedList() 
    {
        this.head = null;
        this.count = 0;
    }

    public void AddLast(T data)
    {
        Node<T> newNode = new Node<T>(data); 
        if (head == null) 
        {
            head = newNode;
        }
        else  
        {
            Node<T> current = head;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }
        count++; 
    }

    public void ForEach(Action<T> action)
    {
        Node<T> current = head;
        while (current != null)
        {
            action(current.data); 
            current = current.next;
        }

    }
}


class Program
{
    static void Main(string[] args)
    {
        LinkedList<int> list = new LinkedList<int>();  
        list.AddLast(1);  
        list.AddLast(2);
        list.AddLast(3);
        list.AddLast(4);
        int sum = 0;  
        list.ForEach(x => Console.WriteLine(x));  
        list.ForEach(x => sum += x); 
        Console.WriteLine("Sum: " + sum);
        int max = int.MinValue;  
        list.ForEach(x => { if (x > max) max = x; }); 
        Console.WriteLine("Max: " + max);
        int min = int.MaxValue;  
        list.ForEach(x => { if (x < min) min = x; }); 
        Console.WriteLine("Min: " + min);
    }
}