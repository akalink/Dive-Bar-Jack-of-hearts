
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ToggleBeer : UdonSharpBehaviour
{
    #region Global Variables (public and private)
    public GameObject ColliderName;
    public ParticleSystem Beer;
    public Transform Handle;
    public float HandleRotation = 30;
    private bool StateOfFountain = false;
    private Vector3 StateOfCollider;
    private Quaternion StateOfHandle;
    private AudioSource ASource;
    private AudioClip AClip;
    
    
    #endregion
    void Start()
    {
        var em = Beer.emission;
        em.enabled = false;

        ASource = this.GetComponent<AudioSource>();
        AClip = ASource.clip;

        //Debug.Log("works before setting State of Collider");
        StateOfCollider = new Vector3(ColliderName.transform.position.x, ColliderName.transform.position.y, ColliderName.transform.position.z);
        //Debug.Log("works after setting State of Collider");
        ColliderName.transform.position = new Vector3(StateOfCollider.x, -10.0f, StateOfCollider.z);
        StateOfHandle = Quaternion.Euler(Handle.rotation.x, Handle.rotation.y, Handle.rotation.z);
        Handle.rotation = StateOfHandle;
    }
    public override void Interact()
    {
        if (StateOfFountain == false)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TurnOnFountain");
        } else
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TurnOffFountain");
        }
    }
    #region Custom Events
    public void TurnOnFountain()
    {
        ASource.pitch = 0.95f;
        ASource.PlayOneShot(AClip);
        StateOfFountain = true;
        var em = Beer.emission;
        em.enabled = true;
        ColliderName.transform.position = StateOfCollider;
        Handle.Rotate(HandleRotation,0,0);
    }

    public void TurnOffFountain()
    {
        ASource.pitch = 1f;
        ASource.PlayOneShot(AClip);
        StateOfFountain = false;
        var em = Beer.emission;
        em.enabled = false;
        ColliderName.transform.position = new Vector3(StateOfCollider.x, -10.0f, StateOfCollider.z);
        Handle.rotation = StateOfHandle;
    }
    #endregion
}
