using UnityEngine;
using System.Collections;
using Helpers;
using UnityEditor;
using UnityEngine.UI;

public class BossFight : MonoBehaviour
{
    public Player Player;

    public Slider BossHealthSlider;
    public GameObject BossBody;
    public GameObject CannonHolder;
    private BossCannon[] _cannons;

    public float BossHealth;
    public float RotationRate;

    public float BossWeaponUseDelay;
    private float _currentWeaponDelay = 0.0f;

	void Start ()
	{
        _cannons = CannonHolder.GetComponentsInChildren<BossCannon>();
        Debug.Log(_cannons.Length);
	}
	
	void Update ()
	{
	    BossBody.transform.Rotate(Vector3.forward * (RotationRate * Time.deltaTime));
	    UpdateBossShooting();

	}

    public void UpdateBossHealth(float amout)
    {
        BossHealth -= amout;

        if (BossHealth <= 0.0f)
        {
            Debug.Log("Wygrales");
            Destroy(this);
        }
    }

    private void UpdateBossShooting()
    {
        _currentWeaponDelay += Time.deltaTime;
        if (_currentWeaponDelay >= BossWeaponUseDelay)
        {
            Fire();
            _currentWeaponDelay = 0.0f;
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
        }
    }
}
