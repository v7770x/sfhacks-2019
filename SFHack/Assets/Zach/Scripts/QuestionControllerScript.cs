//This script controls the UI for questions, checking what button is being pressed.

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static OVRInput;

public class QuestionControllerScript : MonoBehaviour
{
    public int promptNum = 0;
    public string startingPrompt;
    public string userName;
    public Text prompt;



    // Start is called before the first frame update
    void Start()
    {
        startingPrompt = "Hello, welcome to Mindful: A VR Experience. \nPress and hold the main button to talk. \nReady? Say 'Yes' or 'No'"; 

        prompt.text = startingPrompt;
        PlaySpeech(startingPrompt);

    }

    public bool triedName = false;
    public bool isMindful = false;
    public bool feelGood;
    public bool reading = false;

    public string mindfulness = ("Think of something that happens every day more than once; something you take for granted, like opening a door, for example. " +
                                "At the very moment you touch the doorknob to open the door, stop for a moment and be mindful of where you are, how you feel " +
                                "in that moment and where the door will lead you. " + "Similarly, the moment you open your computer to start work, take a moment " +
                                "to appreciate the hands that enable this process and the brain that facilitates your understanding of how to use the computer. ");

    // Update is called once per frame
    void Update()
    {
        string resp = GetResponse();
        Text prompt = GetComponent<Text>();
        switch (promptNum)
        {
            case 0:
                if(triedName == true)
                {
                    prompt.text = "What is your name?";
                    if (!(userName = resp).Contains(""))
                    {
                        promptNum = 1;
                    }

                }
                else if (resp.Contains("yes"))
                {
                    PlaySpeech("Great, let's get started. First, what is your name?");
                    prompt.text = "What is your name?";
                    promptNum = 1;
                    ResetT2S();
                    triedName = true;
                }
                else if (resp.Contains("no"))
                {
                    PlaySpeech("I'm sorry to hear that, feel free to reach out when you change your mind.");
                    ResetT2S();
                    userName = null;
                }
                else if (!resp.Contains("yes") || !resp.Contains("no"))
                {
                    VoiceError();
                }
                break;
            case 1:
                
                if ((userName = resp).Contains(""))
                {
                    PlaySpeech("Did you say " + userName + "?");
                    prompt.text = userName + "?";
                    promptNum = 2;
                    ResetT2S();
                }
                break;
            case 2:
                if (resp.Contains("yes"))
                {
                    PlaySpeech("Great to meet you, " + userName + ". How are you doing today?");
                    prompt.text = "";
                    ResetT2S();
                    promptNum += 1;
                }
                else if (resp.Contains("no"))
                {
                    PlaySpeech("Let's try that again. What is your name?");
                    ResetT2S();
                    promptNum = 1;
                }
                else if (!resp.Contains("yes") || !resp.Contains("no"))
                {
                    VoiceError();
                }
                break;
            case 3:
                if(resp.Contains("bad") || resp.Contains("not") || resp.Contains("shitty") || resp.Contains("sad") || resp.Contains("awful") || resp.Contains("terrible")){
                    promptNum++;
                    feelGood = false;
                }
                else if(resp.Contains("great") || resp.Contains("okay") || resp.Contains("awesome") || resp.Contains("alright") || resp.Contains("amazing") || resp.Contains("cool") || resp.Contains("happy") || resp.Contains("good"))
                {
                    promptNum++;
                    feelGood = true;
                }
                else if(!resp.Contains("bad") || !resp.Contains("not") || !resp.Contains("shitty") || !resp.Contains("sad") || !resp.Contains("awful") || !resp.Contains("terrible") || !resp.Contains("great") || !resp.Contains("okay") || !resp.Contains("awesome") || !resp.Contains("alright") || !resp.Contains("amazing") || !resp.Contains("cool") || !resp.Contains("happy"))
                {
                    VoiceError();
                }
                break;
            case 4:
                if (reading == false)
                {
                    reading = true;
                    if (!isMindful == false)
                    {
                        PlaySpeech("I'm sorry to hear that. When I'm feeling sad, I like to think about the world around me. " + mindfulness + "I'm sorry you feel down in the dumps today, " + userName + ". I hope that this experience has helped brighten your day. Thank you for using Mindful: A VR Experience");
                    }
                    else if (!isMindful)
                    {
                        PlaySpeech("That's good to hear. Say, have you ever noticed how beautiful the world is? " + mindfulness + "I'm glad you're feeling happy today, " + userName + "! It's nice to talk with someone about the things I'm passionate about! Thank you for using Mindful: A VR Experience");
                    }
                }
                break;
                


        }


    }

    void ResetT2S()
    {
        GetComponent<GoogleVoiceSpeech>().finalS2T = null;
    }

    string GetResponse()
    {
        return GetComponent<GoogleVoiceSpeech>().finalS2T;
    }

    void PlaySpeech(string speech)
    {
        GetComponent<TextToSpeech>().textToConvert = speech;
        GetComponent<TextToSpeech>().hiThere();
        GetComponent<TextToSpeech>().textToConvert = null;
        ResetT2S();
    }
    
    void VoiceError()
    {
        PlaySpeech("Sorry, I didn't quite understand that. Can you please repeat what you have said?");
        ResetT2S();
    }

    
}