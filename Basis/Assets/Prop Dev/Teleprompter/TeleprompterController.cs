using System;
using System.Collections.Generic;
using Basis.Scripts.BasisSdk.Interactions;
using Basis.Scripts.Device_Management.Devices;
using Basis.Shims;
using UnityEngine;

[Cilboxable]
public class TeleprompterController : MonoBehaviour
{


    public Teleprompter teleprompter;

    private BasisPickupInteractable _pickup;
    private bool _isHeld;
    private HashSet<BasisInput> _inputs = new();

    private BasisInputRequest _request;




    private void Start()
    {

        _pickup = GetComponent<BasisPickupInteractable>();
        _pickup.OnPickupUse.AddListener(OnPickupUse);

        _pickup.OnInteractStartEvent.AddListener(OnInteractStart);
        _pickup.OnInteractEndEvent.AddListener(OnInteractEnd);

        // _request = BasisInputShim.Request(this);
        // _request.AddController(BasisControllerHand.Right, BasisInputControllerAxis.StickY);
    }

    private void Update()
    {
        if (!_isHeld) return;

        if (BasisControllerInputShim.IsAvailable(BasisControllerHand.Right))
        {
            Vector2 rightStick = BasisControllerInputShim.GetStick(BasisControllerHand.Right);
            teleprompter.InterpretInput(rightStick.y > 0);
        }
        else
        {
            float scroll = -BasisDesktopInputShim.MouseScroll.y;
            if (scroll != 0) teleprompter.InterpretInput(scroll > 0);
        }
    }



    public void OnInteractStart(BasisInput input)
    {
        _inputs.Add(input);
        _isHeld = _inputs.Count > 0;
    }

    public void OnInteractEnd(BasisInput input)
    {
        _inputs.Remove(input);
        _isHeld = _inputs.Count > 0;
    }


    public void OnPickupUse(BasisPickUpUseMode mode)
    {
        switch (mode)
        {
            case BasisPickUpUseMode.OnPickUpUseUp:
                teleprompter.EnableRaycast(true);
                break;
            case BasisPickUpUseMode.OnPickUpUseDown:
                teleprompter.EnableRaycast(false);
                teleprompter.NextState();
                break;
        }
    }
}
