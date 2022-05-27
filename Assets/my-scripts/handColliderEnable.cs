
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace akaUdon
{
    public class handColliderEnable : UdonSharpBehaviour
    {
        private Collider brah;
        private string doorName = "XKCD";
        public TextMeshProUGUI logger;
        private void LoggerPrint(string text)
        {
            if (logger != null)
            {
                logger.text += "-" + this.name + "-" + text + "\n";
            }
        }
        void Start()
        {
            brah = GetComponentsInChildren<Collider>()[1];
        }

        public void OnTriggerEnter(Collider other)
        {
            LoggerPrint("Entered Trigger");
            if (other.gameObject.name.Contains(doorName))
            {
                brah.enabled = true;
                LoggerPrint("Enabled Collider");
            }
        }

        public void OnTriggerExit(Collider other)
        {
            LoggerPrint("Exited Trigger");
            if (other.gameObject.name.Contains(doorName))
            {
                brah.enabled = false;
                LoggerPrint("Disabled Collider");
            }
        }
    }
}
