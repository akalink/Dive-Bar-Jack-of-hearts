
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace akaUdon
{
    public class castPositionToPlane : UdonSharpBehaviour
    {
        [SerializeField] private Transform referenceWorld;
        [SerializeField] private Transform worldCenter;
        [SerializeField] private Transform referenceDisplay;
        //[SerializeField] private Transform displayCenter;
        private Vector3 posWorld = Vector3.zero;

        void Start()
            {
        
         }

        private void Update()
        {
            posWorld = new Vector3(referenceWorld.position.x - worldCenter.position.x
                , 0
                , referenceWorld.position.z -worldCenter.position.z);
            
            Debug.Log("Vector X + Y + Z:" + posWorld.ToString());
            referenceDisplay.localPosition = posWorld;
        }
    }
}
