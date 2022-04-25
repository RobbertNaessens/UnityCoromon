
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum States { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSceneManager : MonoBehaviour
{
    //variabelen
    private Unit enemy;
    public Unit player;

    public GameObject enemyObject;
    public GameObject playerObject;

    public GameObject FireballToEnemy;
    public GameObject FireballToPlayer;
    public Sprite RedFire;
    public Sprite BlueFire;

    public Text info;
    public Slider slider;

    private States currentState;

    public Text gameOmschrijving;
    public GameObject attackButton;
    public GameObject runButton;

    public Animator sceneTransition;

    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        //enemy aanmaken volgens het platform
        GameObject enemiesInstance = enemies[CreatePlatform.getIndex()];
        enemyObject = Instantiate(enemiesInstance, new Vector3(0.5f, -1.5f, 0), enemiesInstance.transform.rotation);      
        enemy = enemyObject.GetComponent<Unit>();
        enemy.info = info;
        enemy.slider = slider;

        //start van de turn based combat
        currentState = States.START;
        gameOmschrijving.text = "A wild enemy approaches..";
        currentState = States.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }
    //speler zijn beurt
    IEnumerator PlayerTurn()
    {
        //functie om een aantal seconden te wachten
        yield return new WaitForSeconds(1);
        //gametext aanpassen 
        gameOmschrijving.text = "What is your next move?";
        //attack buttons aanzetten zodat speler de volgende move kan kiezen
        SetButtons(true);
    }
    //vuurbal animatie afhankelijk van de huige state
    IEnumerator PlayAnimation()
    {
        if(currentState == States.PLAYERTURN)
        {
            FireballToEnemy.SetActive(true);
            Animator anim = FireballToEnemy.GetComponent<Animator>();
            anim.Play("Base Layer.FireballToEnemy");
            yield return new WaitForSeconds(1);
            FireballToEnemy.SetActive(false);
        }
        else if(currentState == States.ENEMYTURN)
        {
            FireballToPlayer.SetActive(true);
            Animator anim = FireballToPlayer.GetComponent<Animator>();
            anim.Play("Base Layer.FireballToEnemy");
            yield return new WaitForSeconds(1);
            FireballToPlayer.SetActive(false);
        }

    //attack function voor op de button te hangen (kan geen IEnumerator zijn)    
    }
    public void Attack()
    {
        StartCoroutine(PlayerAttack());
    }
    //random attack function voor op de button te hangen
    public void RandomAttack()
    {
        StartCoroutine(PlayerRandomAttack());
    }
    //echte attack functie
    public IEnumerator PlayerAttack()
    {
        if(currentState == States.PLAYERTURN)
        {
            SetButtons(false);
            FireballToEnemy.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            FireballToEnemy.GetComponent<SpriteRenderer>().sprite = RedFire;
            StartCoroutine(PlayAnimation());
            yield return new WaitForSeconds(1);
            gameOmschrijving.text = "You attacked " + enemy.name + " for " + player.attack + " damage!";
            enemy.TakeDamage(player.attack);
            CheckGame();
        }
    }
    public IEnumerator PlayerRandomAttack()
    {
        if (currentState == States.PLAYERTURN)
        {
            SetButtons(false);
            int randomDmg = Random.Range(player.attack - 2, player.attack + 4);
            FireballToEnemy.GetComponent<Transform>().localScale += new Vector3(0.1f, 0.1f, 0.1f) * (randomDmg - player.attack);
            FireballToEnemy.GetComponent<SpriteRenderer>().sprite = BlueFire;
            StartCoroutine(PlayAnimation());
            yield return new WaitForSeconds(1);
            gameOmschrijving.text = "You attacked " + enemy.name + " for " + randomDmg + " damage!";
            enemy.TakeDamage(randomDmg);
            CheckGame();
        }
    }
    //enemy attack turn
    IEnumerator EnemyAttacks()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(PlayAnimation());
        yield return new WaitForSeconds(1);
        gameOmschrijving.text = enemy.name + " attacked you for " + enemy.attack + " damage.";
        player.TakeDamage(enemy.attack, true);
        CheckGame();
    }
    //game eindigen op een manier afhankelijk van wie er gewonnen is
    IEnumerator EndGame()
    {
        SetButtons(false);
        yield return new WaitForSeconds(1);
        if (currentState == States.WON)
        {
            gameOmschrijving.text = "You defeated " + enemy.name + " !";
            //animatie van speler die sterft
            StartCoroutine(PlayerDies(GameObject.Find("EnemyImage")));
            yield return new WaitForSeconds(2.5f);
            gameOmschrijving.text = "You gained 75 experience.";
            yield return new WaitForSeconds(0.5f);
            //player krijgt exp, dit zorgt voor mogelijke level-up
            ManagePlayer.GainExp(75);
            yield return new WaitForSeconds(0.5f);
            if(CreatePlatform.index == enemies.Length)
            {
                StartCoroutine(LoadScene("WonScene"));
            }
            else
            {
                StartCoroutine(LoadScene("StartScene"));
            }          
        }
        else if(currentState == States.LOST)
        {
            gameOmschrijving.text = "You were defeated by " + enemy.name + " ...";
            //animatie van speler die sterft
            StartCoroutine(PlayerDies(GameObject.Find("PlayerImage")));
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(LoadScene("LostScene"));
        } 
    }
    //functie om terug te gaan naar de wereld
    IEnumerator LoadScene(string name)
    {
        //animatie voor overgang tussen verschillende scenes
        sceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
    }
    //functie die player laat 'sterven'
    IEnumerator PlayerDies(GameObject obj)
    {
        for(int i = 0; i < 3; i++)
        {
            obj.transform.position -= obj.transform.up * 1/2;
            yield return new WaitForSeconds(0.700F);
        }
        obj.SetActive(false);
    }
    //afhankelijk van de state naar de correcte volgende state gaan
    void CheckGame()
    {
        if (player.isDead)
        {
            currentState = States.LOST;
            StartCoroutine(EndGame());
        }
        else if (enemy.isDead)
        {
            currentState = States.WON;
            StartCoroutine(EndGame());
        }
        else if(currentState == States.PLAYERTURN)
        {
            currentState = States.ENEMYTURN;
            StartCoroutine(EnemyAttacks());
        }
        else
        {
            currentState = States.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }
    //functie om attack buttons aan/af te zetten
    void SetButtons(bool value)
    {
        attackButton.SetActive(value);
        runButton.SetActive(value);
    }
    
}
