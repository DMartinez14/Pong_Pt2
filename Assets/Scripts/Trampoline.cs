using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float impulseStrength = 1f;

    void OnCollisionEnter(Collision collision)
    {
         Debug.Log($"{this.name} collided with the {collision.gameObject.name}");
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }

        float hitOffset = transform.InverseTransformPoint(collision.GetContact(0).point).z;
        Vector3 normal = collision.GetContact(0).normal;

        Vector3 inVelocity = rb.linearVelocity;
        float speed = inVelocity.magnitude;
        if (speed < 0.1f)
        {
            speed = impulseStrength;
        }

        Vector3 bounceDir = Vector3.Reflect(inVelocity, normal).normalized;
        bounceDir = (bounceDir + transform.forward * hitOffset).normalized;
        rb.linearVelocity = bounceDir * speed;
        
        
        // one-liner
        // collision.rigidbody.AddForce(transform.up * impulseStrength, ForceMode.Impulse);
    }
}
