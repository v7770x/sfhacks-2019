using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using WWUtils.Audio;



public class TextToSpeech : MonoBehaviour
{

    public GUIText TextBox;

    public AudioSource audioSourceFinal;
    public string textToConvert;
    struct ClipData
    {
        public int samples;
    }

    const int HEADER_SIZE = 44;

    private int minFreq;
    private int maxFreq;

    private bool micConnected = false;

    //A handle to the attached AudioSource
    private AudioSource goAudioSource;

    public string apiKey;

    string url = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?&key=AIzaSyAOViwmf8Y4KGusoMEVhpmSUi6MXkDjGus";

    // Start is called before the first frame update
    void Start()
    {
        //hiThere();
        
    }

    // Update is called once per frame
    public void hiThere()
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {




                string json2 = "{ \"input\": {\"text\":\"" + textToConvert + "\"},\"voice\": {\"languageCode\":\"en-US\"}, \"audioConfig\": {\"audioEncoding\":\"LINEAR16\"}}";

                Debug.Log(json2);
                streamWriter.Write(json2);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Debug.Log(httpResponse);
            
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Log("Response:" + result);
                string temp = (string)result;
                Debug.Log(temp);
                string[] words = temp.Split('"');
                string decodeThis = words[3];
                byte[] decodedBytes = Convert.FromBase64String(decodeThis);
                WAV wav = new WAV(decodedBytes);
                Debug.Log(wav);
                AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                audioClip.SetData(wav.LeftChannel, 0);
                audioSourceFinal.clip = audioClip;
                audioSourceFinal.Play();

            }

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            Debug.Log(resp);

        }


    }
}
