using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject _prefabSpray;
    [SerializeField] private float _lifetime = 1f;

    public Vector3 _offSet = new Vector3(0,0,0);

    private void Start()
    {
        // Apenas aparece, dispara
        StartCoroutine(Shoot());

        // Se destruye después de un tiempo
        Destroy(gameObject, _lifetime);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSecondsRealtime(1f);

        GameObject _spray = Instantiate(_prefabSpray, transform.position + _offSet, _prefabSpray.transform.rotation);
        _spray.transform.SetParent(transform);
        AudioManager.Instance._sfxSource.PlayOneShot(AudioManager.Instance._spray);
        yield return null;
    }
}
