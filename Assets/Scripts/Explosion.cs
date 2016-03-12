using UnityEngine;

public class Explosion : Entity
{
    protected ParticleSystem ParticleSystem;

    protected override void Start ()
	{
	    ParticleSystem = GetComponent<ParticleSystem>();
	}
	
    protected override void Update ()
    {
	    if (!ParticleSystem.IsAlive())
	    {
	        Destroy(gameObject);
	    }
	}
}
