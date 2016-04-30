using UnityEngine;
using System.Collections;
using Helpers;

public class EnemyBullet : Bullet
{

    protected override void Start ()
    {
	
	}
	
    protected override void Update ()
    {
        if (Range > 0)
        {
            var positionChange = Time.deltaTime * Velocity;
            Range -= positionChange;

            transform.position += (positionChange * transform.right);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        var entity = coll.gameObject;

        if (entity.name == "PlayerShip")
        {
            var player = entity.GetComponent<Player>();
            if (player)
            {
                player.ApplyDamage(this.Damage);
                CreateExplosion(Color.white);
                Destroy(this.gameObject);
                return;
            }
        }

        var border = coll.gameObject.GetComponent<Border>();
        if (border)
        {
            Destroy(this.gameObject);
        }
    }

    protected override void CreateExplosion(Color? color)
    {
        if (ShouldCreateExplosion)
        {
            var gO = (GameObject)Instantiate(ResourceManager.GetGameObject("FastExplo"), transform.position, transform.rotation);
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
}
