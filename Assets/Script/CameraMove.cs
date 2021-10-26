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
        //���� ī�޶� iTween ��ǥ �޾ƿ���
        camPos = GameManager.GM.curStageInfo.startCamPos;
        camLook = GameManager.GM.curStageInfo.startCamLook;

        //�ð����� ��ġ �޾ƿ���
        scanTime = GameManager.GM.curStageInfo.scanTime;
        toPlayerTime = GameManager.GM.curStageInfo.toPlayerTime;

        //�ʱ�ī�޶���ġ����
        transform.position = camPos[0].position;
        transform.LookAt(camLook[0]);

        //Ʈ������� ī�޶� ����
        truckSavePath = player.truck.camPosForSave;
    }

    private void LateUpdate()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
            case GameManager.GameState.Start:
                //Debug.Log("����̳� �����?");
                StartCoroutine(StartCameraMove());
                break;
            case GameManager.GameState.Play:
                //ī�޶� ��ġ
                if(!isTruckSaveLoad)
                {
                    //�÷��̾� ���󰡱�
                    transform.position = player.transform.position - Vector3.forward * offsetZ + Vector3.up * offsetY;                  
                }
                else
                {
                    //�÷��̾��� push ��� ���߱�
                    SaveLoadCamera();
                }
                transform.LookAt(player.transform); //�Ĵٺ���
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
            var toward = iTween.PointOnPath(camLook, timer / scanTime); //��ġ��ȯ���� �޾Ƽ� ���� ����.
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
        //�������� Path��� ��ġ ����
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
