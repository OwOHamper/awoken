using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class LightFlickering : MonoBehaviour
{

    public List<UnityEngine.Rendering.Universal.Light2D> lightList;
    public string waveFunction = "noise"; // possible values: sin, tri(angle), sqr(square), saw(tooth), inv(verted sawtooth), noise (random)
    public float Base = 0.0f; // start
    public float amplitude = 1.0f; // amplitude of the wave
    public float phase = 0.0f; // start point inside on wave cycle
    public float frequency  = 0.5f; // cycle frequency per second
 
    // Keep a copy of the original color
    private List<Color> originalColorList;
 
 
    float EvalWave () {
        float x = (Time.time + phase)*frequency;
        float y;
        
        x = x - Mathf.Floor(x); // normalized value (0..1)
        
        if (waveFunction=="sin") {
            y = Mathf.Sin(x*2*Mathf.PI);
        }
        else if (waveFunction=="tri") {
            if (x < 0.5)
            y = 4.0f * x - 1.0f;
            else
            y = -4.0f * x + 3.0f;  
        }    
        else if (waveFunction=="sqr") {
            if (x < 0.5)
            y = 1.0f;
            else
            y = -1.0f;  
        }    
        else if (waveFunction=="saw") {
            y = x;
        }    
        else if (waveFunction=="inv") {
            y = 1.0f - x;
        }    
        else if (waveFunction=="noise") {
            y = 1f - (Random.value*2);
        }
        else if (waveFunction=="custom") {
            float firstPart = (float)Math.Min(Math.Abs(-0.1f * Math.Tan(2f * Math.PI * -0.05f) + 1.1f), 10f);
            float secondPart = (float)Math.Abs(Math.Sin(2f * Math.PI * 59f * -0.05f) + 1.1f);

            return (float)Math.Min(firstPart * secondPart, 3f);
        }
        else {
            y = 1.0f;
        }        
        return (y*amplitude)+Base;     
    }
 
    // Start is called before the first frame update
    void Start()
    {
        originalColorList = new List<Color>();
        for (int i = 0; i < lightList.Count; i++) {
            originalColorList.Add(lightList[i].color);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lightList.Count; i++)
        {
            lightList[i].color = originalColorList[i] * EvalWave();
        }
    }

}
