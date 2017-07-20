using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class ServerMenu : MonoBehaviour
{
    public RawImage QRCode;
    public RectTransform LeftBackground;
    public RectTransform RightBackground;

    void Start()
    {
        RectTransform qrCodeTrans = QRCode.GetComponent<RectTransform>();
        int minSize = Screen.width < Screen.height ? Screen.width : Screen.height;
        qrCodeTrans.sizeDelta = new Vector2(minSize, minSize);
        Texture2D code = generateQR(Network.player.ipAddress);
        QRCode.texture = code;

        float backgroundWidthPercent = (Screen.width - minSize) / 2f/ Screen.width;
        LeftBackground.anchorMax = new Vector2(backgroundWidthPercent, 1);
        RightBackground.anchorMin = new Vector2(1 - backgroundWidthPercent, 0);

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
