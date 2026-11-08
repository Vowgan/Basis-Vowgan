using System;
using Basis.Scripts.BasisSdk.Interactions;
using UnityEngine;


[RequireComponent(typeof(BasisPickupInteractable))]
public class LatchPickupToView : MonoBehaviour
{


    public bool enableForDesktop;
    public bool enableForHeadset;

    public Vector3 offsetPosition;
    public Vector3 offsetRotation;




    private void OnEnable()
    {

    }
}

