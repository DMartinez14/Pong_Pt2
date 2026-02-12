//Ball
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEditor.PackageManager.Requests;

public class Ball : MonoBehaviour
{
    
    public TextMeshProUGUI countText;
    public TextMeshProUGUI countTextP2; 
    public TextMeshProUGUI winTextObject;
    public TextMeshProUGUI ScoreText;
    public float speed = 10f;
    public float resetDelay = 1.5f;
    private float currentSpeed;
    private int count;
    private int countP2;
    private Rigidbody rb;
    private CameraShake camShake;
    public ParticleSystem hitParticles;
    AudioSource WallAudio;
    public AudioSource goalAudio;

    private Coroutine resetCoroutine;
    private Player lastPaddleHit;
    private Coroutine powerUpCoroutine;

    void Start()
    {
        //goalAudio = GetComponent<AudioSource>();
        WallAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        currentSpeed = speed;
        camShake = Camera.main.GetComponent<CameraShake>();
        LaunchBall();
    }

    void LaunchBall()
    {
        float xDir = Random.value < 0.5f ? -1f : 1f;
        float yDir = Random.Range(-0.5f, 0.5f);
        Vector3 direction = new Vector3(xDir, yDir, 0).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("out")){

            ResetBall();
        }
        if (other.gameObject.CompareTag("P1Point"))
        {
            if(goalAudio != null)
            {
                goalAudio.Play();
            }
            ScoreText.gameObject.SetActive(true);
            ScoreText.text = "Goooal!";
            count = count + 1;
            countText.color = GetRandomColor();
            SetCountText();
            Debug.Log("Player 1 just Scored ");
            Debug.Log("The Score is " + count + " - " + countP2);
            ResetBall();
        }
        else if (other.gameObject.CompareTag("P2Point"))
        {
            ScoreText.gameObject.SetActive(true);
            if(goalAudio != null)
            {
                goalAudio.Play();
            }
            ScoreText.text = "Goooal!";
            countP2 = countP2 + 1;
            countTextP2.color = GetRandomColor();
            SetCountText(); // Update Player 2 score display
            Debug.Log("Player 2 justed Scored ");
            Debug.Log("The Score is " + count + " - " + countP2);
            ResetBall();
        }

        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("player2"))
        {
            lastPaddleHit = other.gameObject.GetComponent<Player>();
        }
        if(other.gameObject.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
            ApplyRandomPowerUp();
        }
        
    }

    void SetCountText() 
    {
        countText.text = count.ToString();
        if (countTextP2 != null)
        {
            countTextP2.text = countP2.ToString();
        }
        if (count >= 11 || countP2 >= 11)
        {
            Debug.Log("Game Over");
            if(count > countP2)
            {
                winTextObject.gameObject.SetActive(true);
                winTextObject.text = "Game Over, Player 1 Wins";
                Debug.Log("Game Over, Left Paddle Wins");
                Debug.Log("0-0");
                
            }
            else
            {
                winTextObject.gameObject.SetActive(true);
                winTextObject.text = "Game Over, Player 2 Wins";
                Debug.Log("Game Over, Right Paddle Wins");
                Debug.Log("0-0");
            }
            Destroy(GameObject.FindGameObjectWithTag("ball"));
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check for scoring tags
        // if (collision.gameObject.CompareTag("P1Point"))
        // {
            
         
        //     ResetBall();
        //     return;
        // }
        // else if (collision.gameObject.CompareTag("P2Point"))
        // {
           
        //     ResetBall();
        //     return;
        // }
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("player2"))
    {
        lastPaddleHit = collision.gameObject.GetComponent<Player>();
    }
        //Plays audio on wall hit
        if (collision.gameObject.CompareTag("Wall") && WallAudio != null) WallAudio.Play();
        // Camera shake on wall hit
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("player2"))
        {
            if (camShake != null) camShake.Shake();

        }
        // Reflect the velocity for classic Pong bounce
        Vector3 normal = collision.contacts[0].normal;
        Vector3 newVel = Vector3.Reflect(rb.linearVelocity, normal).normalized * currentSpeed;
        currentSpeed *= 1.1f; // Increase speed by 10% on each paddle hit
        // Prevent vertical-only bouncing: ensure X velocity is always above a minimum
        float minX = 0.5f;
        if (Mathf.Abs(newVel.x) < minX)
        {
            newVel.x = minX * Mathf.Sign(newVel.x == 0 ? Random.value - 0.5f : newVel.x);
            newVel = newVel.normalized * currentSpeed;
        }
        rb.linearVelocity = newVel;

    }
    
    void ResetBall()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetBallAfterDelay());
        PowerUpTimer();
        
    }
    void ApplyRandomPowerUp()
{
    if (powerUpCoroutine != null)
    {
        StopCoroutine(powerUpCoroutine);
    }
    powerUpCoroutine = StartCoroutine(PowerUpTimer());
    
    int powerUpType = Random.Range(0, 5);
    
    switch(powerUpType)
    {
        case 0: // Increase paddle size
            if(lastPaddleHit != null) lastPaddleHit.transform.localScale *= 1.5f;
            ScoreText.text = "Paddle Size Up!";
            break;
        case 1: // Decrease ball size
            transform.localScale *= 0.7f;
            ScoreText.text = "Ball Size Down!";
            break;
        case 2: // Decrease paddle size (bad power-up)
            if(lastPaddleHit != null) lastPaddleHit.transform.localScale *= 0.7f;
            ScoreText.text = "Paddle Size Down!";
            break;
        case 3: // Increase ball size (bad power-up)
            transform.localScale *= 1.5f;
            ScoreText.text = "Ball Size Up!";
            break;
        case 4: 
            currentSpeed /= 2f; 
            if(lastPaddleHit != null) lastPaddleHit.paddleSpeed /= 2f;
            ScoreText.text = "Speed Down!";
            break;
        
    }
    ScoreText.gameObject.SetActive(true);
}

IEnumerator PowerUpTimer(){
    yield return new WaitForSeconds(8f);
    // Reset both paddles to original size
    Player[] allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
    foreach(Player player in allPlayers)
    {
        player.transform.localScale = new Vector3(1, 6, 1.45421553f);
    }
    // Reset ball size
    transform.localScale = new Vector3(2, 2, 2);
    powerUpCoroutine = null;
}
 IEnumerator ResetBallAfterDelay()
{
    //rb.isKinematic = true;
    rb.detectCollisions = false;
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    //resets speed
    currentSpeed = speed; 
    Vector3 resetPosition = new Vector3(0, 30, 23.95f);
    transform.position = resetPosition;
    float elapsed = 0f;
    while (elapsed < resetDelay)
    {
        transform.position = resetPosition;
        elapsed += Time.deltaTime;
        yield return null;
    }
    rb.isKinematic = false;
    rb.detectCollisions = true;
    ScoreText.gameObject.SetActive(false);
    LaunchBall();
    resetCoroutine = null;
}

Color GetRandomColor()
{
    // Random HSV for vibrant colors
    return Color.HSVToRGB(Random.value, 1f, 1f);
}
}
