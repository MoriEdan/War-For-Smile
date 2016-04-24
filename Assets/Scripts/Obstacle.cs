using UnityEngine;
using System.Collections;
using Helpers;

public class Obstacle : Entity
{
    public Player Player;

    public Color Color;
    public float Health = 100;

    public AudioSource ExplosionAudioSource;
    public BoxCollider2D BoxCollider2D;
    public GameObject AliveGameObject;
    public GameObject DeadGameObject;

    public float DamageFromCollision;
    public float ScoreToAdd;

    private bool _enabledDeadEffects = false;

	// Use this for initialization
    protected override void Start ()
    {
	
	}
	
	// Update is called once per frame
    protected override void Update ()
    {
        if (_enabledDeadEffects)
        {
            Destroy(gameObject, 1.9f);
            return;
        }

        if (Health <= 0 && !_enabledDeadEffects)
        {
            DestoryOnDemand();
        }
	}

    public void DestoryOnDemand(bool addScoreToPlayer = true)
    {
        Instantiate(ResourceManager.GetGameObject("Explosion1"), transform.position, transform.rotation);

        if (addScoreToPlayer)
        {
            Player.PlayerScore += ScoreToAdd;
        }
        Destroy(AliveGameObject);

        DeadGameObject.SetActive(true);
        BoxCollider2D.enabled = false;
        _enabledDeadEffects = true;
    }

    public void SilentDestory()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        enabled = false;
        Debug.Log(enabled);
    }

    private void OnBecameVisible()
    {
        enabled = true;
        Debug.Log(enabled);
    }
}
