using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add the correct namespace for the Item class
using Items;
using KartGame.KartSystems;

public class GoldenMushroom : Item
{
    public float elapsedTime = 0;
    bool used = false;

    bool end = false;

    private float delay = 0;
    void Update()
    {
        delay += Time.deltaTime;
        if (!used)
            return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 7.5) {
            end = true;
            Destroy(gameObject, 1);
        }
    }

    public override void Use()
    {
        if (end)
            return;
        // check last time used
        if (delay < 1)
            return;
        delay = 0;
        ArcadeKart.Stats MushroomStats = new ArcadeKart.Stats();
        MushroomStats.TopSpeed = 8;
        MushroomStats.Acceleration = 16;
        ArcadeKart.StatPowerup powerup = new ArcadeKart.StatPowerup();
        powerup.modifiers = MushroomStats;
        powerup.MaxTime = 1;
        powerup.ElapsedTime = 0;
        kart.AddPowerup(powerup);
        GetComponent<AudioSource>().Play();
        used = true;
    }
}