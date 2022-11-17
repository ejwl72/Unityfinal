using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight  : MonoBehaviour
{
    int Day;
    [SerializeField] private float secondPerRealTimeSecound;

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc;

    [SerializeField] private float nightFogDensity;
    private float dayFogDensity;
    private float currentFogDensity;
    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
        InvokeRepeating("dayplus", 1.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

        angle();

        if (isNight) {
            if(currentFogDensity <= nightFogDensity)
            currentFogDensity += 0.001f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
                currentFogDensity -= 0.001f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }

        
    }
    void angle() {


        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecound * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
        {
            if (transform.eulerAngles.x >= 340)
            {
                isNight = false;
            }
            else
                isNight = true;
        }
        if (isNight)
        {
            secondPerRealTimeSecound = 3;
            transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecound * Time.deltaTime);
        }
        else
        {
            secondPerRealTimeSecound = 50;
            transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecound * Time.deltaTime);
        }

    }
    void dayplus() {
        Day++;
        Debug.Log(Day);
    }
}
