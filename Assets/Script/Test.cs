using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 forceFoward;
    private void Update()
    {
        Debug.DrawRay(transform.position, forceFoward, Color.red, 1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var foward = (collision.transform.position) - transform.position;
            var minRot = Quaternion.Euler(0f, -8f, 0f) * foward;
            var maxRot = Quaternion.Euler(0f, 8f, 0f) * foward;
            forceFoward = Vector3.Lerp(minRot, maxRot, 5 * Time.deltaTime).normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceFoward * 10, ForceMode.Impulse);
        }
    }
}
