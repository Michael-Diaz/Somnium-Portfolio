using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWebCam : MonoBehaviour
{
    public RawImage footageImage;
    public AspectRatioFitter footageDimensions;

    // Start is called before the first frame update
    void Start()
    {
        WebCamTexture webCam = new WebCamTexture();
        footageImage.texture = webCam;
        footageImage.material.mainTexture = webCam;
        webCam.Play();

        float videoRatio = (float)webCam.width / (float)webCam.height;
        footageDimensions.aspectRatio = videoRatio;
    }
}
