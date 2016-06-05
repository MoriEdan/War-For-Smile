using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Helpers;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Entity
{
    public bool EnableSpawnScreen = true;

    public bool IsAlive;
    private bool _hasPlayerExploded = false;

    private SpriteRenderer _spriteRenderer;
    private EmotionManager _emotionManager;

    public GameObject CrosshairSphere;
    public GameObject CrosshairObject;

    public float WeaponUseDelay;
    private float _currentWeaponUseDelay;

    public Slider EmotionMetterSlider;
    private float _emotionMetterValue = 100.0f;

    private AmmoType _currentAmmoType = AmmoType.Neutral;
    private float _firedFunBullets = 0;
    private float _firedSadBullets = 0;
    private float _firedNeutralBullets = 0;

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

    private ScoreHandler _scoreHandler;
    public Text ScoreText;
    private float _playerScore;
    public float PlayerScore
    {
        get { return _playerScore; }
        set
        {
            _playerScore = value;
            _scoreHandler.TotalPlayerPoints = _playerScore;
            ScoreText.text = string.Format("Score:{0}", _playerScore.ToString("F0"));
        }
    }

    public GameObject WebcamView;
    public GameObject PauseScreen;


    protected override void Start()
    {
        _scoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
        PlayerScore = 0.0f;
        _currentHealth = MaxHealth;
        IsAlive = true;
        //transform.position = SpawnPoint.transform.position;

        if (EnableSpawnScreen)
        {
            SpawnScreen.ActiveSpawnScreen(SpawnDelay);
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("No Sprite Renderer");
        }

        _emotionManager = GetComponentInChildren<EmotionManager>();
        if (_emotionManager == null)
        {
            Debug.LogError("No Emotion Manager");
        }
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

        if (_currentHealth <= 0.0f && !_hasPlayerExploded)
        {
            PlayDeadSequence();
        }
    }

    public void Shoot()
    {
        if (ReadyToFire())
        {
            _currentWeaponUseDelay = 0.0f;

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;

            angle += Random.Range(-2.9f, 2.9f);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            CrosshairSphere.transform.rotation = q;
            var spawnRotation = CrosshairSphere.transform.rotation;
            var spawnPosition = CrosshairObject.transform.position;

            var bullet = ((GameObject)Instantiate(GetCurrentBulletPrefab(), spawnPosition, spawnRotation)).GetComponent<Bullet>();
            if (_currentExplosionDelay >= ExplosionDelay)
            {
                bullet.ShouldCreateExplosion = true;
                _currentExplosionDelay = 0.0f;
            }

            _currentHeat += GunHeatingRate;
            HeatBar.value = _currentHeat;

            if (!IsNeutralAmmoType())
            {
                UpdateAmmoState(-10);
            }
        }
    }

    private bool ReadyToFire()
    {
        // can fire if:
        // 1 - weapon use delay passed
        // 2 - weapon is not overheated
        // 3 - have emotion ammo if emotion bullet selected
        // 4 - emotion manager is not loading ammo if emotion bullet selected
        return (_currentWeaponUseDelay >= WeaponUseDelay) 
            && _currentHeat < HeatBar.maxValue 
            && ((_emotionMetterValue > 0.0f) || IsNeutralAmmoType())
            && (!_emotionManager.IsLoadingEmotions || IsNeutralAmmoType());
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var entity = coll.gameObject;

        var border = entity.GetComponent<Border>();
        if (border)
        {
            ApplyDamage(9999, false);
            return;
        }

        var bossBody = entity.GetComponent<BossBodyPart>();
        if (bossBody)
        {
            ApplyDamage(9999, false);
            return;
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        var obscale = coll.gameObject.GetComponent<Obstacle>();
        if (obscale)
        {
            ApplyDamage(obscale.DamageFromCollision);
            return;
        }

        var chasingBorder = coll.gameObject.GetComponent<ChasingBorder>();
        if (chasingBorder)
        {
            ApplyDamage(99999.0f);
            return;
        }

    }

    public void ApplyDamage(float damage, bool delayed = true)
    {
        if ((_currentDamageApplyDelay >= DamageApplyDelay) || !delayed)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0.0f;
                IsAlive = false;
            }

            SetDisplayHealth((_currentHealth/MaxHealth)*100.0f);
            _currentDamageApplyDelay = 0.0f;
        }
    }

    public void SetCurrentHeatBar(float amount)
    {
        _currentHeat = amount;
        HeatBar.value = _currentHeat;
    }

    private void SetDisplayHealth(float health)
    {
        HealthText.text = string.Format("Health:{0}%", health.ToString("F0"));
    }

    private void PlayDeadSequence()
    {
        _hasPlayerExploded = true;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.enabled = false;
        }

        Instantiate(ResourceManager.GetGameObject("PlayerExplosion"), transform.position, transform.rotation);
    }

    private void UpdateAmmoState(float amount)
    {
        switch (_currentAmmoType)
        {
                case AmmoType.Neutral:
                _firedNeutralBullets++;
                break;

                case AmmoType.Fun:
                _firedFunBullets++;
                UpdateEmotionMeterSlider(amount);
                break;

                case AmmoType.Sad:
                _firedSadBullets++;
                UpdateEmotionMeterSlider(amount);
                break;
        }
    }

    public void SetCurrentAmmoType(AmmoType type)
    {
        _currentAmmoType = type;
        switch (_currentAmmoType)
        {
            case AmmoType.Neutral:
                _spriteRenderer.color = Color.white;
                break;

            case AmmoType.Fun:
                _spriteRenderer.color = Color.green;
                break;

            case AmmoType.Sad:
                _spriteRenderer.color = Color.red;
                break;

            default:
                _spriteRenderer.color = Color.cyan;
                Debug.LogWarning("Default Ammo Type");
                break;
        }
    }

    private bool IsNeutralAmmoType()
    {
        return _currentAmmoType == AmmoType.Neutral;
    }

    public void UpdateEmotionMeterSlider(float amount)
    {
        _emotionMetterValue += amount;
        _emotionMetterValue = Mathf.Clamp(_emotionMetterValue, 0.0f, 100.0f);

        EmotionMetterSlider.value = _emotionMetterValue;
    }

    public void RefreshEmotionAmmo(float amount)
    {
        UpdateAmmoState(+amount);
    }

    private GameObject GetCurrentBulletPrefab()
    {
        switch (_currentAmmoType)
        {
            case AmmoType.Neutral:
                return ResourceManager.GetGameObject("PlayerWhiteBullet");

            case AmmoType.Fun:
                return ResourceManager.GetGameObject("PlayerGreenBullet");

            case AmmoType.Sad:
                return ResourceManager.GetGameObject("PlayerRedBullet");

            default:
                Debug.LogWarning("Default Bullet Type");
                return ResourceManager.GetGameObject("PlayerWhiteBullet");
        }
    }
}
