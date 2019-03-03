using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class InteractableObject : MonoBehaviour
{
    public Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        Renderer rend = GetComponent<Renderer>();



        bool isPressed = OVRInput.Get(OVRInput.Button.One, controller);
        

        if (isPressed)
        {
            Debug.Log("Pressed");
            rend.material.SetColor("_Color", Color.green);
        }
        else
        {
            rend.material.SetColor("_Color", Color.red);
        }

    }
}
