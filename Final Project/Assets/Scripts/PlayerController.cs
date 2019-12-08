using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float timeLeft;

    public Text score;
    public Text endText;
    public Text livesText;
    public Text timerText;

    private int scoreValue;
    private int livesValue;

    public AudioSource musicSource;
    public AudioSource musicSource2;

    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip coinMusic;
    public AudioClip jumpMusic;
    public AudioClip hitMusic;

    private bool facingRight = true;

    Animator anim1;

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        scoreValue = 0;
        livesValue = 3;

        SetLivesText();
        SetScoreText();
        SetTimerText();

        endText.text = "";

        musicSource.clip = backgroundMusic;
        musicSource.Play();

        anim1 = GetComponent<Animator>();
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim1.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim1.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim1.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim1.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim1.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim1.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim1.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim1.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim1.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim1.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim1.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim1.SetInteger("State", 0);
        }

        timeLeft -= Time.deltaTime;
        SetTimerText();
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "pickup")
        {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);
            SetScoreText();
            musicSource2.clip = coinMusic;
            musicSource2.Play();
        }

        if (collision.collider.tag == "enemy")
        {
            livesValue -= 1;
            Destroy(collision.collider.gameObject);
            SetLivesText();
            musicSource2.clip = hitMusic;
            musicSource2.Play();
            StartCoroutine(Timer());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            if (anim1.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anim1.SetInteger("State", 0);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                musicSource2.clip = jumpMusic;
                musicSource2.Play();
            }
        }

        if (collision.collider.tag == "ground")
        {
            if (anim1.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anim1.SetInteger("State", 0);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                musicSource2.clip = jumpMusic;
                musicSource2.Play();
            }
        }
    }

    private void SetLivesText()
    {
        livesText.text = "Lives left: " + livesValue.ToString();

        if (livesValue <= 0)
        {
            endText.text = "You lose! Game created by Jessica Caplova!";
            Destroy(this);
            anim1.SetInteger("State", -1);
            musicSource.clip = loseMusic;
            musicSource.Play();
        }
    }

    private void SetScoreText()
    {
        score.text = "Score: " + scoreValue.ToString();
    
        if (scoreValue == 4)
        {
            transform.position = new Vector2(60.8f, 0.05f);
        }

        if (scoreValue == 4)
        {
            livesValue = 3;
            SetLivesText();
            timeLeft = 60.0f;
            SetTimerText();
        }

        if (scoreValue >= 8)
        {
            endText.text = "You win! Game created by Jessica Caplova!";
            Destroy(this);
            musicSource.clip = winMusic;
            musicSource.Play();
        }
    }

    private void SetTimerText()
    {
        timerText.text = "Time Remaining:" + timeLeft.ToString("f1");

        if (timeLeft < 0)
        {
            endText.text = "You lose! Game created by Jessica Caplova!";
            Destroy(this);
            anim1.SetInteger("State", -1);
            musicSource.clip = loseMusic;
            musicSource.Play();
        }
    }

    IEnumerator Timer()
    {
        speed /= 2;
        yield return new WaitForSeconds(10);
        speed *= 2;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

}
