using UnityEngine;
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

    public float BossMaxHealth;
    public float BossHealth;
    public float RotationRate;

    public float BossWeaponUseDelay;
    private float _currentWeaponDelay = 0.0f;
    public float BossSuperWeaponUseDelay;
    private float _currentSuperWeaponDelay = 0.0f;

    public float ExplosionDelay;
    private float _currentExplosionDelay;

    private bool _hasBossExploded = false;

    void Start ()
	{
        _cannons = CannonHolder.GetComponentsInChildren<BossCannon>();

        BossMaxHealth = BossHealth;
        BossHealthSlider.maxValue = BossHealth;
        BossHealthSlider.value = BossHealth;
        BossHealthSlider.gameObject.SetActive(true);

        DestoryAllObstacles();

	}
	
	void Update ()
	{
	    if (ResourceManager.IsDoingSetup)
	    {
	        return;
	    }

	    _currentExplosionDelay += Time.deltaTime;

	    BossBody.transform.Rotate(Vector3.forward * (RotationRate * Time.deltaTime));
	    UpdateBossShooting();

	    if (BossHealth <= 0.0f && !_hasBossExploded)
	    {
	        PlayDeadSequence();
	    }

	}

    private void PlayDeadSequence()
    {
        _hasBossExploded = true;

        this.gameObject.SetActive(false);

        Instantiate(ResourceManager.GetGameObject("BossExplosion"), transform.position, transform.rotation);
    }

    public void UpdateBossHealth(float amout)
    {
        BossHealth -= amout;
        BossHealth = Mathf.Clamp(BossHealth, 0.0f, BossMaxHealth);
        BossHealthSlider.value = BossHealth;

        if (BossHealth <= 0.0f)
        {
            BossHealth = 0.0f;
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

    private void DestoryAllObstacles()
    {
        var objs = GameObject.FindObjectsOfType<Obstacle>();
        foreach (var obstacle in objs)
        {
            obstacle.SilentDestory();
        }
    }
}
