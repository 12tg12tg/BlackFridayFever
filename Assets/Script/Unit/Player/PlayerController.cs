using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitBehaviour
{
    //터치입력
    public MultiTouch multiTouch;

    //스턴시 이동제한을 걸기 위함.
    public float stunTimer;
    private Vector3 direction;

    //스킨 및 색배정
    public GameObject defaultSkin;
    public Transform skinParent;
    public SkinnedMeshRenderer skin;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Init(GameObject skinPrefab)
    {
        //스킨 및 색
        if(skinPrefab != null)
        {
            Instantiate(skinPrefab, skinParent);
        }
        else
        {
            Instantiate(defaultSkin, skinParent);
        }
        var skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].tag == "Skin")
            {
                skin = skins[i];
                break;
            }
        }
        skin.material.color = stats.truck.bodyColor;

        //위치
        transform.position = stats.truck.dokingSpot.position + transform.forward * 2f;
    }

    private void Update()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
                break;
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:

                if (!stats.isStuned && !animator.GetCurrentAnimatorStateInfo(0).IsName("Push"))
                    Move();
                else
                {
                    stunTimer += Time.deltaTime;
                    if (stunTimer > stats.stats.stunTime)
                    {
                        stunTimer = 0f;
                        stats.isStuned = false;
                    }
                }
                AnimationUpdate();
                break;
            case GameManager.GameState.End:
                if(GameManager.GM.winner != stats)
                {
                    setIdleAnimation();
                }
                break;
        }
    }
    public void Move()
    {
        //이동 및 회전
        var joystick = multiTouch.Joystick;
        direction = new Vector3(joystick.x, 0, joystick.y);

        if (direction != Vector3.zero)
        {
            direction = Vector3.Lerp(transform.forward, direction, 8.0f * Time.deltaTime);
            transform.position += direction * stats.speed * Time.deltaTime;
            //var lerp = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void AnimationUpdate()
    {
        //애니
        if (direction == Vector3.zero)
            setIdleAnimation();
        else
            setMoveAnimation();

        animator.SetInteger("Stack", stats.itemStack);
    }
}
