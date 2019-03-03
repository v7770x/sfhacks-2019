using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlEnvironment : MonoBehaviour
{
    protected float heartRate;
    public GameObject fireBase;

    protected float maxHR = 120;
    protected float leastHR = 60;
    protected float restHR = 80;

    protected int avgHR = 80;
    protected int checksHR = 0;

    protected Color color = new Color(41f, 70f, 216f);

    public Light blueLight;

    // Start is called before the first frame update
    void Start()
    {
        blueLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        fireBase = GameObject.Find("Firebase");
        heartRate = fireBase.GetComponent<GetData>().heartRate;
        checksHR++;



        //Change light based on heart rate
        float change = (heartRate - restHR) / (maxHR - restHR);

        if(change < 0)
        {
            change = 0;
        }
        else if(change > 1)
        {
            change = 1;
        }


        blueLight.intensity = change;
        Debug.Log("Change: " + change);
        

    }
}
