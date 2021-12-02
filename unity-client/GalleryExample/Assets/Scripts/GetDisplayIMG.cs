using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetDisplayIMG : MonoBehaviour
{
    // public GameObject rawImage;
    public TMPro.TMP_Text infoLabel;
    public string IPAddress = "localhost";
    public int port = 12572;
    private string info_url_pre = "http://localhost:12572/info/id/";
    private string img_url_pre = "http://localhost:12572/img/id/";

    IEnumerator DlInfo(string addr)
    {
        UnityWebRequest request = UnityWebRequest.Get(addr);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string info = request.downloadHandler.text;
            Debug.Log("DlInfo:: retrieved: " + info);
            infoLabel.text = info;
        }

    }
    IEnumerator DlImage(object[] parms)
    {

        RawImage img = (RawImage)parms[0];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture((string)parms[1]);

        Texture2D texture;
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            img.texture = texture;
            img.rectTransform.sizeDelta = new Vector2((float)texture.width / texture.height, 1);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        info_url_pre = $"http://{IPAddress}:{port}/info/id/";
        img_url_pre = $"http://{IPAddress}:{port}/img/id/";
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LoadImage(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadImage(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadImage(2);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadImage(3);
        }
    }

    public void LoadImage(int id)
    {
        object[] parms = new object[2] { this.GetComponent<RawImage>(), img_url_pre + id.ToString() };
        StartCoroutine("DlInfo", info_url_pre + id.ToString());
        StartCoroutine("DlImage", parms);
    }
}



