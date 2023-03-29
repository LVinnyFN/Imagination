using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List 
{
    public Element first, last;

    public List()
    {        
        first = new Element(null);
        last = first;        
    }

    //Checks whether list is empty for not execute methods in wrong way
    public bool Empty()
    {
        if (first == last)
            return true;
        else
            return false;
    }

    //Adds an element to list
    public void Add(Content content)
    {
        Element element = new Element(content);
        if (first.next == null)
            first.next = element;
        last.next = element;
        last = element;
    }

    //Removes an element from the list
    public Content Remove(Content content)
    {
        if (Empty()) return null;

        Element aux = first;
        while (aux.next != null && aux.next.myContent != content)
        {
            aux = aux.next;
        }
        if (aux.next == null) return null;
        else
        {
            Element auxRemove = aux.next;
            aux.next = auxRemove.next;  

            return auxRemove.myContent;
        }    

    }

    //Search an element in the list, but don't remove it
    public Content Search(Content content)
    {
        if (Empty()) return null;

        Element aux = first.next;
        while(aux.myContent != content && aux.myContent != null)
        {
            aux = aux.next;
        }

        return aux.myContent;          
        
    }

}
