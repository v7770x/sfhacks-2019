using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{
    public Color lightColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Light lt = GetComponent<Light>();
        GameObject speech = GameObject.Find("Speech");

        string text = speech.GetComponent<GoogleVoiceSpeech>().finalS2T;

        switch (text)
        {
            case "red":
                lt.color = Color.red;
                break;
            case "blue":
                lt.color = Color.blue;
                break;
            case "cyan":
                lt.color = Color.cyan;
                break;
            case "green":
                lt.color = Color.green;
                break;
            case "gray":
            case "grey":
                lt.color = Color.gray;
                break;
            case "yellow":
                lt.color = Color.yellow;
                break;
            case "magenta":
                lt.color = Color.magenta;
                break;
            case "white":
                lt.color = Color.white;
                break;
            case "black":
                lt.color = Color.black;
                break;
        }
    }
}
