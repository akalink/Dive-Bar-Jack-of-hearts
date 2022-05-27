
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace akaUdon{
public class toggleOnHandPush : UdonSharpBehaviour
{
    public Collider collide;
    public string name;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(name) && other != null)
        {
            collide.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains(name) && other != null)
        {
            collide.enabled = false;
        }
    }
}
}