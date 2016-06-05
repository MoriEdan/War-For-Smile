using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour
{
    public Outline TextOutline;

    private void Start ()
    {
	    
	}
    
    private void Update ()
    {
        if (TextOutline != null)
        {
            TextOutline.effectColor = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time, 1));
        }
	}
}
