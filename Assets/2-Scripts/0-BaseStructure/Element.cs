using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public Content myContent;
    public Element next;

    public Element(Content content)
    {
        myContent = content;
        next = null;
    }
}
