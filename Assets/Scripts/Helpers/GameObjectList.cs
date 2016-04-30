using UnityEngine;
using System.Collections;

using Helpers;

public class GameObjectList : MonoBehaviour
{
    public GameObject[] Objects;

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            ResourceManager.SetGameObjectList(this);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject GetGameObject(string objectName)
    {
        foreach (var obj in Objects)
        {
            if (obj.name == objectName)
            {
                return obj;
            }
        }
        return null;
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
}
