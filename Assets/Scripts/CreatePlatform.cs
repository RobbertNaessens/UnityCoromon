using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlatform : MonoBehaviour
{
    private List<GameObject> platformsList = new List<GameObject>();
    public GameObject[] platforms;
    public static int index = 0;
    public GameObject bossPlatform;

    private float time;
    

    private void Start()
    {
        
        //Place every element of the array in the list
        //Because we can't remove objects from an array...
        foreach(GameObject o in platforms)
        {
            platformsList.Add(o);
        }

        //Saved index, so that the correct platfrom gets displayed
        index = PlayerPrefs.GetInt("index");
    }
    // Update is called once per frame
    void Update()
    {
        //Keeps track of the time in seconds 
        time += Time.deltaTime;

        //Every 3 seconds a new platfrom becomes active
        if (time > 3 && index < platformsList.Count)
        {
            MakePlatform(index);
            time = 0;
        }
        //If all the platforms were activated, the boss platfrom becomes activated
        if(index == platformsList.Count)
        {
            bossPlatform.SetActive(true);
        }
        //Debug.Log(index);

    }
    //Method to make platfrom active
    void MakePlatform(int index)
    {
        platformsList[index].SetActive(true);
    }

    //Method to 'delete' a platform
    //We increase the index so that the platfrom never becomes active again
    public void deletePlatform()
    {
        index++;
        PlayerPrefs.SetInt("index", index);
    }

    public static int getIndex()
    {
        return index - 1;
    }

    public static void DeleteIndex()
    {
        PlayerPrefs.SetInt("index", 0);
    }
}
