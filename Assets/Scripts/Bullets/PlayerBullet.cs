using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Helpers;

public class PlayerBullet : Bullet
{
    public float DamageMultiplier = 1.0f;

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

        var border = entity.GetComponent<Border>();
        if (border)
        {
            CreateExplosionOnBorder();
            Destroy(this.gameObject);
            return;
        }      

        var obscale = entity.GetComponent<Obstacle>();
        if (obscale)
        {
            obscale.Health -= Damage;
            Destroy(this.gameObject);
            CreateExplosion(obscale.Color);
            return;
        }

        var enemyBullet = entity.GetComponent<EnemyBullet>();
        if (enemyBullet)
        {
            Destroy(entity);
            Destroy(this.gameObject);
        }

        var boss = entity.GetComponent<BossBodyPart>();
        if (boss)
        {
            Destroy(this.gameObject);
            switch (boss.BossPart)
            {
                    case Assets.Scripts.Helpers.BossBodyPart.Left:
                    switch (BulletType)
                    {
                        case AmmoType.Neutral:
                            boss.BossFight.UpdateBossHealth(Damage);
                            break;
                        case AmmoType.Fun:
                            boss.BossFight.UpdateBossHealth(-Damage * DamageMultiplier);
                            break;
                        case AmmoType.Sad:
                            boss.BossFight.UpdateBossHealth(Damage * DamageMultiplier);
                            break;
                        default:
                            Debug.Log("Invalid Bullet Type");
                            break;
                    }
                    break;

                case Assets.Scripts.Helpers.BossBodyPart.Central:
                    boss.BossFight.UpdateBossHealth(-Damage * DamageMultiplier);
                    break;

                case Assets.Scripts.Helpers.BossBodyPart.Right:
                    switch (BulletType)
                    {
                        case AmmoType.Neutral:
                            boss.BossFight.UpdateBossHealth(Damage);
                            break;
                        case AmmoType.Fun:
                            boss.BossFight.UpdateBossHealth(Damage * DamageMultiplier);
                            break;
                        case AmmoType.Sad:
                            boss.BossFight.UpdateBossHealth(-Damage * DamageMultiplier);
                            break;
                        default:
                            Debug.Log("Invalid Bullet Type");
                            break;
                    }
                    break;

                default:
                    Debug.Log("Invalid Boss Part");
                    break;
            }
            CreateExplosion(Color.green);
        }
    }

    protected override void CreateExplosion(Color? color)
    {
        if (ShouldCreateExplosion)
        {
            var gO = (GameObject) Instantiate(ResourceManager.GetGameObject("Explosion1"), transform.position, transform.rotation);
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

    protected override void CreateExplosionOnBorder()
    {
        if (ShouldCreateExplosion)
        {
            Instantiate(ResourceManager.GetGameObject("Explo1"), transform.position, transform.rotation);
        }
    }
}
