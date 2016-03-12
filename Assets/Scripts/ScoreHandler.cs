using UnityEngine;
using System.Collections;
using Helpers;

public class ScoreHandler : MonoBehaviour
{
    public float TotalPlayerPoints;

    private static bool _created = false;

    private void Awake()
    {
        if (!_created)
        {
            DontDestroyOnLoad(transform.gameObject);
            _created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
