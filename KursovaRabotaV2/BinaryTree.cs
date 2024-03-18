using System;

internal class BinaryTree
{
    public Node root;

    public BinaryTree()
    {
        root = null;
    }

    public void Insert(Node newNode)
    {
        if (root == null)
        {
            root = newNode;
            return;
        }

        Node temp = null;
        Node current = root;

        while (current != null)
        {
            temp = current;
            if (newNode.element == current.element)
                return;
            else if (newNode.element.CompareTo(current.element) < 0)
                current = current.left;
            else
                current = current.right;
        }

        if (newNode.element.CompareTo(temp.element) < 0)
            temp.left = newNode;
        else
            temp.right = newNode;
    }

    public void InOrder(Node troot)
    {
        if (troot != null)
        {
            InOrder(troot.left);
            Console.Write(troot.element + " ");
            InOrder(troot.right);
        }
    }

    public void PreOrder(Node troot)
    {
        if (troot != null)
        {
            Console.Write(troot.element + " ");
            PreOrder(troot.left);
            PreOrder(troot.right);
        }
    }

    public string PostOrder(Node troot, string postOrderTraversal)
    {
        
        if (troot != null)
        {
            PostOrder(troot.left,postOrderTraversal);
            PostOrder(troot.right,postOrderTraversal);
            postOrderTraversal += troot.element;
            //Console.Write(troot.element + " ");
        }
        return postOrderTraversal;
    }

    public int Count(Node troot)
    {
        if (troot == null)
            return 0;

        int leftCount = Count(troot.left);
        int rightCount = Count(troot.right);

        return leftCount + rightCount + 1;
    }

    public int Height(Node troot)
    {
        if (troot == null)
            return 0;

        int leftHeight = Height(troot.left);
        int rightHeight = Height(troot.right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }
}


