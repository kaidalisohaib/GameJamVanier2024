using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeleportationManager : MonoBehaviour
{
    public Transform[] entryPoints; // Entry points of the rooms
    public Transform[] exitPoints;  // Exit points of the rooms

    public int teleportCount = 0;
    public int maxTeleportCount = 3; // for portal
}