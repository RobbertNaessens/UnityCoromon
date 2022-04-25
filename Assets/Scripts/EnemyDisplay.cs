using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;

    public Text nameText;
    public Image artworkImage;
    public Slider sliderEnemy;
    public Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = enemy.name;
        artworkImage.sprite = enemy.artwork;
        sliderEnemy = enemy.slider;
        levelText.text = enemy.level;
    }
}
