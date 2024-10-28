using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RainController : MonoBehaviour
{
    private RainScript2D rainController => GetComponent<RainScript2D>();
    [Range(0.0f, 1.0f)]
    [SerializeField] private float intensity;
    [SerializeField] private float targetIntensity;
    [SerializeField] private float changeRate = .05f;
    [SerializeField] private float maxValue = .2f;
    [SerializeField] private float minValue = .49f;

    [SerializeField] private float chanceToRain = 50;
    [SerializeField] private float rainCheckCoolDown;
    private float rainCheckTimer;
    private bool canChangeIntensity;
    private void Update()
    {
        rainCheckTimer -= Time.deltaTime;
        rainController.RainIntensity = intensity;
        if (Input.GetKeyDown(KeyCode.R))
        {
            canChangeIntensity = true;
        }

        if(canChangeIntensity)
        {
            ChangeIntensity();
        }
    }
    private void CheckForRain()
    {
        if(rainCheckTimer < 0)
        {
            rainCheckTimer = rainCheckCoolDown;
            canChangeIntensity = true;

            if (Random.Range(0,100) < chanceToRain)
            {
                targetIntensity = Random.Range(minValue, maxValue);
            }
            else 
            {
                targetIntensity = 0;
            }
        }
    }
    private void ChangeIntensity()
    {
        if(intensity < targetIntensity)
        {
            intensity += changeRate * Time.deltaTime;

            if(intensity >= targetIntensity)
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }
        if(intensity > targetIntensity) {
            intensity -= changeRate * Time.deltaTime;
            if(intensity <= targetIntensity)
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }
    }
}
