using UnityEngine;
using UnityEngine.UI;

public class DisplayWebCam : MonoBehaviour
{
    /* PUBLICS */
    public GameObject UniversalWebcam;
    public RawImage rawImage;


    /* PRIVATES */
    private WebCamTexture _webcamTexture;

    
    void Update()
    {
        _webcamTexture = UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture;
        rawImage.texture = _webcamTexture;
    }
}
