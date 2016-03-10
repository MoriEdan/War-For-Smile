using UnityEngine;
using System.Collections;

public class MovingBorder : Entity
{
    public float Speed = 1.0f;

	// Use this for initialization
    protected override void Start ()
    {
	
	}
	
	// Update is called once per frame
    protected override void Update ()
    {
        var movement = transform.position;
        movement.x += Speed*Time.deltaTime;

        transform.position = movement;
    }
}
