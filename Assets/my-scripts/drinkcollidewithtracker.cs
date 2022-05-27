
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace akaUdon
{
    public class drinkcollidewithtracker : UdonSharpBehaviour
    {
  
        #region AllVars
        private Renderer liquidFill;
        
        [SerializeField] private float EmptyFloat = 1.0f;
        
        [SerializeField] private float FullFloat = 0.35f;
        [SerializeField] private float TimeMultpler = 0.01f;
        private bool isDrinkingHand = false;
        private bool inMouth = false;
        private bool isLeftAlone = true;
        private bool isFilling = false;
        private float drinkState;
        private SphereCollider myCollider;
        
        #endregion

        #region Calls&Update&Start

        private void Start()
        {
            liquidFill = GetComponent<Renderer>();
            myCollider = GetComponent<SphereCollider>();
            drinkState = EmptyFloat;
        }
        private void Update()
        {
            ReportState();
            if (isFilling)
            {
                //filling code
                drinkState = drinkState - TimeMultpler * Time.deltaTime;
                if (drinkState < FullFloat)
                {
                    isLeftAlone = true;
                }
                UpDateMaterialState();
            }
            if (isLeftAlone) //is nothing happening, exit if true
            {
                return;
            }
            if ((isDrinkingHand && inMouth) || (isDrinkingHand && !Networking.LocalPlayer.IsUserInVR())) //when the drink is being drunk
            {
                //drinking code
                drinkState = drinkState + TimeMultpler * Time.deltaTime;
                if (drinkState > EmptyFloat)
                {
                    isLeftAlone = true;
                }
            }
            UpDateMaterialState();
        }

        public void UpDateMaterialState()
        {
            liquidFill.material.SetFloat("_FillAmount", drinkState);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 22) //22 is post processing layer, for drinking
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(StickInMouth));
            }
            else if (other.gameObject.layer == 11) //11 is enviorment layer
            {
                FillBeverage();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 22) //22 is post processing layer, for drinking
            {
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(RemoveFromMouth));
            }
            else if (other.gameObject.layer == 11) //11 is enviorment layer, for filling
            {
                SendCustomNetworkEvent(NetworkEventTarget.All,nameof(StopFillBeverage));
            }
        }
        #endregion
        #region CustomMethodsForNetworkEvents

        public void DrinkBeverage()
        {
            isDrinkingHand = true;
        }

        public void RemoveFromDrinking()
        {
            isDrinkingHand = false;
        }
        
        public void StickInMouth()
        {
            inMouth = true;
        }

        public void RemoveFromMouth()
        {
            inMouth = false;
        }

        public void FillBeverage()
        {
            isFilling = true;
        }

        public void StopFillBeverage()
        {
            isFilling = false;
        }

        public void IsLeftAlone()
        {
            isLeftAlone = true;
            myCollider.enabled = false;
        }
        
        public void IsPickedUp()
        {
            isLeftAlone = false;
            myCollider.enabled = true;
        }
        
        #endregion

        #region Pickup and Trigger Methods

        public override void OnPickup()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IsPickedUp));
        }

        public override void OnDrop()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All,nameof(IsLeftAlone));
        }

        public override void OnPickupUseDown()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All,nameof(DrinkBeverage));
        }

        public override void OnPickupUseUp()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(RemoveFromDrinking));
        }

        #endregion

        private void ReportState()
        {
            Debug.Log("isLeftAlone: "+ isLeftAlone + " isDrinkHand" + isDrinkingHand + " state of material: " + drinkState);
        }
    }
}
