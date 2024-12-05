using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add the correct namespace for the Item class
using Items;
using KartGame.KartSystems;

public class Mushroom : Item
{
    public int life = 1;

    public override void Use()
    {
        if (life == 0)
            return;
        ArcadeKart.Stats MushroomStats = new ArcadeKart.Stats();
        MushroomStats.TopSpeed = 8;
        MushroomStats.Acceleration = 16;
        ArcadeKart.StatPowerup powerup = new ArcadeKart.StatPowerup();
        powerup.modifiers = MushroomStats;
        powerup.MaxTime = 1;
        powerup.ElapsedTime = 0;
        kart.AddPowerup(powerup);
        GetComponent<AudioSource>().Play();
        life--;
        if (life == 0)
            Destroy(gameObject, 1);
    }
}