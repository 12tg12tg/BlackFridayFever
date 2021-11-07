using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct RandomStageInfo
{
    public int randStageIndex;
    public int[] randAiIndex;
}

public class GameManager : MonoBehaviour
{
    public ParkingIndicator parking_indicator_ui;

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
                    SoundManager.Instance.PlayGameBGM();
                    //문열기 - 취소
                    break;
                case GameState.End:
                    SoundManager.Instance.StopGameBGM();
                    break;
            }
        }
    }

    //Reward
    private int diamond;
    public int Diamond
    {
        get => diamond;
        set => diamond = value;
    }

    //Player
    public GameObject player;

    //Camera
    public CameraMove mainCam;

    //스테이지마다의 정보 불러오기 : ScriptableObject를 이용하여 씬을 불러올 때 변수 초기화 할 것.
    public Stage curStageInfo;

    //엔딩씬
    public CharacterStats winner;

    //엔딩이후 UI
    private float endUITimer;
    private bool endSceneAdded;

    //Main or Stage
    public static bool isStage;

    //Save할 정보 인스턴스
    [Header("저장을 위한 인스턴스")]
    public MainWindows main;
    public StorageButtonGroup skin;
    public StorageButtonGroup car;
    public SoundManager sound;

    //MoneyUpdate
    public InGameWindow inGame;

    //Tutorial
    public bool tutorialDone;



    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        GameManager.GM.LoadData();

        State = GameState.Start;
        if (isStage)
        {
            StartLevel();
        }
        else
        {
            WindowManager.Instance.Init();
        }
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Idle:
                break;
            case GameState.Start:
                if (startSceneCoroutine == null)
                {
                    startSceneCoroutine = StartCoroutine(CoStartScene());
                }
                break;
            case GameState.Play:
                break;
            case GameState.End:
                if(mainCam.isSecondWinnerMoveEnd)
                {
                    //3초후 유아이띄우기
                    endUITimer += Time.deltaTime;
                    if (!endSceneAdded && endUITimer > 3f)
                    {
                        GoEndUi();
                        endSceneAdded = true;
                    }
                }
                break;
        }
    }


    public int finalStage = 17;
    public int lastOpenedStage;
    public bool IsRandStage
    {
        get => lastOpenedStage > finalStage;
    }

    public RandomStageInfo randStageInfo;

    public void CreateRandomStageInfo()
    {
        randStageInfo.randStageIndex = Random.Range(1, finalStage + 1);
        randStageInfo.randAiIndex = Stage.Instance.RandAiIndexArr;
    }

    public void StartMain()
    {
        GameManager.isStage = false;
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("Stage0", LoadSceneMode.Additive);
    }

    public void StartScene()    //씬불러오기
    {
        isStage = true;
        SaveData();     //다이아및 설정 저장

        if (closeSceneCoroutine == null)
            closeSceneCoroutine = StartCoroutine(CoCloseAndStartScene());

        //if (IsRandStage)
        //{
        //    SceneManager.LoadScene("Game");
        //    SceneManager.LoadScene($"Stage{randStageInfo.randStageIndex}", LoadSceneMode.Additive);
        //}
        //else
        //{
        //    SceneManager.LoadScene("Game");
        //    SceneManager.LoadScene($"Stage{lastOpenedStage}", LoadSceneMode.Additive);
        //}
    }

    private Coroutine closeSceneCoroutine;

    private IEnumerator CoCloseAndStartScene()
    {
        var fade = WindowManager.Instance.fade;
        fade.FadeOut();
        yield return new WaitUntil(() => fade.fadeEnd);

        if (IsRandStage)
        {
            SceneManager.LoadScene("Game");
            SceneManager.LoadScene($"Stage{randStageInfo.randStageIndex}", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("Game");
            SceneManager.LoadScene($"Stage{lastOpenedStage}", LoadSceneMode.Additive);
        }
    }

    private Coroutine startSceneCoroutine;

    private IEnumerator CoStartScene()
    {
        var fade = WindowManager.Instance.fade;
        fade.FadeIn();
        yield return new WaitUntil(() => fade.fadeEnd);
    }

    public void CheckTutorialAndPlay()
    {
        if(!tutorialDone)
        {
            //Debug.Log("듀토리얼을 안했습니다. 시작해주시겠어요?");
            StartCoroutine(DoTutorial());
        }
        else
        {
            //이미 했음
            State = GameState.Play;
        }
    }

    public Tutorial tutorial;
    private IEnumerator DoTutorial()
    {
        tutorial.Open1();
        yield return new WaitUntil(()=> tutorial.isDone);

        tutorialDone = true;
        //SaveData();
        State = GameState.Play;
    }











    public void AfterClear()
    {
        lastOpenedStage++;

        if (IsRandStage)
        {
            CreateRandomStageInfo();
        }

        SaveData();
    }

    public void ContinueLevel()
    {
        StartScene();
        isStage = true;
    }


    private void StartLevel()   //게임준비
    {
        //Ai와 플레이어의 상호 연결

        //버튼이 눌리면, 해당 스테이지 시작.
        curStageInfo = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();
        curStageInfo.Init();

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

        //트럭관련 Init
        player.GetComponent<CharacterStats>().truck = trucks[0]; //0번트럭은 플레이어, 1, 2, 3번은 AI
        trucks[0].SkinInit(car.currentSkin);
        player.GetComponent<PlayerController>().Init(skin.currentSkin);

        for (int i = 0; i < AIs.Length; i++)
        {
            AIs[i].truck = trucks[i+1];
            trucks[i+1].SkinInit(null);
            curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        }

        Debug.Log($"씬 준비 완료.");

        //플레이어활성화
        player.SetActive(true);

        ////트럭관련 Init
        //player.GetComponent<PlayerController>().Init();
        //for (int i = 0; i < curStageInfo.Ais.Length; i++)
        //{
        //    curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        //}

        //카메라 활성화
        mainCam = Camera.main.GetComponent<CameraMove>();
        mainCam.Init();

        //게임매니저 상태 변경
        State = GameState.Start;

        //게임씬 열기 - UI Indicator연결
        var window = WindowManager.Instance.Open(Windows.InGame);
        var inGmaeWindow = window as InGameWindow;
        var indicators = inGmaeWindow.indicators;
        for (int i = 0; i < 3; i++)
        {
            indicators[i].Init(curStageInfo.Ais[i].GetComponent<AiBehaviour>());
        }

        parking_indicator_ui.Init();
    }

    private void GoEndUi()
    {
        //SceneManager.LoadScene("EndUi", LoadSceneMode.Additive);
        if(winner == player.GetComponent<CharacterStats>())
            WindowManager.Instance.Open(Windows.Win);
        else
            WindowManager.Instance.Open(Windows.Defeated);
    }

    public void EndingScene(CharacterStats winner) //1회 호출.
    {
        //승리자설정
        this.winner = winner;

        //엔딩 루트로 카메라이동
        mainCam.EndingInit(winner);
    }





    /*=========================저장용 함수=========================*/
    public void SaveData()
    {
        SaveSystem.SaveInfo(lastOpenedStage, main, skin, car, sound, randStageInfo);
    }

    public void LoadData()
    {
        SaveData data = SaveSystem.LoadInfo();

        if (data != null)
        {
            lastOpenedStage = data.openStage;

            main.Init(data.isNewSkin, data.isNewCarSkin);
            //main.lastOpenedStage = data.openStage;
            //main.characterSkin.haveNewItem = data.isNewSkin;
            //main.carSkin.haveNewItem = data.isNewCarSkin;

            skin.Init(data.skinOpenMask, data.skinGetMask, data.curSkinIndex);
            //skin.openMask = data.skinOpenMask;
            //skin.buyMask = data.skinGetMask;
            //skin.curSelectedButton = data.curSkinIndex;

            car.Init(data.carSkinOpenMask, data.carSkinGetMask, data.curCarSkinIndex);
            //car.openMask = data.carSkinOpenMask;
            //car.buyMask = data.carSkinGetMask;
            //car.curSelectedButton = data.curCarSkinIndex;

            sound.SetMute(data.isMute);
            sound.SetVibrate(data.nonVibrate);

            Diamond = data.diamond;

            randStageInfo.randStageIndex = data.randStageIndex;
            randStageInfo.randAiIndex = data.randAiIndexs;

            tutorialDone = data.tutorialDone;
        }
        else
        {
            GameManager.GM.lastOpenedStage = 1;
            main.Init(false, false);
            skin.Init(0, 0, -1);
            car.Init(0, 0, -1);
            sound.SetMute(false);
            sound.SetVibrate(false);
            Diamond = 0;

            randStageInfo.randStageIndex = 0;
            randStageInfo.randAiIndex = null;

            tutorialDone = false;
        }
    }



    /*=========================외부 호출용 함수=========================*/
    //승리모션
    public void LetsDance()
    {
        winner.EndAnimation(true);

        if (winner.stats.type == UnitType.Player)
        {
            SoundManager.Instance.PlayWinBgm();
        }
    }

    public void LetsDeafeated()
    {
        var playerStats = player.GetComponent<CharacterStats>();
        var Ais = curStageInfo.Ais;

        if (winner != playerStats)
        {
            playerStats.EndAnimation(false);
            playerStats.BoxUnFreeze();
            SoundManager.Instance.PlayLoseBgm();
        }
        for (int i = 0; i < Ais.Length; i++)
        {
            if (winner != Ais[i])
            {
                Ais[i].EndAnimation(false);
                Ais[i].BoxUnFreeze();
            }
        }

    }


    //돈 : 돈을 습득한 경우 → 1) 보유 돈 증가 함수 호출, 2) UI Text 업데이트
    public void MoneyCollision(CharacterStats unit, Money money)
    {
        unit.getMoney(money.money);
        if (unit.tag == "Player")
        {
            inGame.SetMoneyText();
            SoundManager.Instance.PlayTakeMoney();
        }
    }


    //점수 & 상품 : 아이템을 습득한 경우 → 0) 돈 확인, 1) 보유 아이템 정보 변경 함수 호출, 2) UI Text 업데이트
    public bool ItemCollsion(CharacterStats unit, ItemInfo item)
    {
        //플레이중이 아닐 땐, 아이템먹지않기
        if (GameManager.GM.State != GameState.Play) return false;

        //돈 체크
        if (unit.money >= item.price)
        {
            unit.getStack(item);//Character의 Stats 점수올리기
            unit.GetComponentInChildren<LiftLoad>()?.LiftPurchased(item); //Character의 물건올리기
            //item.SetActive(false);  //비활성화

            if (unit.tag == "Player")
            {
                //score.text = unit.score.ToString();
                //money.text = unit.money.ToString();
                //this.stack.text = unit.itemStack.ToString();
                //gauage.text = unit.score.ToString("P1");
                SoundManager.Instance.PlayTakeItem();
            }

            inGame.SetMoneyText();
            return true;
        }
        return false;
    }

    //끝나면 카메라랑 연동해서 엔딩 연출 나오도록 하는 함수.
    public bool IsEnd(CharacterStats unit)
    {
        if (State == GameState.End) return false;

        if(unit.truck.currentScore >= curStageInfo.goalScore)
        {
            //종료 점수에 도달했다면 게임매니저의 상태를 End로 바꾸면서 현재 우승자 AI 보내기? → 카메라우승자로 이동 → 플레이어로 이동.

            /*종료시 호출할 함수들*/
            State = GameState.End;

            EndingScene(unit);
            return true;
        }
        return false;
    }

    //쟁반끼리 부딫혔을때 판단하는 함수.
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
            if(aStat.stats.type == UnitType.Ai_Tank || aStat.stats.type == UnitType.Ai_Tank2)
            {
                aStat.GetComponent<AiBehaviour>().isCrush = true;
            }    

            bStat.DropItem(forceToB);
            if(aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.4f, 0.2f);
                SoundManager.Instance.Vibrate();

                //사운드
                if (aStat.stats.type == UnitType.Player)
                {
                    SoundManager.Instance.PlayCrushWin();
                }
                else if(bStat.stats.type == UnitType.Player)
                {
                    SoundManager.Instance.PlayCrushLose();
                }
            }
        }
        else
        {
            //a가 상자를 흘림.
            //Debug.Log("b가 더 많다");
            if (bStat.stats.type == UnitType.Ai_Tank || bStat.stats.type == UnitType.Ai_Tank2)
            {
                bStat.GetComponent<AiBehaviour>().isCrush = true;
            }

            aStat.DropItem(forceToA);
            if (aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.4f, 0.2f);
                SoundManager.Instance.Vibrate();

                //사운드
                if (aStat.stats.type == UnitType.Player)
                {
                    SoundManager.Instance.PlayCrushLose();
                }
                else if (bStat.stats.type == UnitType.Player)
                {
                    SoundManager.Instance.PlayCrushWin();
                }
            }
        }

    }
}
