using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//set element for choices
public class Selectable : MonoBehaviour
{
    public object element;
    public void Decide()
    {
        DialogueManager.SetDecision(element);
    }

}

