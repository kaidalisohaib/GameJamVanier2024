using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeleportationManager : MonoBehaviour
{
    public Transform[] entryPoints; // Entry points of the rooms
    public Transform[] exitPoints;  // Exit points of the rooms
    public Transform bossRoomEntry; // Entry point for the boss room

    public int teleportCount = 0;
    public int maxTeleportCount = 3;
    


    private void Start()
    {
        // Subscribe to player enter exit event
        MainCharacter.OnPlayerEnterExit += OnPlayerEnterExit;
    }

    private void OnPlayerEnterExit(Transform exitPoint)
    {
        TeleportPlayer(exitPoint);
    }

    private void TeleportPlayer(Transform exitPoint)
    {
        
        int randomIndex = Random.Range(0, entryPoints.Length);
        Vector3 teleportPosition = entryPoints[randomIndex].position;

        // Teleport the player to the random entry point
        MainCharacter.Instance.transform.position = teleportPosition;

        teleportCount++;

        if (teleportCount >= maxTeleportCount)
        {
            // Teleport to the boss room if max teleport count is reached
            MainCharacter.Instance.transform.position = bossRoomEntry.position;
            teleportCount = 0;
        }
    }
}