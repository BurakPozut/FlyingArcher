using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public GameObject groundTile;
    public GameObject Ground;
    public GameObject ArrowPef;
    public GameObject FinalTile;
    private int previousArrowPos = 1;
    [HideInInspector] public static Vector3 nextSpawnPoint;
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
    void Start()
    {
        nextSpawnPoint = Ground.transform.position; // If we don't assing this value first tile will randomly appear
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size-10; i++)
            {
                GameObject obj = SpawnTile(i);
                obj.SetActive(true);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private GameObject SpawnTile(int i)
    {
        GameObject temp =  Instantiate(groundTile, nextSpawnPoint, Quaternion.Euler(90f,90f,0f), Ground.transform);
        nextSpawnPoint = temp.transform.GetChild(0).transform.position;
        if(i>5) ObstacleSpawn(temp);
        return temp;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Tag does't exists: "+tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        //Closing the obstacles
        objectToSpawn.transform.GetChild(1).gameObject.SetActive(false);    // left obstacle
        objectToSpawn.transform.GetChild(2).gameObject.SetActive(false);
        objectToSpawn.transform.GetChild(3).gameObject.SetActive(false);
        //Closing the arrows
        objectToSpawn.transform.GetChild(4).gameObject.SetActive(false);
        objectToSpawn.transform.GetChild(5).gameObject.SetActive(false);
        objectToSpawn.transform.GetChild(6).gameObject.SetActive(false);

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        nextSpawnPoint = objectToSpawn.transform.GetChild(0).transform.position;
        objectToSpawn.transform.rotation = Quaternion.Euler(90f,90f,0f);
        poolDictionary[tag].Enqueue(objectToSpawn);

        ObstacleSpawn(objectToSpawn);
        return objectToSpawn;
    }
    void ObstacleSpawn(GameObject Tile)
    {

        int i = rnd.Next(1,12);
        if(i== 1 || i == 7)    // Left
            Tile.transform.GetChild(1).gameObject.SetActive(true);
        else if(i == 2 || i == 8)   // Middle
            Tile.transform.GetChild(2).gameObject.SetActive(true);
        else if(i == 3 || i == 9)   // Right
            Tile.transform.GetChild(3).gameObject.SetActive(true);
        else if(i==4)   // Left, Middle
        {
            Tile.transform.GetChild(1).gameObject.SetActive(true);
            Tile.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if(i==5)   // Middle, Right
        {
            Tile.transform.GetChild(2).gameObject.SetActive(true);
            Tile.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if(i==6)   // Left, Right
        {
            Tile.transform.GetChild(1).gameObject.SetActive(true);
            Tile.transform.GetChild(3).gameObject.SetActive(true);
        }
        ArrowSpawn(Tile,i);
    }

    public void FinalTileSpawn()
    {
        FinalTile.transform.position = nextSpawnPoint;
        FinalTile.SetActive(true);
    }

    void ArrowSpawn(GameObject Tile, int i)
    {
        // Firt we check where is the obstacle then we look for the previous arrow if we can't decide where to put our next arrow we randomly pick one
        if(i == 1 || i == 7)  // Left Tile Obstacle
        {
            if(previousArrowPos == 0){
                Tile.transform.GetChild(5).gameObject.SetActive(true);   // Middle Arrow
                previousArrowPos = 1;
                return;
            }

            if(rnd.Next(1,3)==1)   
            {
                Tile.transform.GetChild(5).gameObject.SetActive(true);   // Middle Arrow
                previousArrowPos = 1;
                return;
            }
            Tile.transform.GetChild(6).gameObject.SetActive(true);   // Right Arrow
            previousArrowPos = 2;
        }


        else if(i == 2 || i == 8) // Middle Tile Obstacle
        {
            if(previousArrowPos == 0)
            {
                Tile.transform.GetChild(4).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 0;
                return;
            }
            else if (previousArrowPos == 2)
            {
                Tile.transform.GetChild(6).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 2;
                return;
            }


            if(rnd.Next(1,3)==1)
            {
                Tile.transform.GetChild(4).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 0;
                return;
            }
            Tile.transform.GetChild(6).gameObject.SetActive(true);
            previousArrowPos = 2;
        }


        else if (i == 3 || i == 9)    // Right Tile Obstacle
        {
            if(previousArrowPos == 2)
            {
                Tile.transform.GetChild(5).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 1;
                return;
            }
            if(rnd.Next(1,3)==1)
            {
                Tile.transform.GetChild(4).gameObject.SetActive(true);
                previousArrowPos = 0;
                return;
            }
            Tile.transform.GetChild(5).gameObject.SetActive(true);
            previousArrowPos = 1;
        }
        else if(i == 4)
        {
            Tile.transform.GetChild(6).gameObject.SetActive(true);
            previousArrowPos = 2;
        }
        else if(i == 5)
        {
            Tile.transform.GetChild(4).gameObject.SetActive(true);
            previousArrowPos = 0;
        }
        else if(i == 6)
        {
            Tile.transform.GetChild(5).gameObject.SetActive(true);
            previousArrowPos = 1;
        }
        else
        {
            if(previousArrowPos == 0)
            {
                Tile.transform.GetChild(4).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 0;
                return;
            }
            if(previousArrowPos == 1)
            {
                Tile.transform.GetChild(5).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 1;
                return;
            }
            if(previousArrowPos == 2)
            {
                Tile.transform.GetChild(6).gameObject.SetActive(true);   // Left Arrow
                previousArrowPos = 2;
                return;
            }

        }

    }
    
}
