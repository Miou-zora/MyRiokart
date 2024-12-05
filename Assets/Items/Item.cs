using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems; // Replace 'YourNamespace' with the actual namespace where ArcadeKart is defined

namespace Items
{
    public class Item : MonoBehaviour
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