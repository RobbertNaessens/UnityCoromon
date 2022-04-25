using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveNameScript : MonoBehaviour
{
    public static string playerName;
    public Text nameInput;

    public void Update()
    {        
        if(nameInput.text != "")
        {
            playerName = nameInput.text;
        }
        else
        {
            playerName = "PlayerOne";
        }
    }
}
