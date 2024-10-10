using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveContent
    {
        [SerializeField][NonReorderable] GameObject[] monsterSpawner;  // Es un array de GameObjects

        public GameObject[] GetMonsterSpawnList()  // Debe retornar el array de GameObject
        {
            return monsterSpawner;
        }
    }

    [SerializeField][NonReorderable] WaveContent[] waves;  // Es un array de WaveContent
    int currentWave = 0;
    float SpawRange = 10;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        
    }

    void SpawnWave()
    {
        GameObject[] monsters = waves[currentWave].GetMonsterSpawnList();  // Obtener el array de monstruos

        for (int i = 0; i < monsters.Length; i++)
        {
            Instantiate(monsters[i], FindSpawnLoc(), Quaternion.identity);
        }
    }

    Vector3 FindSpawnLoc()
    {
        Vector3 SpawnPos;

        float xLoc = Random.Range(-SpawRange, SpawRange) + transform.position.x;
        float zLoc = Random.Range(-SpawRange, SpawRange) + transform.position.z;
        float yLoc = transform.position.y;

        SpawnPos = new Vector3(xLoc, yLoc, zLoc);

        if (Physics.Raycast(SpawnPos, Vector3.down, 5))
        {
            return SpawnPos;
        }
        else
        {
            return FindSpawnLoc();
        }
    }
}
