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
        Play,   //출발신호 - Door 열기
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
                    //문열기
                    door.OpenDoor();
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

    //Ai
    public List<CharacterStats> AIs;

    //Door
    public DoorScript door;

    //Trucks
    public List<TruckScript> trucks;

    //스테이지마다의 정보 불러오기 : ScriptableObject를 이용하여 씬을 불러올 때 변수 초기화 할 것.
    //Timer
    private float startDelay = 10f;
    private float startTimer;
    private int goalScore = 100;



    private void Awake()
    {
        //싱글톤 및 상태 초기화 : 일단 Idle로 초기화 하지 않음. 
        instance = this;
        State = GameState.Start;

        //플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        //현재 활동 씬에서 필요한 오브젝트 찾기         //SceneManager.GetActiveScene().GetRootGameObjects();
        //1. Door 찾기
        Scene curScene = SceneManager.GetActiveScene();
        //Debug.Log(temp.name);
        var gos = curScene.GetRootGameObjects();
        foreach (var go in gos)
        {
            if(go.tag == "Door")
            {
                door = go.GetComponent<DoorScript>();
            }
        }

        //2. Truck 찾기
        foreach (var go in gos)
        {
            var truckScripts = go.GetComponentsInChildren<TruckScript>();
            foreach (var truck in truckScripts)
            {
                trucks.Add(truck);
            }
        }

        //3. AI 찾기
        foreach (var go in gos)
        {
            if (go.tag == "AI")
            {
                AIs.Add(go.GetComponent<CharacterStats>());
            }
        }

        //시작작업
        //Truck - Unit 연결.
        var playerTruck = Random.Range(0, trucks.Count);
        player.GetComponent<CharacterStats>().truck = trucks[playerTruck];
        trucks.RemoveAt(playerTruck);

        for (int i = 0; i < trucks.Count; i++)
        {
            int rnd = Random.Range(0, trucks.Count);
            var temp = trucks[i];
            trucks[i] = trucks[rnd];
            trucks[rnd] = temp;
        }

        for (int i = 0; i < AIs.Count; i++)
        {
            AIs[i].truck = trucks[i];
        }

    }
    private void Update()
    {
        switch (State)
        {
            case GameState.Idle:
                break;

            case GameState.Start:
                //문열기
                startTimer += Time.deltaTime;
                if (startTimer > startDelay)
                    State = GameState.Play;
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
            unit.getStack(item);//점수올리기
            unit.GetComponentInChildren<LiftLoad>().LiftPurchased(0); //물건올리기
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
        if(unit.truck.currentScore >= goalScore)
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

        if(aStat.belongings.Count == 0 && bStat.belongings.Count == 0)
        {
            //Nothing
            //Debug.Log("낫띵");
        }
        else if(aStat.belongings.Count == 0)
        {
            //a가 돈을 흘리는 함수 호출
            Debug.Log("a가 돈을 흘림");
        }
        else if (bStat.belongings.Count == 0)
        {
            //a가 돈을 흘리는 함수 호출
            Debug.Log("b가 돈을 흘림");
        }
        else if (aStat.belongings.Count == bStat.belongings.Count)
        {
            //아무일도없다
            Debug.Log("같아서 아무일도 일어나지 않아요");
        }
        else if (aStat.belongings.Count > bStat.belongings.Count)
        {
            //b가 상자를 흘림
            //Debug.Log("a가 더 많다");
            bStat.DropItem();
        }
        else
        {
            //a가 상자를 흘림.
            //Debug.Log("b가 더 많다");
            aStat.DropItem();
        }

    }
}
