using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public float duration = 0.2f;
    public float magnitude = 0.1f;

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
        Debug.Log("Shake camera");
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
