using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float yvalue;
    // Start is called before the first frame update
    void Awaken()
    {
        //Debug.Log(GetComponent<RectTransform>().sizeDelta.y);
        //yvalue = GetComponent<RectTransform>().sizeDelta.y;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponent<RectTransform>().sizeDelta.y);
        yvalue = GetComponent<RectTransform>().sizeDelta.y;
    }

}
