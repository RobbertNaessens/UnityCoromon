using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int attack;
    public int maxHp;
    private static int resetHp;
    public int currentHp;
    public int level;
    public new string name;

    public Slider slider;
    public Gradient gradient;
    public Text info;

    public bool isDead = false;

    

    public void Start()
    {
        Redraw();
    }

    public void TakeDamage(int dmg, bool isPlayer = false)
    {
        currentHp -= dmg;
        if(currentHp <= 0)
        {
            currentHp = 0;
            isDead = true;
        }
        SetSlider(currentHp);
        if (isPlayer)
        {
            ManagePlayer.SetHealth(currentHp);
        }
    }

    public void SetSlider(int hp)
    {
        slider.value = hp;
        slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = gradient.Evaluate(slider.normalizedValue);

    }
     public void Redraw()
    {
        resetHp = maxHp;
        slider.maxValue = maxHp;
        info.text = name + "\nLevel: " + level;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        SetSlider(currentHp);
    }
}
