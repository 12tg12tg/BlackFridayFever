using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();
    }
    public void OpenDoor()
    {
        col.enabled = false;
    }
}
