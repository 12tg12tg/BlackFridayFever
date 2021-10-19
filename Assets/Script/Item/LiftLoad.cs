using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLoad : MonoBehaviour
{
    public MeshRenderer[] mesh;
    public GameObject prefab;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        var maxY = 0f;
        mesh = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            var bounds = mesh[i].bounds;
            Vector3 extents = bounds.extents;
            var y = mesh[i].transform.position.y + extents.y * 2;
            maxY = maxY > y ? maxY : y;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var newgo = Instantiate(prefab, transform);
            var pos = transform.position;
            if(maxY != 0)
                pos.y = maxY;
            newgo.transform.position = pos;
        }

        var pos2 = transform.position;
        pos2.y = maxY;
        Debug.DrawLine(transform.position, pos2);

    }

}
