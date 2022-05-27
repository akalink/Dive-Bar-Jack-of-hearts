
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.Animations;
using VRC.SDKBase;
using VRC.Udon;

namespace akaUdon
{

    public class lookattoggle : UdonSharpBehaviour
    {
        public Animator _animator;
        public string name;
        public string talk;
        public TextAsset file;
        private string[] lines;
        public TextMeshProUGUI textBox;
        private int stringIndex = 0;
        private string tempString;
        private int playerTalking = -1;

        void Start()
        {
            lines = file.text.Split('\n');
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (playerTalking == -1)
            {
                playerTalking = player.playerId;
                
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("*", player.displayName);
                }
                _animator.SetBool(name, true);
                _animator.SetTrigger(talk);
                InitializeString(0);
            }
        }

        private void InitializeString(int i)
        {
            tempString = lines[i];
            textBox.text = "";
            stringIndex = 0;
            SendCustomEventDelayedFrames(nameof(StringLoad), 2);
        }

        public void StringLoad()
        {
            if (stringIndex < tempString.Length)
            {
                textBox.text += tempString[stringIndex];
                stringIndex++;
                SendCustomEventDelayedFrames(nameof(StringLoad), 2);
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player.playerId == playerTalking)
            {
                playerTalking = -1;
                _animator.SetBool(name, false);
                lines = file.text.Split('\n');
            }
        }
    }
}
