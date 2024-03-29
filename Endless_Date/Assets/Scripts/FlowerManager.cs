using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//manage flower spawn
public class FlowerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> flowerList;
    bool FlowerInScene = false;
    bool FlowerSpawn = false;
    [SerializeField] int timerMin = 0;
    [SerializeField] int timerMax = 0;
    Vector3 leftPos;        //when spawn either spawn from left or right
    Vector3 rightPos;
    [SerializeField] float offset = 2;
    public float randomFlowerTime;

    // Start is called before the first frame update
    void Start()
    {
        generateFlowerTime();
        leftPos = gameObject.transform.position - offset * Vector3.forward;
        rightPos = gameObject.transform.position + offset * Vector3.forward;
        Debug.Log(flowerList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FlowerInScene)         //random generate flower timer countdown
        {
            FlowerSpawn = false;
            if (randomFlowerTime > 0)
            {
                randomFlowerTime -= Time.deltaTime;
            }
            else
            {
                FlowerInScene = true;
            }
        }

        if (FlowerInScene && !FlowerSpawn)//random generate flower from flower list
        {
            int randomIndex = Random.Range(0, flowerList.Count);
            if(flowerList[randomIndex].active == false)
            {
                flowerList[randomIndex].SetActive(true);
                FlowerSpawn = true;
                //flowerList[randomIndex].transform.position = gameObject.transform.position;
                int randomNum = Random.Range(0, 2);
                if(randomNum == 0)
                {
                    flowerList[randomIndex].transform.position = leftPos;
                }
                else
                {
                    flowerList[randomIndex].transform.position = rightPos;
                }
            }
        }

    }

    public void generateFlowerTime()
    {
        FlowerInScene = false;
        randomFlowerTime = Random.Range(timerMin, timerMax);

    }
}
