using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] float lightTimer = 1.0f;
    void Start()
    {
        InvokeRepeating("BlinkingLight", lightTimer, lightTimer);
    }

    void BlinkingLight()
    {
        GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
    }
}
