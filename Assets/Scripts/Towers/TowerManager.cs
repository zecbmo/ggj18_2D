using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerManager : MonoBehaviour
{
    //[Header("Tower Settings")]
    //[SerializeField]
    //float towerFillLevelOn


    [Header("Spawn Settings")]
    [SerializeField]
    int maxActiveTowers = 2;

    [SerializeField]
    float towerLifeTime = 8.0f;
    float towerHideTime = 0;

    [SerializeField]
    float towerSpawnDelay = 2.0f;
    float nextSpawnTime = 0;
    

    [Header("Prefabs")]
    [SerializeField]
    GameObject towerPrefab = null;

    [Header("Spawn Points")]
    [SerializeField]
    GameObject[] spawnPoints = null;

    List<PlayFieldTower> activeTowers = new List<PlayFieldTower>();

    // Use this for initialization
    void Start ()
    {
        if (maxActiveTowers > spawnPoints.Length)
        {
            Debug.Log("Not enought spawn points for max active towers.");
        }

       
     
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (IsGameStarted())
        {
            if (nextSpawnTime < Time.time)
            {
                SpawnTowers();
                nextSpawnTime = Time.time + towerLifeTime + towerSpawnDelay;
                towerHideTime = Time.time + towerLifeTime;
            }

            if (towerHideTime < Time.time && activeTowers.Count != 0)
            {
                RemoveActiveTowers();
            }
        }
	}

    bool IsGameStarted()
    {
        if (GameManager.Instance().gameState == GameState.InGame)
        {
            return true;
        }

        return false;
    }

    void SpawnTowers()
    {
        RemoveActiveTowers();

        List<GameObject> currentPoints  = spawnPoints.ToList<GameObject>();

        for (int i = 0; i < maxActiveTowers; i++)
        {
            int rand = Random.Range(0, currentPoints.Count);
            SpawnTowerAtPoint(currentPoints[rand].transform.position);
            currentPoints.Remove(currentPoints[rand]);
        }
    }

    void SpawnTowerAtPoint(Vector3 point)
    {
        GameObject go = Instantiate(towerPrefab, point, Quaternion.identity);
        PlayFieldTower newTower = go.GetComponent<PlayFieldTower>();
        activeTowers.Add(newTower);
    }

    void RemoveActiveTowers()
    {
        foreach (PlayFieldTower tower in activeTowers)
        {
            tower.HideTower(true);
        }
        activeTowers.Clear();
    }


}
