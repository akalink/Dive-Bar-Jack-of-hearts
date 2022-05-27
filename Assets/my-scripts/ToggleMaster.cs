
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRCPrefabs.CyanEmu;

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
    }
}
