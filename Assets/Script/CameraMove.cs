using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public CharacterStats player;
    public float offsetZ;
    public float offsetY;

    public Transform[] camPos;
    public Transform[] camLook;

    public float scanTime;
    public float toPlayerTime;
    public float timer;
    public bool lookPlayer;

    private bool isTruckSaveLoad;
    private float truckSaveCamTime = 1f;
    private float savetimer;
    private Transform[] truckSavePath;

    public void Init()
    {
        //시작 카메라 iTween 좌표 받아오기
        camPos = GameManager.GM.curStageInfo.startCamPos;
        camLook = GameManager.GM.curStageInfo.startCamLook;

        //시간관련 수치 받아오기
        scanTime = GameManager.GM.curStageInfo.scanTime;
        toPlayerTime = GameManager.GM.curStageInfo.toPlayerTime;

        //초기카메라위치설정
        transform.position = camPos[0].position;
        transform.LookAt(camLook[0]);

        //트럭저장용 카메라 시점
        truckSavePath = player.truck.camPosForSave;
    }

    private void LateUpdate()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
            case GameManager.GameState.Start:
                //Debug.Log("몇번이나 갈까요?");
                StartCoroutine(StartCameraMove());
                break;
            case GameManager.GameState.Play:
                //카메라 위치
                if(!isTruckSaveLoad)
                {
                    //플레이어 따라가기
                    transform.position = player.transform.position - Vector3.forward * offsetZ + Vector3.up * offsetY;                  
                }
                else
                {
                    //플레이어의 push 모습 비추기
                    SaveLoadCamera();
                }
                transform.LookAt(player.transform); //쳐다보기
                break;
            case GameManager.GameState.End:
                break;
        }
    }

    private IEnumerator StartCameraMove()
    {
        yield return new WaitForSeconds(1.5f);
        if (!lookPlayer && timer < scanTime)
        {
            timer += Time.deltaTime;
            iTween.PutOnPath(gameObject, camPos, timer / scanTime);
            var toward = iTween.PointOnPath(camLook, timer / scanTime); //위치반환값을 받아서 직접 대입.
            transform.rotation = Quaternion.LookRotation((toward - transform.position).normalized);

        }
        else
        {
            lookPlayer = true;
            StartCoroutine(LerpToPlayerPos());
            //GameManager.GM.State = GameManager.GameState.Play;
        }
    }

    private IEnumerator LerpToPlayerPos()
    {
        var finalPos = player.transform.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
        var curPos = transform.position;

        var curRot = transform.rotation;
        var finalRot = Quaternion.LookRotation(player.transform.position - finalPos);
        timer = 0f;
        while (timer < toPlayerTime)
        {
            timer += Time.deltaTime;
            var lerp = Vector3.Lerp(curPos, finalPos, timer / toPlayerTime);
            transform.position = lerp;

            var lerpRot = Quaternion.Slerp(curRot, finalRot, timer / toPlayerTime);
            transform.rotation = lerpRot;
            yield return null;
        }
        GameManager.GM.State = GameManager.GameState.Play;
        lookPlayer = false;
        timer = 0f;
    }

    public void StartSaveLoadCamMove()
    {
        isTruckSaveLoad = true;
        savetimer = 0f;
        truckSavePath[0].position = transform.position;
    }

    public void SaveLoadCamera()
    {
        //매프레임 Path대로 위치 변경
        savetimer += Time.deltaTime;
        iTween.PutOnPath(gameObject, truckSavePath, Mathf.Clamp01(savetimer / truckSaveCamTime));
        if (savetimer > 3f)
            isTruckSaveLoad = false;
    }














    private void OnDrawGizmos()
    {
        iTween.DrawPathGizmos(camPos, Color.blue);
        iTween.DrawPathGizmos(camLook, Color.red);

    }
}
