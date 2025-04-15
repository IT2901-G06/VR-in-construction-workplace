using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTaskNPCDemo : MonoBehaviour
{
    [SerializeField] private NPCSpawner _npcSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (_npcSpawner.ActiveNPCInstances.Count >= 2)
        {
            _npcSpawner.ActiveNPCInstances[1].SetActive(true);
        }
    }
}
