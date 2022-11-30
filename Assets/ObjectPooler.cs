using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        //Detta best�mmer vad vi spawnar
        public string tag;
        public GameObject prefab;
        //Detta best�mmer hur stor poolen f�r var (n�r object despawnar)
        public int size;

    }
    //regions �r coola
    #region Singleton
    public static ObjectPooler Instance;

    public void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //Skapar en pool f�r ordboken genom att loopa igenom alla tillg�ngliga pooler.
        foreach (Pool pool in pools) //fr�ga Bjarne varf�r det inte kune vara ett semikolon h�r.
        {
            //Skapar en k� f�r objekt
            Queue<GameObject> objectPool = new Queue<GameObject>();

          //Skapar objekten
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //L�gger till poolen i ordboken
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        //om koden f�r en tagg den inte har s� skickar den ett felmeddelande ist�llet f�r att krascha spelet
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't exist");
            return null;
        }


       GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        iPooledObject pooledObj = objectToSpawn.GetComponent<iPooledObject>();
        //l�gger tillbaka objektet i k�n
        poolDictionary[tag].Enqueue(objectToSpawn);

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }

}
