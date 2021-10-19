using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private MeshRenderer mesh;
    private Color originalColor;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        originalColor = mesh.material.color;
    }
    private void Update()
    {
        var playerPos = GameManager.GM.player.transform.position;
        var playerViewport = Camera.main.WorldToViewportPoint(playerPos);
        Ray ray = Camera.main.ViewportPointToRay(playerViewport);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.gameObject == gameObject)
            {
                var color = mesh.material.color;
                color.a = 0.2f;
                mesh.material.color = color;
            }
            else
                mesh.material.color = originalColor;
        }
    }
}
