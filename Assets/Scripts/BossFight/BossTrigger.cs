using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public GameObject Boss;
    public GameObject ChasingBorder;
    public GameObject EnemySpawner;

    private bool _bossActivated = false;

    private void Start ()
    {
	
	}
	
    private void Update ()
    {
	
	}

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!_bossActivated)
        {
            var player = coll.gameObject.GetComponent<Player>();
            if (player)
            {
                Boss.SetActive(true);
                ChasingBorder.SetActive(false);
                EnemySpawner.SetActive(false);
                _bossActivated = true;
            }
        }
    }
}
