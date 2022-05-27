
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.Animations;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace akaUdon
{
    public class LookAtToggle : UdonSharpBehaviour
    {
        #region Serialized
        [SerializeField] private Animator _animator;
        [SerializeField] private string lookAtName;
        [SerializeField] private string talk;
        [SerializeField] private string responsesShow;
        [SerializeField] private  TextAsset file;
        [SerializeField] private TextAsset answers;
        [SerializeField] private TextMeshProUGUI textBox;
        #endregion

        #region not serialized
        private string[] lines;
        private int stringIndex;
        private string tempString;
        private int playerTalking = -1;
        private int currentLine;
        
        #endregion
        
        
        void Start()
        {
            lines = file.text.Split('\n');
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (playerTalking == -1) //if the conversation is not being occupied
            {
                playerTalking = player.playerId;
                
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("*", player.displayName);
                }

                if (player.playerId == playerTalking) //makes the look at constraint only work for the person in the conversation
                {
                    _animator.SetBool(lookAtName, true);
                }
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
            else
            {
                _animator.SetBool(responsesShow, true);
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player.playerId == playerTalking)
            {
                playerTalking = -1;
                _animator.SetBool(lookAtName, false);
                lines = file.text.Split('\n');
            }
        }

        public void FirstAnswer()
        {
            if (Networking.LocalPlayer.playerId == playerTalking)
            {
                SendCustomNetworkEvent(NetworkEventTarget.All,nameof(FirstAnswerNetwork));
            }
        }

        private void FirstAnswerNetwork()
        {
            Debug.Log("this is an answer");   
        }
    }
}
