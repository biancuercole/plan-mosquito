using UnityEngine;
using System.Collections;
//using TMPro.EditorUtilities;
public class EnemySpawn : MonoBehaviour
{
    [Header("Configuraci�n de Spawn")]
    public GameObject enemyPrefab;
    public float topLimit = 4f;
    public float bottomLimit = -4f;
    public float spawnInterval = 5f;

    public float _spawnEnemies = 5;
    public float _currentEnemies = 0;

    public Menu _sceneManager;
    private void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    private IEnumerator SpawnCycle()
    {
        while (_currentEnemies <= _spawnEnemies)
        {
            // Genera una posici�n aleatoria en el eje Y
            float randomY = Random.Range(bottomLimit, topLimit);
            Vector3 spawnPos = new Vector3(transform.position.x, randomY, transform.position.z);

            // Instancia el enemigo
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            // Espera antes de volver a spawnear
            yield return new WaitForSeconds(spawnInterval);
            _currentEnemies++;
        }
        Debug.Log($"EnemySpawn: spawn loop finished (_currentEnemies={_currentEnemies}, _spawnEnemies={_spawnEnemies}). Calling Victory via _sceneManager.");
        if (_sceneManager == null)
        {
            Debug.LogWarning("EnemySpawn: _sceneManager is null. Attempting to find Menu in scene.");
            var found = FindObjectOfType<Menu>();
            if (found != null)
            {
                found.ChangeScene("VictoryMinigame");
            }
            else
            {
                Debug.LogError("EnemySpawn: No Menu found to load Victory.");
            }
        }
        else
        {
            _sceneManager.RandomLevel();
        }
    }
}