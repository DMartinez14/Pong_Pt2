 using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour
{
    public float paddleSpeed = 5f;
  //  public float forceStrength = 10f;
    public float impulseStrength = 8f;
    public float maxY = 30f;

    AudioSource paddleAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform.position = new Vector3(-23.3500004f, 29.8669395f, 24.75f);
        paddleAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
    if (Keyboard.current.wKey.isPressed && CompareTag("Player"))
    {
        // Vector3 force = new Vector3(0f, 0f, forceStrength);
        // Rigidbody rBody = GetComponent<Rigidbody>();
        // rBody.AddForce(force, ForceMode.Force);

        Vector3 newPosition = transform.position + new Vector3(0f, paddleSpeed, 0f) * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, 20f, maxY);

        transform.position = newPosition;

        // transform.position += new Vector3(0f, 0f, paddleSpeed) * Time.deltaTime;
    }

        if (Keyboard.current.sKey.isPressed && CompareTag("Player"))
        {
            // Vector3 force = new Vector3(0f, 0f, -forceStrength);
            // Rigidbody rBody = GetComponent<Rigidbody>();
            // rBody.AddForce(force, ForceMode.Force);

            Vector3 newPosition = transform.position - new Vector3(0f, paddleSpeed, 0f) * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, 20f, maxY);
            transform.position = newPosition;
        }
        if (Keyboard.current.upArrowKey.isPressed && CompareTag("player2"))
    {
        // Vector3 force = new Vector3(0f, 0f, forceStrength);
        // Rigidbody rBody = GetComponent<Rigidbody>();
        // rBody.AddForce(force, ForceMode.Force);

        Vector3 newPosition = transform.position + new Vector3(0f, paddleSpeed, 0f) * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, 20f, maxY);
        transform.position = newPosition;
    }

        if (Keyboard.current.downArrowKey.isPressed && CompareTag("player2"))
        {
            // Vector3 force = new Vector3(0f, 0f, -forceStrength);
            // Rigidbody rBody = GetComponent<Rigidbody>();
            // rBody.AddForce(force, ForceMode.Force);

            Vector3 newPosition = transform.position - new Vector3(0f, paddleSpeed, 0f) * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, 20f, maxY);
            transform.position = newPosition;
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
         //Debug.Log($"{this.name} collided with the {collision.gameObject.name}");
        if(paddleAudio != null)
        {
            paddleAudio.Play();
        }
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            return;
        }
        

        float hitOffset = transform.InverseTransformPoint(collision.GetContact(0).point).y;
        Vector3 normal = collision.GetContact(0).normal;

        Vector3 inVelocity = rb.linearVelocity;
        float speed = inVelocity.magnitude;
        if (speed < 0.1f)
        {
            speed = impulseStrength;
        }

        // Increase speed by 10% on each paddle hit
        speed *= 1.1f;

        Vector3 bounceDir = Vector3.Reflect(inVelocity, normal).normalized;
        bounceDir = (bounceDir + transform.up * hitOffset).normalized;
        rb.linearVelocity = bounceDir * speed;
        
        
        // one-liner
        // collision.rigidbody.AddForce(transform.up * impulseStrength, ForceMode.Impulse);
    }

}