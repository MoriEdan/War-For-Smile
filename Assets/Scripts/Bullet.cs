using UnityEngine;
using System.Collections;
using Helpers;

public class Bullet : Entity
{
    public float Velocity;
    public float Range;
    public float Damage;

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

        if (entity.ObjectName == "PlayerDoubleBullet")
        {
            return;
        }

        if (entity.ObjectName == "Border")
        {
            CreateExplosionOnBorder();
            Destroy(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
        
        var obscale = coll.gameObject.GetComponent<Obscale>();
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
