using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using KartGame.KartSystems;
using Unity.Netcode;

public class Star : Item
{
    public int life = 1;

    public override void Use()
    {
        if (life == 0)
            return;

        ApplyStarEffect();
        TriggerRainbowEffectOnOtherPlayersServerRpc();
        life--;
        if (life == 0)
            Destroy(gameObject, 7);
    }

    private void ApplyStarEffect()
    {
        ArcadeKart.Stats starStats = new ArcadeKart.Stats
        {
            TopSpeed = 8,
            Acceleration = 8
        };
        ArcadeKart.StatPowerup powerup = new ArcadeKart.StatPowerup
        {
            modifiers = starStats,
            MaxTime = 7,
            ElapsedTime = 0
        };
        kart.AddPowerup(powerup);
        GetComponent<AudioSource>().Play();
        kart.GetComponent<RainbowEffect>().TemporarilyApplyRainbowEffect(7);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TriggerRainbowEffectOnOtherPlayersServerRpc(ServerRpcParams rpcParams = default)
    {
        if (IsServer)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                ulong clientId = client.Key;

                if (clientId == rpcParams.Receive.SenderClientId)
                {
                    GameObject kartObject = client.Value.PlayerObject.GetComponent<ArcadeKart>().gameObject;
                    TriggerRainbowEffectOnPlayerClientRpc(kartObject.GetComponent<NetworkObject>().NetworkObjectId);
                }
            }
        }
    }

    [ClientRpc]
    private void TriggerRainbowEffectOnPlayerClientRpc(ulong networkObjectId)
    {
        if (IsOwner)
            return;

        GameObject kart = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].gameObject;
        if (kart != null)
        {
            kart.GetComponent<RainbowEffect>().TemporarilyApplyRainbowEffect(7);
        }
    }
}
