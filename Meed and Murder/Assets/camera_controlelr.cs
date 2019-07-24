using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controlelr : MonoBehaviour
{

    private float shakeAmount = 0;
    private Vector2 originalPoss;
    public GameObject cam;


    void Awake()
    {
        originalPoss = cam.transform.position;
    }

    public void cameraShake(float shakeTime, float shakeStrenth)
    {
        shakeAmount = shakeStrenth;

        InvokeRepeating("startShake", 0, 0.01f); //kallar funktionen som ger en ny possition till cameran varje 0,01sekunder
        Invoke("stopShake", shakeTime);
    }

    private void startShake()
    {
        if (shakeAmount > 0)
        {

            Vector3 shakePossision = cam.transform.position;

            float shakeAmountOffsetX = Random.value * shakeAmount * 2 - shakeAmount;      // uträkning av bra slumpvärden för nya possitioner som hittades online 
            float shakeAmountOffsetY = Random.value * shakeAmount * 2 - shakeAmount;

            shakePossision.x += shakeAmountOffsetX;
            shakePossision.y += shakeAmountOffsetY;

            cam.transform.position = shakePossision;
        }
    }

    private void stopShake()
    {
        CancelInvoke("startShake");

        cam.transform.localPosition = new Vector3(0,0,-1);
    }

}
