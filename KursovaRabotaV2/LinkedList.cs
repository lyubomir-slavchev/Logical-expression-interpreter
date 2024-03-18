using System.Collections;
using System.Collections.Generic;
using System;


public class LinkedListNode<T>
{
    public T element;
    public LinkedListNode<T> next;
    public LinkedListNode(T e, LinkedListNode<T> n)
    {
        element = e;
        next = n;
    }
}

class LinkedList<T>
{
    private LinkedListNode<T> head;
    private LinkedListNode<T> tail;
    private int size;

    public LinkedList()
    {
        head = null;
        tail = null;
        size = 0;
    }

    public int length()
    {
        return size;
    }

    public bool isEmpty()
    {
        return size == 0;
    }

    public void addLast(T e)
    {
        LinkedListNode<T> newest = new LinkedListNode<T>(e, null);
        if (isEmpty())
            head = newest;
        else
            tail.next = newest;
        tail = newest;
        size = size + 1;
    }

    public void addFirst(T e)
    {
        LinkedListNode<T> newest = new LinkedListNode<T>(e, null);
        if (isEmpty())
        {
            head = newest;
            tail = newest;
        }
        else
        {
            newest.next = head;
            head = newest;
        }
        size = size + 1;
    }

    public void addAny(T e, int position)
    {
        if (position <= 0 || position >= size)
        {
            Console.WriteLine("Invalid Position");
            return;
        }
        LinkedListNode<T> newest = new LinkedListNode<T>(e, null);
        LinkedListNode<T> p = head;
        int i = 1;
        while (i < position - 1)
        {
            p = p.next;
            i = i + 1;
        }
        newest.next = p.next;
        p.next = newest;
        size = size + 1;
    }



    public T removeFirst()
    {
        if (isEmpty())
        {
            Console.WriteLine("List is Empty");
            return head.element;
        }
        else
        {
            T e = head.element;
            head = head.next;
            size = size - 1;
            if (isEmpty())
                tail = null;
            return e;
        }
    }

    public T removeLast()
    {
        if (isEmpty())
        {

            return head.element;
        }
        LinkedListNode<T> p = head;
        int i = 1;
        while (i < size - 1)
        {
            p = p.next;
            i = i + 1;
        }
        tail = p;
        p = p.next;
        T e = p.element;
        tail.next = null;
        size = size - 1;
        return e;
    }

    public T removeAny(int position)
    {
        if (position <= 0 || position >= size - 1)
        {
            Console.WriteLine("Invalid Position");
            return head.element;
        }
        LinkedListNode<T> p = head;
        int i = 1;
        while (i < position - 1)
        {
            p = p.next;
            i = i + 1;
        }
        T e = p.next.element;
        p.next = p.next.next;
        size = size - 1;
        return e;
    }



    public void display()
    {
        LinkedListNode<T> p = head;
        while (p != null)
        {
            Console.Write(p.element + " --> ");
            p = p.next;
        }
        Console.WriteLine();
    }

    public IEnumerator<T> GetEnumerator()
    {
        LinkedListNode<T> current = head;
        while (current != null)
        {
            yield return current.element;
            current = current.next;
        }
    }

  
}