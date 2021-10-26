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
        Start,  //게임씬 로드.
        Play,   //게임중. 승리조건 확인.
        End,    //종료UI 예정. - 버튼 눌리기 전까지 스테이지씬은 남아있는다.
    }

    //Singleton
    private static GameManager instance;
    public static GameManager GM
    {
        get { return instance; }
    }

    //GameState
    [SerializeField] private GameState gameState;
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
    public Stage curStageInfo;

    //엔딩씬
    private bool canEnd;

    private void Awake()
    {
        //Debug.Log($"GameManager Awake : {Time.time}");
        //싱글톤 및 상태 초기화 : 일단 Idle로 초기화 하지 않음. 
        instance = this;

    }
    private void Start()
    {
        State = GameState.Idle;

        //Ai와 플레이어의 상호 연결
        StartLevel();
        Debug.Log($"씬 준비 완료.");

        //플레이어활성화
        player.SetActive(true);

        //트럭관련 Init
        player.GetComponent<PlayerController>().Init();
        for (int i = 0; i < curStageInfo.Ais.Length; i++)
        {
            curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        }

        //카메라 활성화
        Camera.main.GetComponent<CameraMove>().Init();

        //게임매니저 상태 변경
        State = GameState.Start;
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
                EndUpdate();
                break;
        }
    }

    private void StartLevel()
    {
        //버튼이 눌리면, 해당 스테이지 시작.
        curStageInfo = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();

        /*시작작업*/
        //Truck - Unit 연결.
        var trucks = curStageInfo.trucks;
        var AIs = curStageInfo.Ais;

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

    private void EndUpdate()
    {
        if(canEnd)
            SceneManager.LoadScene(0);
    }

    public void EndingScene(CharacterStats winner)
    {
        var playerStats = player.GetComponent<CharacterStats>();
        var Ais = curStageInfo.Ais;

        //애니바꾸고,
        playerStats.EndAnimation(playerStats == winner);
        for (int i = 0; i < Ais.Length; i++)
        {
            Ais[i].EndAnimation(Ais[i] == winner);
        }

        //카메라이동


        //(카메라이동 끝나거나) 몇초후


        //유아이띄우기


        //버튼눌리면 엔드씬 ㄱㄱ


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
        if(unit.truck.currentScore >= curStageInfo.goalScore)
        {
            //종료 점수에 도달했다면 게임매니저의 상태를 End로 바꾸면서 현재 우승자 AI 보내기? → 카메라우승자로 이동 → 플레이어로 이동.

            /*종료시 호출할 함수들*/
            EndingScene(unit);



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
