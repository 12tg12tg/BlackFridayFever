using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,   //시작 UI 예정
        Start,  //게임씬 로드
        Play,   //게임중. 승리조건 확인.
        End,    //종료UI 예정
    }
    //Singleton
    private static GameManager instance;
    public static GameManager GM
    {
        get { return instance; }
    }

    //GameState
    private GameState gameState;
    public GameState State
    {
        get { return gameState; }
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.Idle:
                    break;
                case GameState.Start:
                    break;
                case GameState.Play:
                    //문열기 - 취소
                    break;
                case GameState.End:
                    break;
            }
        }
    }

    //UI
    public Text score;
    public Text money;
    public Text stack;
    public Text gauage;

    //Player
    public GameObject player;

    //스테이지마다의 정보 불러오기 : ScriptableObject를 이용하여 씬을 불러올 때 변수 초기화 할 것.
    private Stage curStage;

    private void Awake()
    {
        Debug.Log($"GameManager Awake : {Time.time}");
    }
    private void Start()
    {
        //싱글톤 및 상태 초기화 : 일단 Idle로 초기화 하지 않음. 
        instance = this;
        State = GameState.Play;

        //플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        StartLevel();



    }

    private void StartLevel()
    {
        //버튼이 눌리면, 해당 스테이지 시작.
        curStage = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();

        //==== 현재 활동 씬에서 필요한 오브젝트 찾기 - 보류 ====
        //Scene curScene = SceneManager.GetActiveScene();
        //var gos = curScene.GetRootGameObjects();

        //1. Truck 찾기
        //foreach (var go in gos)
        //{
        //    var truckScripts = go.GetComponentsInChildren<TruckScript>();
        //    foreach (var truck in truckScripts)
        //    {
        //        trucks.Add(truck);
        //    }
        //}

        //2. AI 찾기
        //foreach (var go in gos)
        //{
        //    if (go.tag == "AI")
        //    {
        //        AIs.Add(go.GetComponent<CharacterStats>());
        //    }
        //}

        /*시작작업*/
        //Truck - Unit 연결.
        var trucks = curStage.trucks;
        var AIs = curStage.Ais;

        for (int i = 0; i < trucks.Length; i++)
        {
            int rnd = Random.Range(0, trucks.Length);
            var temp = trucks[i];
            trucks[i] = trucks[rnd];
            trucks[rnd] = temp;
        }

        player.GetComponent<CharacterStats>().truck = trucks[0]; //0번트럭은 플레이어, 1, 2, 3번은 AI

        for (int i = 0; i < AIs.Length; i++)
        {
            AIs[i].truck = trucks[i+1];            
        }
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Idle:
                break;

            case GameState.Start:
                break;

            case GameState.Play:
                break;

            case GameState.End:
                break;
        }
    }




    /*=========================외부 호출용 함수=========================*/
    //돈 : 돈을 습득한 경우 → 1) 보유 돈 증가 함수 호출, 2) UI Text 업데이트
    public void MoneyCollision(CharacterStats unit, Money money)
    {
        unit.getMoney(money.money);
        if (unit.tag == "Player")
        {
            this.money.text = unit.money.ToString();
        }
    }


    //점수 & 상품 : 아이템을 습득한 경우 → 0) 돈 확인, 1) 보유 아이템 정보 변경 함수 호출, 2) UI Text 업데이트
    public bool ItemCollsion(CharacterStats unit, ItemInfo item)
    {
        //돈 체크
        if (unit.money >= item.price)
        {
            unit.getStack(item);//Character의 Stats 점수올리기
            unit.GetComponentInChildren<LiftLoad>().LiftPurchased(item); //Character의 물건올리기
            //item.SetActive(false);  //비활성화

            if (unit.tag == "Player")
            {
                score.text = unit.score.ToString();
                money.text = unit.money.ToString();
                this.stack.text = unit.itemStack.ToString();
                gauage.text = unit.score.ToString("P1");
            }
            return true;
        }
        return false;
    }

    public bool IsEnd(CharacterStats unit)
    {
        if(unit.truck.currentScore >= curStage.goalScore)
        {
            //종료 점수에 도달했다면 게임매니저의 상태를 End로 바꾸면서 현재 우승자 AI 보내기? → 카메라우승자로 이동 → 플레이어로 이동.
            State = GameState.End;
            return true;
        }
        return false;
    }

    public void DecideTrayCollision(Collider a, Collider b)
    {
        var aStat = a.gameObject.GetComponentInParent<CharacterStats>();
        var bStat = b.gameObject.GetComponentInParent<CharacterStats>();

        if (aStat.isStuned || bStat.isStuned)
            return;

        var forceToA = (aStat.transform.position - bStat.transform.position).normalized;
        var forceToB = (bStat.transform.position - aStat.transform.position).normalized;

        if(aStat.itemStack == 0 && bStat.itemStack == 0)
        {
            //Nothing
            //Debug.Log("낫띵");
        }
        else if(aStat.itemStack == 0)
        {
            //a가 돈을 흘리는 함수 호출
            Debug.Log("a가 돈을 흘림");
        }
        else if (bStat.itemStack == 0)
        {
            //a가 돈을 흘리는 함수 호출
            Debug.Log("b가 돈을 흘림");
        }
        else if (aStat.itemStack == bStat.itemStack)
        {
            //아무일도없다
            Debug.Log("같아서 아무일도 일어나지 않아요");
        }
        else if (aStat.itemStack > bStat.itemStack)
        {
            //b가 상자를 흘림
            //Debug.Log("a가 더 많다");
            bStat.DropItem(forceToB);
        }
        else
        {
            //a가 상자를 흘림.
            //Debug.Log("b가 더 많다");
            aStat.DropItem(forceToA);
        }

    }
}
