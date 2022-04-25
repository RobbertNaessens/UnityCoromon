using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayer : MonoBehaviour
{
    public Unit playerStats;
    public static int maxHp;
    private static int level;
    private int attack;
    public static int currentHp;

    public Text hpText;
    public Text expText;
    private static int exp;
    private static bool leveledUp = false;

    public GameObject levelUpUI;
    public Text levelUpText;
    int vorigLevel;
    int vorigMaxHp;
    int vorigAttack;

    public static int verschil;

    public static string statsText;

    // Start is called before the first frame update
    void Start()
    {
        if (CreatePlatform.index == 1)
        {
            ResetStats();
        }
        getStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (leveledUp)
        {
            leveledUp = false;
            LevelUp();
            PlayerPrefs.SetInt("Level", playerStats.level);
            PlayerPrefs.SetInt("Health", playerStats.currentHp);
            PlayerPrefs.SetInt("maxHp", playerStats.maxHp);
            PlayerPrefs.SetInt("attack", playerStats.attack);
            playerStats.Redraw();
        }
        getStats();
        hpText.text = "Hp: " + playerStats.currentHp + "/" + playerStats.maxHp;
        expText.text = "Exp: " + exp + "/100";
        verschil = playerStats.maxHp - playerStats.currentHp;
        statsText = "Last Battle Results: \n" + SaveNameScript.playerName + ":\n\nLevel: " + playerStats.level + "\n\nHp: " +
                      playerStats.currentHp + "/" + playerStats.maxHp + "\n\nAttack: " + playerStats.attack;
    }
    void getStats()
    {
        maxHp = playerStats.maxHp;
        exp = PlayerPrefs.GetInt("Exp");
        if (PlayerPrefs.GetInt("Health") != 0)
        {
            currentHp = PlayerPrefs.GetInt("Health");
            playerStats.currentHp = currentHp;
        }
        if (PlayerPrefs.GetInt("Level") != 0)
        {
            level = PlayerPrefs.GetInt("Level");
            playerStats.level = level;
        }
        if (PlayerPrefs.GetInt("attack") != 0)
        {
            attack = PlayerPrefs.GetInt("attack");
            playerStats.attack = attack;
        }
        if (PlayerPrefs.GetInt("maxHp") != 0)
        {
            maxHp = PlayerPrefs.GetInt("maxHp");
            playerStats.maxHp = maxHp;
        }
        playerStats.name = SaveNameScript.playerName;
        playerStats.Redraw();
    }

    public static void SetHealth(int value)
    {
        PlayerPrefs.SetInt("Health", value);
    }

    public static void Heal()
    {
        PlayerPrefs.SetInt("Health", maxHp);
    }

    void ResetStats()
    {
        PlayerPrefs.SetInt("Exp", 0);
        PlayerPrefs.SetInt("Health", playerStats.maxHp);
        PlayerPrefs.SetInt("Level", playerStats.level);
        PlayerPrefs.SetInt("maxHp", playerStats.maxHp);
        PlayerPrefs.SetInt("attack", playerStats.attack);
    }

    public static void GainExp(int expGain)
    {
        exp += expGain;
        if (exp >= 100)
        {
            exp -= 100;
            leveledUp = true;
        }
        PlayerPrefs.SetInt("Exp", exp);
    }

    public void LevelUp()
    {
        vorigLevel = level;
        level++;
        playerStats.level = level;
        vorigMaxHp = maxHp;
        maxHp += 3;
        playerStats.maxHp = maxHp;
        playerStats.currentHp += 3;
        vorigAttack = attack;
        attack += Random.Range(1, 3);
        playerStats.attack = attack;
        StartCoroutine(RedrawUI());
    }
    public IEnumerator RedrawUI()
    {
        Debug.Log("Level-up scherm");
        levelUpUI.SetActive(true);
        levelUpText.text = "Level: " + vorigLevel + " => " + level + "\n\nAttack: " + vorigAttack + " => " + attack + "\n\nHp: " + vorigMaxHp + "=>" + maxHp;
        yield return new WaitForSeconds(5f);
        levelUpUI.SetActive(false);
    }

    public static int getHeathDiff()
    {
        return verschil;
    }

    public string getStatsString()
    {
        getStats();
        return SaveNameScript.playerName + ":\n\nLevel: " + playerStats.level + "\n\nHp: " +
                      playerStats.currentHp + "/" + playerStats.maxHp + "\n\nAttack: " + playerStats.attack;
    }
}
