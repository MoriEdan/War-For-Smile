using UnityEngine;
using System.Collections;

public class Explosion : Entity
{
    protected ParticleSystem _particleSystem;

	// Use this for initialization
    protected override void Start ()
	{
	    _particleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
    protected override void Update ()
    {
	    if (!_particleSystem.IsAlive())
	    {
	        Destroy(gameObject);
	    }
	}
}
