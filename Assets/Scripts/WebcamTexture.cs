using UnityEngine;
using System.Collections;
using Affdex;
using Helpers;
using UnityEngine.UI;

public class WebcamTexture : MonoBehaviour
{
    public Affdex.CameraInput CameraInput;

    private RawImage _rawImage;

    private void Start ()
    {
        if (!AffdexUnityUtils.ValidPlatform())
        {
            Debug.LogError("Unsupported Affectiva Platform");
            return;
        }

        _rawImage = GetComponent<RawImage>();
    }

    private void Update ()
    {
        if (ResourceManager.IsDoingSetup)
        {
            return;
        }

        var texture = CameraInput.Texture;

        if (texture == null)
        {
            Debug.LogError("No Texture from CameraInput");
            return;
        }

	    _rawImage.texture = texture;

        /*var wscale = (float)texture.width / (float)texture.height;
	    var scaleY = transform.localScale.y*0.5f;
        transform.localScale = new Vector3(scaleY , scaleY, 1);*/
    }
}
