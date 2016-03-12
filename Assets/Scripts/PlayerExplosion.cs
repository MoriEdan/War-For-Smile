﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerExplosion : Explosion
{
    protected override void Update ()
    {
        if (!ParticleSystem.IsAlive())
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
	}
}
