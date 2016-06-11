using System;
using UnityEngine;
using System.Collections;
using System.IO;
using Helpers;

public class ScoreHandler : MonoBehaviour
{
    public float TotalPlayerPoints;

    public float BossHealth = 0.0f;
    public bool BossFightStarted = false;
    public int ShotsFired = 0;

    private static bool _created = false;

    private void Awake()
    {
        if (!_created)
        {
            DontDestroyOnLoad(transform.gameObject);
            _created = true;

            try
            {
                File.Delete(ResourceManager.FilepathWithAnalyticData);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
