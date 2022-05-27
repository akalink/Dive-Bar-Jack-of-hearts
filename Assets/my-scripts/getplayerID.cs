
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class getplayerID : UdonSharpBehaviour
{
    public TextMeshProUGUI text;
    [UdonSynced()] private int playerId;
    public TextAsset textfile;
    void Start()
    {
        
    }

    public override void Interact()
    {
        ButtonPress();
    }

    public void ButtonPress()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        text.text = Networking.LocalPlayer.playerId.ToString();
        //RequestSerialization();

    }

    public override void OnDeserialization()
    {
        UpdateTextField();
    }

    private void UpdateTextField()
    {
        text.text = playerId.ToString();
    }
}
