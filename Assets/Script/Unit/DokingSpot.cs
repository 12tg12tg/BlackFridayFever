using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DokingSpot : MonoBehaviour
{
    public TruckScript truck;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("도킹1");
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = other.gameObject.GetComponentInChildren<CharacterStats>();
            if (stats.belongings.Count > 0 && stats.truck == truck)
            {
                //Debug.Log("도킹2");
                other.GetComponent<CharacterStats>().LoadUp();
            }
        }
    }
}
