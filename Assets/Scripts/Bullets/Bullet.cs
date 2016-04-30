using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Helpers;

public class Bullet : Entity
{
    public float Velocity;
    public float Range;
    public float Damage;
    public AmmoType BulletType;

    public bool ShouldCreateExplosion = false;

    protected override void Start ()
    {
	    
	}

    protected override void Update ()
    {
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        
    }

    protected virtual void CreateExplosion(Color? color)
    {
        
    }

    protected virtual void CreateExplosionOnBorder()
    {
        
    }
}
