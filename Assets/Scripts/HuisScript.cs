using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuisScript : MonoBehaviour
{
    public Sprite deurGesloten;
    public Sprite deurOpen;
    SpriteRenderer rendererHuis;
    // Start is called before the first frame update
    void Start()
    {
        rendererHuis = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(bool open)
    {
        if (open)
        {
            rendererHuis.sprite = deurOpen;
        }
        else
        {
            rendererHuis.sprite = deurGesloten;
        }
       
    }
}
