using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class MainMenu : MonoBehaviour
{
    public RawImage ScanDisplay;
    public Text ResultText;
    
    private WebCamTexture camTexture;

    void Start()
    {
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        ScanDisplay.texture = camTexture;
    }
    
    void Update()
    {
        if (camTexture.isPlaying)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                if (result != null)
                {
                    SpaceRaceNetworkManager.Instance.networkAddress = result.Text;
                    SpaceRaceNetworkManager.Instance.StartClient();
                    ResultText.text = result.Text;
                    camTexture.Stop();
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
        }
    }

    public void StartScanning()
    {
        camTexture.Play();
    }

    public void StopScanning()
    {
        camTexture.Stop();
    }

    public void OnHostClicked()
    {
        SpaceRaceNetworkManager.Instance.StartServer();
    }
}
