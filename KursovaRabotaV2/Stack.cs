class StackNode<T>
{
    public T element;
    public StackNode<T> next;

    public StackNode(T e, StackNode<T> n)
    {
        element = e;
        next = n;
    }
}

class Stack<T>
{
    StackNode<T> top;
    int size;

    public Stack()
    {
        size = 0;
        top = null;
    }

    public int Count()
    {
        return size;
    }

    public bool isEmpty()
    {
        return size == 0;
    }

    public void Push(T e)
    {
        StackNode<T> newest = new StackNode<T>(e, null);
        if (isEmpty())
        {
            top = newest;

        }
        else
        {
            newest.next = top;
            top = newest;

        }
        size++;

    }

    public T Pop()
    {
        if (isEmpty())
        {
            Console.WriteLine("Invalid");
            return default(T);
        }

        T e = top.element;
        top = top.next;

        size--;
        return e;
    }

    public T peak()
    {
        if (isEmpty())
        {
            Console.WriteLine("Invalid");
            return default(T);
        }
        return top.element;
    }

    public void display()
    {
        StackNode<T> p = top;
        while (p != null)
        {
            Console.Write(p.element + "--");
            p = p.next;
        }
        Console.WriteLine();
    }
}


internal class StacksArr<T>
{
    T[] data;
    int top;

    public StacksArr(int n)
    {
        data = new T[n];
        top = 0;
    }

    public int length()
    {
        return top;
    }

    public bool isEmpty()
    {
        return top == 0;
    }

    public bool isFull()
    {
        return top == data.Length;
    }


    public void push(T e)
    {

        if (isFull())
        {
            Console.WriteLine("Overflow");
            return;
        }
        else
        {
            data[top] = e;
            top++;

        }
    }

    public T pop()
    {
        if (isEmpty())
        {
            Console.WriteLine("Underflow");
            return default(T);
        }
        T e = data[top - 1];
        top--;
        return e;
    }

    public T peak()
    {
        if (isEmpty())
        {
            Console.WriteLine("Underflow");
            return default(T);
        }
        return data[top - 1];
    }

    public void display()
    {
        for (int i = 0; i < length(); i++)
        {
            Console.Write(data[i] + "--");
        }
        Console.WriteLine();
    }
}