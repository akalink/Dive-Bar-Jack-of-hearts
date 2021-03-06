
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRCPrefabs.CyanEmu;
using UnityEngine.UI;

namespace akaUdon
{
    public class ToggleMaster : UdonSharpBehaviour
    {
        [Header("AudioLink")]
        [SerializeField] private GameObject aLinkBehavior;

        [Header("Chair Station Toggle")] [SerializeField]
        private Collider[] stations;
        private bool _stationState = false;
        
        [Header("Chairs and Tables layer swap")] [SerializeField]
        private GameObject[] furniture;
        private bool _layerState = false;

        [UdonSynced()] private bool screenBool = false;
        [SerializeField] private Renderer screen;
        [Header("Animator to adjust bloom")]
        [SerializeField] private Animator bloomAnimator;
        [SerializeField] private string animName;
        [SerializeField] private string animBool;
        [SerializeField] private Slider bloomSlider;


        private void Start()
        {
            if (Networking.LocalPlayer.IsUserInVR() == false)
            {
                bloomAnimator.SetBool(animBool, true);
            }
        }

        public void AudioLink()
        {
            aLinkBehavior.SetActive(!aLinkBehavior.activeSelf);
        }

        public void Furniture()
        {
            _layerState = !_layerState;
            foreach (GameObject chair in furniture)
            {
                if(_layerState)
                {
                    chair.layer = 0;
                }
                else
                {
                    chair.layer = 17;
                }
            }
        }

        public void _Stations()
        {
            _stationState = !_stationState;
            foreach (Collider station in stations)
            {
                station.enabled = _stationState;
            }
        }

        public void SetScreen()
        {
            if (Networking.LocalPlayer == Networking.GetOwner(gameObject))
            {
                screenBool = !screenBool;
                screen.enabled = (screenBool);
                RequestSerialization();
            }
        }

        public override void OnDeserialization()
        {
            screen.enabled =(screenBool);
        }

        public void _SliderValue(){
            bloomAnimator.SetFloat(animName, bloomSlider.value);
        }
    }
}
