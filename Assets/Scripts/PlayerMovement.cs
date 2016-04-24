using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Helpers;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;

    public float PlayerSpeed = 5;
    public bool ChaoticCamera = false;

    private void Start ()
	{
	    _player = GetComponent<Player>();
	}
	
    private void Update ()
    {
        if (_player.IsAlive && !ResourceManager.isDoingSetup)
        {
            UpdateMouse();
            UpdateMovement();
            UpdateShooting();
            UpdateKeyboard();
        }
    }

    private void UpdateMovement()
    {
        var velocity = PlayerSpeed*UserInput.GetMovementDirection()*Time.deltaTime;
        transform.position += velocity;

        //transform.position = new Vector3((transform.position.x<0)?0: transform.position.x, Mathf.Clamp(transform.position.y, -2f, 2f));

        if (ChaoticCamera)
        {
            if (velocity.sqrMagnitude > 0)
            {
                transform.rotation = Quaternion.AngleAxis(velocity.ToAngle(), Vector3.forward);
            }
        }
    }

    private void UpdateShooting()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            _player.Shoot();
        }
    }

    private void UpdateMouse()
    {
        if (Input.GetMouseButton(1))
        {
            _player.RefreshEmotionAmmo(15 * Time.deltaTime);
        }
    }

    private void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _player.SetCurrentAmmoType(AmmoType.Neutral);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _player.SetCurrentAmmoType(AmmoType.Fun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _player.SetCurrentAmmoType(AmmoType.Sad);
        }
    }
}
