
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
        [SerializeField] private TextMeshProUGUI answer1;
        [SerializeField] private TextMeshProUGUI answer2;
        [SerializeField] private AudioSource speak;
        private AudioClip speakSound;
        
        
        #endregion

        #region not serialized
        private string[] lines;
        private int stringIndex;
        private string tempString;
        private int playerTalking = -1;
        private int currentLine;
        private string[] responses;
        private int responseString = 1;
        
        #endregion
        
        
        void Start()
        {
            speakSound = speak.clip;
            lines = file.text.Split('\n');
            responses = answers.text.Split('\n');
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (Networking.LocalPlayer == player) //if the conversation is not being occupied
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("*", player.displayName);
                }
                _animator.SetBool(lookAtName, true);
                _animator.SetTrigger(talk);
                InitializeString(0);
            }
        }

        private void InitializeString(int i)
        {
            if (i >= lines.Length)
            {
                return;
            }
            tempString = lines[i];
            textBox.text = "";
            stringIndex = 0;
            SendCustomEventDelayedSeconds(nameof(StringLoad), 0.03f);
        }

        public void StringLoad()
        {
            if (stringIndex < tempString.Length)
            {
                if(tempString[stringIndex] != '~')
                {
                    textBox.text += tempString[stringIndex];
                    stringIndex++;
                    
                    if(stringIndex % 3 == 0){speak.PlayOneShot(speakSound);}
                    SendCustomEventDelayedSeconds(nameof(StringLoad), 0.03f);
                } else
                {
                    _animator.SetBool(responsesShow, true);
                    answer1.text = responses[responseString - 1];
                    answer2.text = responses[responseString];
                }
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (Networking.LocalPlayer == player)
            {
                playerTalking = -1;
                _animator.SetBool(lookAtName, false);
                _animator.SetBool(responsesShow, false);
                lines = file.text.Split('\n');
            }
        }

        public void FirstAnswer()
        {
            InitializeString(responseString);
            _animator.SetBool(responsesShow, false);
        }


        
        public void SecondAnswer()
        {
            InitializeString(responseString+1);
            _animator.SetBool(responsesShow, false);
        }

        public void SecondAnswerNetwork()
        {
            InitializeString(responseString+1);
            _animator.SetBool(responsesShow, false);
        }
        
    }
}
