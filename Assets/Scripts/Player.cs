using UnityEngine;
using System.Collections;
using Helpers;

public class Player : Entity
{
    private float _currentWeaponUseDelay;

    public float ExplosionDelay;
    private float _currentExplosionDelay;

    protected override void Start ()
    {
        
    }
	
    protected override void Update ()
    {
        _currentWeaponUseDelay += Time.deltaTime;
        _currentExplosionDelay += Time.deltaTime;
    }

    public void Shoot()
    {
        if (ReadyToFire())
        {
            _currentWeaponUseDelay = 0.0f;

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle += Random.Range(-2.9f, 2.9f);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            var offset = transform.rotation*new Vector3(0.3f, 0.0f, 0f);
            var bullet = ((GameObject) Instantiate(ResourceManager.GetGameObject("PlayerDoubleBullet"), transform.position + offset, q)).GetComponent<Bullet>();
            if (_currentExplosionDelay >= ExplosionDelay)
            {
                bullet.ShouldCreateExplosion = true;
                _currentExplosionDelay = 0.0f;
            }
        }
    }

    private bool ReadyToFire()
    {
        return _currentWeaponUseDelay >= WeaponUseDelay;
    }
}
