using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private Material original;
    [Header("(Inspector 모두 연결 필요)")]
    public MeshRenderer mesh;
    public Material transparent;
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
                mesh.material = transparent;
                //Debug.Log("메테리얼 바뀜");
            }
            else
                mesh.material = original;
        }
    }
}
