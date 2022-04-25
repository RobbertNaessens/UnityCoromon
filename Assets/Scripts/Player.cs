using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    public Animator sceneTransition;

    public Animator animator;

    public CreatePlatform platformCreater;

    public HuisScript house;

    public GameObject player;
    public GameObject playerStatsUI;
    public Text playerStatsText;
    public Text healText;

    private bool statsShown = false;

    private bool fullyHealed;
    void Start()
    {
        fullyHealed = false;
        if(CreatePlatform.index == 0)
        {
            ResetPosition();
        }
        rb = GetComponent<Rigidbody2D>();
        float xCo = PlayerPrefs.GetFloat("positieX");
        float yCo = PlayerPrefs.GetFloat("positieY");
        float ZCo = PlayerPrefs.GetFloat("positieZ");
        transform.position = new Vector3(xCo, yCo, ZCo);
        
    }
    private void Update()
    {
        playerStatsText.text = ManagePlayer.statsText;
        if (movement.y == 0)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
        }
        if (movement.x == 0)
        {
            movement.y = Input.GetAxisRaw("Vertical");
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!statsShown)
            {
                playerStatsUI.SetActive(true);
                statsShown = true;
            }
            else if (statsShown)
            {
                playerStatsUI.SetActive(false);
                statsShown = false;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetFloat("positieX", transform.position.x);
        PlayerPrefs.SetFloat("positieY", transform.position.y);
        PlayerPrefs.SetFloat("positieZ", transform.position.z);
        if(collision.gameObject.tag == "Huisje")
        {
            StartCoroutine(HealPlayer());
        }
        else
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
            platformCreater.deletePlatform();
        }
        
    }
    IEnumerator HealPlayer()
    {
        house.ChangeSprite(true);
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        ManagePlayer.Heal();
        yield return new WaitForSeconds(2f);
        player.GetComponent<SpriteRenderer>().enabled = true;
        healText.gameObject.SetActive(true);
        if (!fullyHealed)
        {
            healText.text = "+ " + ManagePlayer.getHeathDiff() + "hp";
        }
        else
        {
            healText.text = "+ 0hp";
        }  
        yield return new WaitForSeconds(0.5f);
        house.ChangeSprite(false);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
        healText.gameObject.SetActive(false);
        fullyHealed = true;
    }

    IEnumerator LoadScene(int index)
    {
        sceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }

    void ResetPosition()
    {
        PlayerPrefs.SetFloat("positieX", 2);
        PlayerPrefs.SetFloat("positieY", -2);
        PlayerPrefs.SetFloat("positieZ", 0);
    }
}

