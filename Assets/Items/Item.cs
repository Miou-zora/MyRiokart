using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems;
using Unity.Netcode; // Replace 'YourNamespace' with the actual namespace where ArcadeKart is defined

namespace Items
{
    public class Item : NetworkBehaviour
    {
        public ArcadeKart kart;
        public bool isSecond = false;
        virtual public void Use()
        {
            Debug.Log("Item used");
        }
        
        public int id;
    }
}