using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGround", menuName = "Ground")]
public class GroundObject : ScriptableObject
{
    public bool isRoad; // classic road
    public bool isOffroad; // offroad like dirt, grass, etc.
    public bool isWall; // classic wall
}