using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeCheck : MonoBehaviour
{
    public RectTransform parent;
    public GameObject childPrefab;
    //public Text readout;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= .5f)
        {
            var child = Instantiate(childPrefab);
            child.transform.SetParent(parent.transform);

            timer = 0;
        }
        //readout.text = string.Format("Parent Width: {0} units", parent.rect.width.ToString());
        Debug.Log(parent.rect.width.ToString());
    }
}
