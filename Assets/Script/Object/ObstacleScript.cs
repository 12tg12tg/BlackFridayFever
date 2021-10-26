using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public MeshRenderer mesh;
    private Material original;
    public Material transParent;
    private void Awake()
    {
        original = mesh.material;
    }
    private void Update()
    {
        if (GameManager.GM.State != GameManager.GameState.Play)
            return;

        var playerPos = GameManager.GM.player.transform.position;
        var playerViewport = Camera.main.WorldToViewportPoint(playerPos);
        Ray ray = Camera.main.ViewportPointToRay(playerViewport);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.gameObject == gameObject)
            {
                mesh.material = transParent;
                //Debug.Log("메테리얼 바뀜");
            }
            else
                mesh.material = original;
        }
    }
}
