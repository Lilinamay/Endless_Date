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
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 myVector = new Vector3(20.0f, 0.0f, 0.0f);
        //randP = Random.Range(0, plane.Count);
        for(int i = 0; i < 3; i++)
        {
            //plane[i].SetActive(true);
            //plane[i].transform.position = transform.position;
            //if (i == 1)
            //{
            //    plane[i].transform.position = transform.position - myVector;
            //}
            //else
            //{
            //    plane[i].transform.localPosition = platform.LastPlatform.transform.localPosition + myVector;
            //}


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
                        plane[randint].transform.localPosition = transform.localPosition - myVector;
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
        //foreach (GameObject p in plane)
        //{
            
        //    p.SetActive(true);
        //    p.transform.position = transform.position;
        //    p.transform.localPosition = platform.LastPlatform.transform.localPosition + myVector;
        //    p.GetComponent<platform>().becomeAva = true;
            
        //}
        //plane[randP].SetActive(true);
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
