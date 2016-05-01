﻿using UnityEngine;
using System.Collections;
using Helpers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossFight : MonoBehaviour
{
    public Player Player;

    public Slider BossHealthSlider;
    public GameObject BossBody;
    public GameObject CannonHolder;
    public GameObject SuperCannonHolder;
    private BossCannon[] _cannons;

    public float BossHealth;
    public float RotationRate;

    public float BossWeaponUseDelay;
    private float _currentWeaponDelay = 0.0f;
    public float BossSuperWeaponUseDelay;
    private float _currentSuperWeaponDelay = 0.0f;

    public float ExplosionDelay;
    private float _currentExplosionDelay;

    void Start ()
	{
        _cannons = CannonHolder.GetComponentsInChildren<BossCannon>();
        BossHealthSlider.maxValue = BossHealth;
	}
	
	void Update ()
	{
	    _currentExplosionDelay += Time.deltaTime;

	    BossBody.transform.Rotate(Vector3.forward * (RotationRate * Time.deltaTime));
	    UpdateBossShooting();

	}

    public void UpdateBossHealth(float amout)
    {
        BossHealth -= amout;
        BossHealthSlider.value = BossHealth;

        if (BossHealth <= 0.0f)
        {
            Debug.Log("Wygrales");
            SceneManager.LoadScene("Victory");
            Destroy(this);
        }
    }

    private void UpdateBossShooting()
    {
        _currentWeaponDelay += Time.deltaTime;
        _currentSuperWeaponDelay += Time.deltaTime;
        if (_currentWeaponDelay >= BossWeaponUseDelay)
        {
            Fire();
            _currentWeaponDelay = 0.0f;
        }

        if (_currentSuperWeaponDelay >= BossSuperWeaponUseDelay)
        {
            FireCircural();
            _currentSuperWeaponDelay = 0.0f;
        }
    }

    private void Fire()
    {
        foreach (var cannon in _cannons)
        {
            var playerPos = Player.transform.position;
            var dir = playerPos - cannon.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle += Random.Range(-2.9f, 2.9f);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            cannon.transform.rotation = q;
            var spawnRotation = cannon.transform.rotation;
            var spawnPosition = cannon.transform.position;

            var bullet = ((GameObject)Instantiate(ResourceManager.GetGameObject("BossRedBullet"), spawnPosition, spawnRotation)).GetComponent<Bullet>();
            //if (_currentExplosionDelay >= ExplosionDelay)
            {
                bullet.ShouldCreateExplosion = true;
                _currentExplosionDelay = 0.0f;
            }
        }
    }

    private void FireCircural()
    {
        for (var i = 0; i <= 360; i += 5)
        {
            var playerPos = PointOnCircle(20, i, SuperCannonHolder.transform.position);
            var dir = playerPos - SuperCannonHolder.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //angle += Random.Range(-2.9f, 2.9f);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            SuperCannonHolder.transform.rotation = q;
            var spawnRotation = SuperCannonHolder.transform.rotation;
            var spawnPosition = SuperCannonHolder.transform.position;

            var bullet = ((GameObject)Instantiate(ResourceManager.GetGameObject("BossRedBullet"), spawnPosition, spawnRotation)).GetComponent<Bullet>();
            bullet.ShouldCreateExplosion = true;
        }
    }

    private Vector3 PointOnCircle(float radius, float angleInDegrees, Vector3 origin)
    {
        // Convert from degrees to radians via multiplication by PI/180        
        var x = (float)(radius * Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)) + origin.x;
        var y = (float)(radius * Mathf.Sin(angleInDegrees * Mathf.Deg2Rad)) + origin.y;

        return new Vector3(x, y);
    }
}
