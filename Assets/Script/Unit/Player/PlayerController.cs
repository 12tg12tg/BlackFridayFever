using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MultiTouch multiTouch;
    public CharacterStats stats;
    public int stack;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (stats == null)
            stats = GetComponent<CharacterStats>();

        Move();

    }

    public void Move()
    {
        var joystick = multiTouch.Joystick;
        var direction = new Vector3(joystick.x, 0, joystick.y);
        transform.position += direction * stats.speed * Time.deltaTime;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
        //Debug.Log(joystick);
    }
}
