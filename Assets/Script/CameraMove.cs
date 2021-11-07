using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 mainOffsetPos;
    private Quaternion mainOffsetRot;

    public CharacterStats player;
    public float offsetZ;
    public float offsetY;

    public Transform[] camPos;
    public Transform[] camLook;

    public float scanTime;
    public float toPlayerTime;
    public float timer;
    //public bool lookPlayer;

    private bool isTruckSaveLoad;
    private float truckSaveCamTime = 1f;
    private float savetimer;
    private Transform[] truckSavePath;

    public bool isFirstWinnerMoveEnd;
    public bool isSecondWinnerMoveEnd;
    private float endTimer;
    private float endTime = 1f;
    private Transform[] endingPathToWinner;
    private Transform winner;

    public bool isShowingDance;
    public Transform[] endingPathToPlayer;
    private float endTimer2;
    private float endTime2 = 1f;

    private struct CameraShake
    {
        public float power;
        public float time;
        public float cool;
    }

    private bool isCameraShake;
    private CameraShake shakeInfo;
    private float shakeTimer;
    private float shakeCoolTimer;

    private Coroutine CoStartCameraMove;
    private Stage curStageInfo;

    private void Awake()
    {
        mainOffsetPos = transform.position;
        mainOffsetRot = transform.rotation;
    }

    public void Init()
    {
        curStageInfo = GameManager.GM.curStageInfo;

        //시작 카메라 iTween 좌표 받아오기
        camPos = curStageInfo.startCamPos;
        camLook = curStageInfo.startCamLook;

        //시간관련 수치 받아오기
        scanTime = curStageInfo.scanTime;
        toPlayerTime = curStageInfo.toPlayerTime;

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
                break;
            case GameManager.GameState.Start:
                //Debug.Log("몇번이나 갈까요?");
                if(CoStartCameraMove == null && curStageInfo != null)
                    CoStartCameraMove = StartCoroutine(StartCameraMove());
                break;
            case GameManager.GameState.Play:
                //카메라 위치
                if (!isTruckSaveLoad)
                {
                    if (!isCameraShake)
                    {
                        //플레이어 따라가기
                        transform.position = player.transform.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
                    }
                    else
                    {
                        //쉐이크
                        shakeTimer += Time.deltaTime;
                        shakeCoolTimer += Time.deltaTime;
                        if(shakeCoolTimer > shakeInfo.cool)
                        {
                            //흔들기
                            var originalPivot = player.transform.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
                            var shakeX = Random.Range(-shakeInfo.power/2, shakeInfo.power/2);
                            var shakeY = Random.Range(-shakeInfo.power/2, shakeInfo.power/2);
                            var shakePivot = originalPivot + new Vector3(shakeX, shakeY, 0f);
                            transform.position = shakePivot;
                        }
                        if(shakeTimer > shakeInfo.time)
                        {
                            //해제
                            isCameraShake = false;
                        }
                    }
                }
                else
                {
                    //플레이어의 push 모습 비추기
                    SaveLoadCamera();
                }
                transform.LookAt(player.transform); //쳐다보기
                break;
            case GameManager.GameState.End:
                if (!isFirstWinnerMoveEnd)
                {
                    //매프레임 Path대로 위치 변경
                    endTimer += Time.deltaTime;
                    iTween.PutOnPath(gameObject, endingPathToWinner, Mathf.Clamp01(endTimer / endTime));
                    transform.LookAt(winner); //쳐다보기
                    Debug.DrawLine(winner.position, endingPathToWinner[2].position, Color.red);
                    if (!isShowingDance && endTimer > 3f)
                    {
                        //춤추자
                        GameManager.GM.LetsDance();
                        isShowingDance = true;

                        //카메라 위치 설정
                        var vec3 = endingPathToWinner[2].position - winner.position;
                        vec3.z = -vec3.z;
                        var playerOffset = player.transform.position + vec3;

                        //2번째 엔딩루트 설정
                        endingPathToPlayer[0].position = Camera.main.transform.position;
                        endingPathToPlayer[1].position = playerOffset;

                    }
                    else if (isShowingDance && endTimer > 4f)
                    {
                        isFirstWinnerMoveEnd = true;    //GM에서 이걸인식 하고 종료 UI 띄운다.
                        //짐떨구면서 좌절하자
                        GameManager.GM.LetsDeafeated();
                    }
                }
                else
                {
                    //매프레임 Path대로 위치 변경
                    endTimer2 += Time.deltaTime;
                    iTween.PutOnPath(gameObject, endingPathToPlayer, Mathf.Clamp01(endTimer2 / endTime2));
                    transform.LookAt(player.transform); //쳐다보기
                    //Debug.DrawLine(player.transform.position, endingPathToPlayer[1].position, Color.red);
                    if (endTimer2 / endTime2 > 1)
                        isSecondWinnerMoveEnd = true;
                }
                break;
        }
    }

    private IEnumerator StartCameraMove()
    {
        yield return new WaitForSeconds(1.0f);

        while (timer < scanTime)
        {
            timer += Time.deltaTime;
            iTween.PutOnPath(gameObject, camPos, timer / scanTime);
            var toward = iTween.PointOnPath(camLook, timer / scanTime); //위치반환값을 받아서 직접 대입.
            transform.rotation = Quaternion.LookRotation((toward - transform.position).normalized);
            yield return null;
        }

        StartCoroutine(LerpToPlayerPos());
        //GameManager.GM.State = GameManager.GameState.Play;
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
        GameManager.GM.CheckTutorialAndPlay();
        //lookPlayer = false;
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

    public void EndingInit(CharacterStats stats)
    {
        winner = stats.transform;
        endingPathToWinner = stats.truck.camPosForSave;
        endingPathToWinner[0].position = transform.position;
    }

    public void ShakeCamera(float power, float time, float cool)
    {
        shakeInfo = new CameraShake();
        shakeInfo.power = power;
        shakeInfo.time = time;
        shakeInfo.cool = cool;
        isCameraShake = true;
        shakeCoolTimer = cool;
        shakeTimer = 0f;
    }











    private void OnDrawGizmos()
    {
        iTween.DrawPathGizmos(camPos, Color.blue);
        iTween.DrawPathGizmos(camLook, Color.red);

    }
}
