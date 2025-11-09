using UnityEngine;
using System.Collections;
using TMPro.EditorUtilities;
public class EnemySpawn : MonoBehaviour
{
    [Header("Configuración de Spawn")]
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
            // Genera una posición aleatoria en el eje Y
            float randomY = Random.Range(bottomLimit, topLimit);
            Vector3 spawnPos = new Vector3(transform.position.x, randomY, transform.position.z);

            // Instancia el enemigo
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            // Espera antes de volver a spawnear
            yield return new WaitForSeconds(spawnInterval);
            _currentEnemies++;
        }
        _sceneManager.ChangeScene("Victory");
    }
}