
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace akaUdon
{

    public class micoverride : UdonSharpBehaviour
    {
        [Range(0f, 24f)] [SerializeField] float gain = 5f;

        [Tooltip("Far is the max distance of the sound.")] [Range(0f, 1000f)] [SerializeField]
        float far = 250f;

        [UdonSynced()] private bool allowOverride = false;
        
        private bool _allowState = false;

        [SerializeField] private TextMeshProUGUI owner;


        private void Start()
        {
            owner.text = Networking.GetOwner(gameObject).displayName;
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            owner.text = Networking.GetOwner(gameObject).displayName;
        }

        public void _Toggle()
        {
            _allowState = !_allowState;

            if (_allowState)
            {
                _Allow();
            }
            else
            {
                _Deny();
            }
        }

        public void _Allow()
        {
            if (Networking.LocalPlayer.IsOwner(gameObject))
            {
                allowOverride = true;
                RequestSerialization();
            }
        }

        public void _Deny()
        {
            if (Networking.LocalPlayer.IsOwner(gameObject))
            {
                allowOverride = false;
                RequestSerialization();
            }
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (allowOverride)
            {
                player.SetVoiceDistanceFar(far);
                player.SetVoiceGain(gain);
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            player.SetVoiceDistanceFar(25f);
            player.SetVoiceGain(15f);
        }

        public override void OnDeserialization()
        {
            //this doesn't actually do anything
        }


    }
}
