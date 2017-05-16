using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class ServerMenu : MonoBehaviour
{
    public Image QRCode;

    void Start()
    {
        if (Network.isClient)
        {
            Destroy(gameObject);
        }
        else
        {
            Texture2D code = generateQR(Network.player.ipAddress);
            QRCode.sprite = Sprite.Create(code, new Rect(0.0f, 0.0f, code.width, code.height), new Vector2(0.5f, 0.5f));
        }
    }

    #region QRCode
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    private Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
    #endregion
}
