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
        if (Range > 0)
        {
            var positionChange = Time.deltaTime*Velocity;
            Range -= positionChange;

            transform.position += (positionChange*transform.right);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var entity = coll.gameObject.GetComponent<Entity>();
        if (entity == null)
        {
            return;
        }

        if (entity.ObjectName == "PlayerDoubleBullet" || entity.ObjectName == "PlayerShip")
        {
            return;
        }

        if (entity.ObjectName == "Border" || entity.ObjectName == "ChasingBorder")
        {
            CreateExplosionOnBorder();
            Destroy(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
        
        var obscale = coll.gameObject.GetComponent<Obstacle>();
        if (obscale)
        {
            obscale.Health -= Damage;
            CreateExplosion(obscale.Color);
        }
    }

    private void CreateExplosion(Color? color)
    {
        if (ShouldCreateExplosion)
        {
            var gO = (GameObject)Instantiate(ResourceManager.GetGameObject("Explo"), transform.position, transform.rotation);
            if (color.HasValue)
            {
                var ps = gO.gameObject.GetComponent<ParticleSystem>();
                if (ps)
                {
                    ps.startColor = color.Value;
                }
            }
        }
    }

    private void CreateExplosionOnBorder()
    {
        if (ShouldCreateExplosion)
        {
            Instantiate(ResourceManager.GetGameObject("Explo1"), transform.position, transform.rotation);
        }
    }
}
