using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWebCam : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        WebCamTexture webCam = new WebCamTexture();
        rawImage.texture = webCam;
        rawImage.material.mainTexture = webCam;
        webCam.Play();
    }
}
