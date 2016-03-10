using UnityEngine;
using System.Collections;
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
        UpdateMovement();
        UpdateShooting();
    }

    private void UpdateMovement()
    {
        var velocity = PlayerSpeed*UserInput.GetMovementDirection()*Time.deltaTime;
        transform.position += velocity;

        transform.position = new Vector3((transform.position.x<0)?0: transform.position.x, 
            Mathf.Clamp(transform.position.y, -2f, 2f));

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
}
