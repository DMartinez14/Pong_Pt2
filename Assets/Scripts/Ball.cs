using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI countTextP2; // Added for Player 2 score
    public TextMeshProUGUI winTextObject;
    public float speed = 10f;
    private float currentSpeed;
    private int count;
    private int countP2;
    private Rigidbody rb;
    private CameraShake camShake;
    public ParticleSystem hitParticles;

    public AudioSource goalAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.gameObject.SetActive(false);
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
        if (other.gameObject.CompareTag("P1Point"))
        {
            goalAudio.Play();
            count = count + 1;
            SetCountText();
            Debug.Log("Player 1 just Scored ");
            Debug.Log("The Score is " + count + " - " + countP2);
            ResetBall();
        }
        else if (other.gameObject.CompareTag("P2Point"))
        {
            goalAudio.Play();
            countP2 = countP2 + 1;
            SetCountText(); // Update Player 2 score display
            Debug.Log("Player 2 justed Scored ");
            Debug.Log("The Score is " + count + " - " + countP2);
            ResetBall();
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
        if (collision.gameObject.CompareTag("P1Point"))
        {
            
            ResetBall();
            return;
        }
        else if (collision.gameObject.CompareTag("P2Point"))
        {
            
            ResetBall();
            return;
        }
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
        rb.linearVelocity = Vector3.zero;
        currentSpeed = speed; // Reset speed to initial value
        transform.position = new Vector3(0,30,24.75f);
        LaunchBall();
    }
}