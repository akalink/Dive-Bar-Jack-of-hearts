
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace akaUdon
{
    public class SlideUnderBar : UdonSharpBehaviour
    {
        [SerializeField] private string colliderName = "headtracker";
        [SerializeField] private Collider _myCollider;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains(colliderName))
            {
                _myCollider.enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains(colliderName))
            {
                _myCollider.enabled = true;
            }
        }
    }
}
