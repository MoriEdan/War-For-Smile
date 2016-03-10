using UnityEngine;
using System.Collections;
using Helpers;

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
        if (!ResourceManager.isDoingSetup)
        {
            var movement = transform.position;
            movement.x += Speed*Time.deltaTime;

            transform.position = movement;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(coll.gameObject.name);
    }
}
