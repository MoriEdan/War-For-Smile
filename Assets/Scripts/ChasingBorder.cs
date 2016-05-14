using UnityEngine;
using System.Collections;
using Helpers;

public class ChasingBorder : Border
{
	// Use this for initialization
    protected override void Start ()
    {
        ResourceManager.ChasingBorderSpeed = 1.0f;
    }
	
	// Update is called once per frame
    protected override void Update ()
    {
        if (ResourceManager.isDoingSetup)
        {
            return;
        }

        var movement = transform.position;
        movement.x += ResourceManager.ChasingBorderSpeed*Time.deltaTime;

        transform.position = movement;
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        var obstacle = coll.gameObject.GetComponent<Obstacle>();
        if (obstacle)
        {
            //obstacle.SilentDestory();
            obstacle.DestoryOnDemand(false);
        }
    }
}
