using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.3f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
