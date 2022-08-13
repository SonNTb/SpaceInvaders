using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    public GameObject GetObject(GameObject gameObject, Vector2 dir, Quaternion rotation)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
            {
                return CreateNewObject(gameObject, dir, rotation);
            }
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.transform.position = dir;
                _object.transform.rotation = rotation;
                _object.SetActive(true);
                return _object;
            }
        }
        else
        {
            return CreateNewObject(gameObject, dir, rotation);
        }
    }

    private GameObject CreateNewObject(GameObject gameObject, Vector2 dir, Quaternion rotation)
    {
        GameObject newGO = Instantiate(gameObject);
        newGO.name = gameObject.name;
        newGO.transform.position = dir;
        newGO.transform.rotation = rotation;
        return newGO;
    }

    public void ReturnGameObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }
        gameObject.SetActive(false);
    }
}
