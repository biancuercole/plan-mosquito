using UnityEngine;
using System.Collections;
public class Proyectile : MonoBehaviour
{

    private bool hitPlayer = false;
    private DamagePlayer _damagePlayer;
    private CameraShake _cameraShake;
    private void Start()
    {
        _damagePlayer = FindFirstObjectByType<DamagePlayer>();
        _cameraShake = FindFirstObjectByType<CameraShake>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hitPlayer = true;
        Destroy(gameObject,4);

        PointsManager.Instance.SubtractPoints(1);
        AudioManager.Instance._sfxSource.PlayOneShot(AudioManager.Instance._damage);
        _damagePlayer.BlinkDamage();
        _cameraShake.Shake();
        Debug.Log("mossquito muerto");
    }

    private void OnDestroy()
    {
        if (!hitPlayer)
        {
            PointsManager.Instance.AddPoints(2);
            Debug.Log("mossquito esquivó");
        }
    }
}
