using UnityEngine;
using System.Collections;
using Helpers;
using UnityEngine.UI;

public class Player : Entity
{
    private float _currentWeaponUseDelay;

    public float ExplosionDelay;
    private float _currentExplosionDelay;

    public GameObject SpawnPoint;
    public CounterScreen SpawnScreen;
    public int SpawnDelay = 3;

    public Slider HeatBar;
    private float _currentHeat = 0.0f;
    public float GunCalmingRate = 10.0f;
    public float GunHeatingRate = 1.7f;

    public Text HealthText;
    public float MaxHealth = 100.0f;
    private float _currentHealth;
    public float DamageApplyDelay;
    private float _currentDamageApplyDelay;

    public Text ScoreText;

    private float _playerScore;
    public float PlayerScore
    {
        get { return _playerScore; }
        set
        {
            _playerScore = value;
            ScoreText.text = string.Format("Score:{0}", _playerScore.ToString("F0"));
        }
    }

    protected override void Start()
    {
        _currentHealth = MaxHealth;
        //ResourceManager.isDoingSetup = true;
        //SpawnScreen.ActiveSpawnScreen(SpawnDelay);
    }

    protected override void Update()
    {
        _currentWeaponUseDelay += Time.deltaTime;
        _currentExplosionDelay += Time.deltaTime;
        _currentDamageApplyDelay += Time.deltaTime;

        if (_currentHeat > 0)
        {
            _currentHeat -= GunCalmingRate*Time.deltaTime;
        }
        else
        {
            _currentHeat = 0.0f;
        }
        HeatBar.value = _currentHeat;

    }

    public void Shoot()
    {
        if (ReadyToFire() && _currentHeat < HeatBar.maxValue)
        {
            _currentWeaponUseDelay = 0.0f;

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;

            angle += Random.Range(-2.9f, 2.9f);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            var offset = transform.rotation*new Vector3(0.3f, 0.0f, 0f);
            var bullet = ((GameObject) Instantiate(ResourceManager.GetGameObject("PlayerDoubleBullet"), transform.position + offset, q)).GetComponent<Bullet>();
            if (_currentExplosionDelay >= ExplosionDelay)
            {
                bullet.ShouldCreateExplosion = true;
                _currentExplosionDelay = 0.0f;
            }

            _currentHeat += GunHeatingRate;
            HeatBar.value = _currentHeat;
        }
    }

    private bool ReadyToFire()
    {
        return _currentWeaponUseDelay >= WeaponUseDelay;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var entity = coll.gameObject.GetComponent<Entity>();
        if (entity == null || entity.ObjectName == "PlayerDoubleBullet")
        {
            return;
        }

    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        var obscale = coll.gameObject.GetComponent<Obscale>();
        if (obscale)
        {
            ApplyDamage(obscale.DamageFromCollision);
            return;
        }

        if (coll.gameObject.name == "StartBorder")
        {
            //Debug.Log(coll.gameObject.name);
            ApplyDamage(11.0f);
        }
    }

    private void ApplyDamage(float damage)
    {
        if (_currentDamageApplyDelay >= DamageApplyDelay)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _currentHealth = 0.0f;
            }

            SetDisplayHealth((_currentHealth/MaxHealth)*100.0f);
            Debug.Log(damage);
            _currentDamageApplyDelay = 0.0f;
        }
    }

    private void SetDisplayHealth(float health)
    {
        HealthText.text = string.Format("Health:{0}%", health.ToString("F0"));
    }
}
