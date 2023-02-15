using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   #region Paramters
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Transform _enemyPoolContainer;


    private List<GameObject> _enemyPool = new List<GameObject>();
    private float _horizontalBounds = 9.5f;
    private float _verticalBounds = 5.5f;
    private bool _stopSpawning = false;

   #endregion

    void Start() {
        PrePopulateEnemyPool();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() {
        while (!_stopSpawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds), _verticalBounds, 0f);
            GameObject enemy = RetrieveEnemyFromPool();
            enemy.transform.position = posToSpawn;
            yield return new WaitForSeconds(5f);
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }

    #region EnemyPool
    private void PrePopulateEnemyPool()  {
        for (int i = 0; i < 20; i++)
        {
            GameObject tempObject = Instantiate(_enemyPrefab, Vector3.zero , Quaternion.identity) as GameObject;
            tempObject.transform.parent = _enemyPoolContainer;
            tempObject.SetActive(false);
            tempObject.name = "Enemy" + i.ToString("000");
            _enemyPool.Add(tempObject);
        }
    }

    private GameObject RetrieveEnemyFromPool() {
        GameObject enemy;
        if (_enemyPool.Count > 0)
        {
            enemy = _enemyPool[_enemyPool.Count - 1];
            _enemyPool.Remove(enemy);
        }
        else {
            enemy = Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            enemy.transform.parent = _enemyPoolContainer;
            enemy.name = "Enemy" + _enemyPoolContainer.childCount.ToString("000");
            _enemyPool.Add(enemy);
        }
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemyPrefabToPool(GameObject enemy) {
        enemy.transform.position = Vector3.zero;
        enemy.SetActive(false);
        _enemyPool.Add(enemy);
    }
    #endregion
}
