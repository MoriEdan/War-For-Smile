using UnityEngine;
using System.Collections;
using Helpers;

public class EnemySpawnManager : MonoBehaviour
{
    public Player PlayerReference;
    public GameObject[] ObscalesToSpawn;

    public float SpawnDelay;
    private float _currentSpawnDelay;

    public bool RotateObstacles;

	// Use this for initialization
    private void Start ()
    {
	        
	}
	
	// Update is called once per frame
    private void Update ()
    {
        if (!ResourceManager.isDoingSetup)
        {
            _currentSpawnDelay += Time.deltaTime;

            if (_currentSpawnDelay >= SpawnDelay)
            {
                SpawnObscale();
                _currentSpawnDelay = 0.0f;
            }
        }
    }

    private void SpawnObscale()
    {
        var index = Random.Range(0, ObscalesToSpawn.Length);

        var pos = transform.position;
        pos.x += Random.Range(-2.0f, 2.0f);
        pos.y += Random.Range(-2.0f, 2.0f);

        var scale = Vector3.one;
        scale.x += Random.Range(0.1f, 2.0f);
        scale.y += Random.Range(0.1f, 1.0f);

        var rotation = transform.rotation;
        if (RotateObstacles)
        {
            rotation.z += Random.Range(0.0f, 180.0f);
        }

        var obstacle = ((GameObject)Instantiate(ObscalesToSpawn[index], pos, rotation)).GetComponent<Obstacle>();
        obstacle.Player = PlayerReference;
        obstacle.transform.localScale = scale;
    }
}
