using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeGenerate : MonoBehaviour
{
    public List<GameObject> plane;
    int randP;
    [SerializeField] Transform startPos;
    float Timer = 0;
    [SerializeField] float maxTime = 3;
    [SerializeField] int planeNum;
    

    void Start()
    {
        Vector3 myVector = new Vector3(20.0f, 0.0f, 0.0f);
        int startMutipler = planeNum - 2;
        for(int i = 0; i < planeNum; i++)
        {
            bool valid = false;
            while (valid == false)
            {
                int randint = Random.Range(0, plane.Count);
                if (plane[randint].active == false)
                {
                    valid = true;
                    plane[randint].SetActive(true);
                    plane[randint].transform.position = transform.position;
                   
                    if (i == 0)
                    {
                        plane[randint].transform.localPosition = transform.localPosition - myVector* startMutipler;
                    }
                    else
                    {
                        plane[randint].transform.localPosition = platform.LastPlatform.transform.localPosition + myVector;
                    }
                    platform.LastPlatform = plane[randint].GetComponent<platform>();
                    //Debug.Log(plane[randint].gameObject.name);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= maxTime)
        {

        }
    }

    public void newplatform()
    {
        
        bool valid = false;
        while (valid == false)
        {
            int randint = Random.Range(0, plane.Count);
            if (plane[randint].active == false)
            {
                valid = true;
                plane[randint].SetActive(true);
                platform.LastPlatform = plane[randint].GetComponent<platform>();
                plane[randint].transform.position = transform.position;
                plane[randint].transform.localPosition = new Vector3(-25, 0, 0);
            }
            
        }
    }
}
