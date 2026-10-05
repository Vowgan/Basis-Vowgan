using Basis;
using UnityEngine;
using Basis.Network.Core;
using Basis.Scripts.Networking.NetworkedAvatar;
using Basis.Scripts.Device_Management.Devices;
using Basis.Scripts.BasisSdk.Interactions;

[Cilboxable]
public class LightSwitch : MonoBehaviour
{
    private BasisNetworkShim networkShim; // Reference to the BasisNetworkShim for network communication
    public GameObject lightSwitch; // Reference to the light switch object
    public GameObject lightObject; // Reference to the light object to toggle
    private BasisInteractableObject interactable;
    private bool isLightOn = false; // Track the state of the light

    private void Start()
    {
        if (!lightObject)
        {
            lightObject = this.transform.GetChild(0).gameObject; // Assuming the light object is the first child of the switch
        }

        if (lightObject)
        {
            lightObject.SetActive(false); // Ensure the light is off at the start
        }

        interactable = this.gameObject.GetComponent<BasisInteractableObject>();
        interactable.OnInteractStartEvent.AddListener(OnInteractStart); // Subscribe to the interact start event to toggle the light
        networkShim = SafeUtil.MakeNetworkable(this); // Make this object networkable to sync state across clients
        networkShim.NetworkMessageReceived = NetworkMessageReceived;
        networkShim.PlayerJoined = PlayerConnected;
        networkShim.ServerOwnershipDestroyed = OnServerOwnershipDestroyed;
    }

    private void OnInteractStart(BasisInput input)
    {
        //ushort playerId = BasisLocalPlayer.Instance.BasisAvatar.LinkedPlayerID; // Get the ID of the player who interacted with the switch
        networkShim.TakeOwnership(); // Take ownership of the object to ensure we can send updates to other clients
        isLightOn = !isLightOn; // Toggle the light state
        this.networkShim.SendCustomNetworkEvent(new byte[] { (byte)(isLightOn ? 1 : 0) }, DeliveryMethod.ReliableOrdered); // Send the new state to other clients
        ToggleLight(); // Toggle the light when interaction starts
    }

    private void ToggleLight()
    {
        lightObject.SetActive(isLightOn); // Toggle the light's active state
        lightSwitch.GetComponent<Transform>().localRotation = Quaternion.Euler(isLightOn ? new Vector3(90, 0, -8) : new Vector3(90, 0, 8)); // Rotate the switch to indicate on/off state
    }

    public void NetworkMessageReceived(ushort PlayerID, byte[] buffer, DeliveryMethod DeliveryMethod)
    {
        Debug.Log($"Received network message from player {PlayerID} with data: {buffer[0]}");
        if (buffer.Length > 0)
        {
            bool newState = buffer[0] == 1; // Convert the byte back to a boolean
            isLightOn = newState; // Update the local state
            ToggleLight(); // Toggle the light based on the received state
        }
    }


    public void PlayerConnected(BasisNetworkPlayer player)
    {
        if (networkShim.IsLocalOwner())
        {
            networkShim.SendCustomNetworkEvent(new byte[] { (byte)(isLightOn ? 1 : 0) }, DeliveryMethod.ReliableOrdered, new ushort[] { player.playerId }); // Send the current state to the newly connected player
        }
    }


    public void OnServerOwnershipDestroyed()
    {
        //Debug.Log($"Server ownership destroyed, attempting to take ownership... from Player {networkShim.currentOwnedPlayer.Player.SafeDisplayName}");
        networkShim.TakeOwnership(); // Attempt to take ownership when the server destroys ownership, which can happen if the owning player disconnects
    }
}
