
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class lowPassCheck : UdonSharpBehaviour
{
    public AudioLowPassFilter filter;
    public Transform position;
    public Transform player;
    public float maxDistance = 16;
    private int off = 22000;
    public float cutoff = 10;
    void Start()
    {
        
    }

    private void Update()
    {
        float dist = Vector3.Distance(player.position, position.position);
        
        dist -= cutoff;
        
        if (dist < 0)
        {
            dist = 0;
            return;
        }

        dist += cutoff;

        dist = Mathf.Abs(dist - maxDistance);
        //Debug.Log("Distance is: " + dist);
        dist /= 6;
        dist *= off;
       // Debug.Log("Multi is: " + dist);
        filter.cutoffFrequency = dist;


    }
}
