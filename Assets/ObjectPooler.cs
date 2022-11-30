using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        //Detta bestämmer vad vi spawnar
        public string tag;
        public GameObject prefab;
        //Detta bestämmer hur stor poolen får var (när object despawnar)
        public int size;

    }
    //regions är coola
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

        //Skapar en pool för ordboken genom att loopa igenom alla tillgängliga pooler.
        foreach (Pool pool in pools) //fråga Bjarne varför det inte kune vara ett semikolon här.
        {
            //Skapar en kö för objekt
            Queue<GameObject> objectPool = new Queue<GameObject>();

          //Skapar objekten
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //Lägger till poolen i ordboken
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        //om koden får en tagg den inte har så skickar den ett felmeddelande istället för att krascha spelet
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
        //lägger tillbaka objektet i kön
        poolDictionary[tag].Enqueue(objectToSpawn);

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }

}
