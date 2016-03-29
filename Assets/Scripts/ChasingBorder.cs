using UnityEngine;
using System.Collections;
using Helpers;

public class ChasingBorder : Entity
{
    public float Speed = 1.0f;

	// Use this for initialization
    protected override void Start ()
    {
        
    }
	
	// Update is called once per frame
    protected override void Update ()
    {
        if (ResourceManager.isDoingSetup)
        {
            return;
        }

        var movement = transform.position;
        movement.x += Speed*Time.deltaTime;

        transform.position = movement;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var obstacle = coll.gameObject.GetComponent<Obscale>();
        if (obstacle)
        {
            //obstacle.SilentDestory();
            obstacle.DestoryOnDemand(false);
        }
    }
}
