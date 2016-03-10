using UnityEngine;
using System.Collections;

public class ScrollUV : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Material _material;

    public float Parralax = 2f;

	void Start ()
	{
	    _meshRenderer = GetComponent<MeshRenderer>();
	    if (_meshRenderer)
	    {
	        _material = _meshRenderer.material;
	    }
	}
	
	void Update ()
	{
	    var offset = _material.mainTextureOffset;

	    offset.x = transform.position.x/transform.localScale.x/Parralax;
	    offset.y = transform.position.y/transform.localScale.y/Parralax;

        _material.mainTextureOffset = offset;
	}
}
