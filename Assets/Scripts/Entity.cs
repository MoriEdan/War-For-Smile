﻿using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    public string ObjectName;
    public Sprite Sprite;

    
    protected virtual void Awake()
    {

    }

    protected virtual void Start ()
    {
	    
	}
	
	protected virtual void Update ()
	{

	}

    protected virtual void FixedUpdate()
    {

    }
}
