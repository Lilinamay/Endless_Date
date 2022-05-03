using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//option menu toggle to turn on invert on camera
public class Menu : MonoBehaviour
{
    [SerializeField] Toggle invertCamX;
    [SerializeField] Toggle invertCamY;
    [SerializeField] cambehavior cambehavior;
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (invertCamX.isOn)                //invert camera if toggled
        {
            cambehavior.invertX = -1;
        }
        else
        {
            cambehavior.invertX = 1;
        }

        if (invertCamY.isOn)
        {
            cambehavior.invertY = -1;
        }
        else
        {
            cambehavior.invertY = 1;
        }
    }
}

