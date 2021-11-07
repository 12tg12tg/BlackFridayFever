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
        Idle,   //���� UI ����
        Start,  //���Ӿ� �ε�.
        Play,   //������. �¸����� Ȯ��.
        End,    //����UI ����. - ��ư ������ ������ ������������ �����ִ´�.
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
                    //������ - ���
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

    //�������������� ���� �ҷ����� : ScriptableObject�� �̿��Ͽ� ���� �ҷ��� �� ���� �ʱ�ȭ �� ��.
    public Stage curStageInfo;

    //������
    public CharacterStats winner;

    //�������� UI
    private float endUITimer;
    private bool endSceneAdded;

    //Main or Stage
    public static bool isStage;

    //Save�� ���� �ν��Ͻ�
    [Header("������ ���� �ν��Ͻ�")]
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
                    //3���� �����̶���
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

    public void StartScene()    //���ҷ�����
    {
        isStage = true;
        SaveData();     //���̾ƹ� ���� ����

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
            //Debug.Log("���丮���� ���߽��ϴ�. �������ֽðھ��?");
            StartCoroutine(DoTutorial());
        }
        else
        {
            //�̹� ����
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


    private void StartLevel()   //�����غ�
    {
        //Ai�� �÷��̾��� ��ȣ ����

        //��ư�� ������, �ش� �������� ����.
        curStageInfo = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();
        curStageInfo.Init();

        /*�����۾�*/
        //Truck - Unit ����.
        var trucks = curStageInfo.trucks;
        var AIs = curStageInfo.Ais;

        for (int i = 0; i < trucks.Length; i++)
        {
            int rnd = Random.Range(0, trucks.Length);
            var temp = trucks[i];
            trucks[i] = trucks[rnd];
            trucks[rnd] = temp;
        }

        //Ʈ������ Init
        player.GetComponent<CharacterStats>().truck = trucks[0]; //0��Ʈ���� �÷��̾�, 1, 2, 3���� AI
        trucks[0].SkinInit(car.currentSkin);
        player.GetComponent<PlayerController>().Init(skin.currentSkin);

        for (int i = 0; i < AIs.Length; i++)
        {
            AIs[i].truck = trucks[i+1];
            trucks[i+1].SkinInit(null);
            curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        }

        Debug.Log($"�� �غ� �Ϸ�.");

        //�÷��̾�Ȱ��ȭ
        player.SetActive(true);

        ////Ʈ������ Init
        //player.GetComponent<PlayerController>().Init();
        //for (int i = 0; i < curStageInfo.Ais.Length; i++)
        //{
        //    curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        //}

        //ī�޶� Ȱ��ȭ
        mainCam = Camera.main.GetComponent<CameraMove>();
        mainCam.Init();

        //���ӸŴ��� ���� ����
        State = GameState.Start;

        //���Ӿ� ���� - UI Indicator����
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

    public void EndingScene(CharacterStats winner) //1ȸ ȣ��.
    {
        //�¸��ڼ���
        this.winner = winner;

        //���� ��Ʈ�� ī�޶��̵�
        mainCam.EndingInit(winner);
    }





    /*=========================����� �Լ�=========================*/
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



    /*=========================�ܺ� ȣ��� �Լ�=========================*/
    //�¸����
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


    //�� : ���� ������ ��� �� 1) ���� �� ���� �Լ� ȣ��, 2) UI Text ������Ʈ
    public void MoneyCollision(CharacterStats unit, Money money)
    {
        unit.getMoney(money.money);
        if (unit.tag == "Player")
        {
            inGame.SetMoneyText();
            SoundManager.Instance.PlayTakeMoney();
        }
    }


    //���� & ��ǰ : �������� ������ ��� �� 0) �� Ȯ��, 1) ���� ������ ���� ���� �Լ� ȣ��, 2) UI Text ������Ʈ
    public bool ItemCollsion(CharacterStats unit, ItemInfo item)
    {
        //�÷������� �ƴ� ��, �����۸����ʱ�
        if (GameManager.GM.State != GameState.Play) return false;

        //�� üũ
        if (unit.money >= item.price)
        {
            unit.getStack(item);//Character�� Stats �����ø���
            unit.GetComponentInChildren<LiftLoad>()?.LiftPurchased(item); //Character�� ���ǿø���
            //item.SetActive(false);  //��Ȱ��ȭ

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

    //������ ī�޶�� �����ؼ� ���� ���� �������� �ϴ� �Լ�.
    public bool IsEnd(CharacterStats unit)
    {
        if (State == GameState.End) return false;

        if(unit.truck.currentScore >= curStageInfo.goalScore)
        {
            //���� ������ �����ߴٸ� ���ӸŴ����� ���¸� End�� �ٲٸ鼭 ���� ����� AI ������? �� ī�޶����ڷ� �̵� �� �÷��̾�� �̵�.

            /*����� ȣ���� �Լ���*/
            State = GameState.End;

            EndingScene(unit);
            return true;
        }
        return false;
    }

    //��ݳ��� �΋H������ �Ǵ��ϴ� �Լ�.
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
            //Debug.Log("����");
        }
        else if(aStat.itemStack == 0)
        {
            //a�� ���� �긮�� �Լ� ȣ��
            Debug.Log("a�� ���� �긲");
        }
        else if (bStat.itemStack == 0)
        {
            //a�� ���� �긮�� �Լ� ȣ��
            Debug.Log("b�� ���� �긲");
        }
        else if (aStat.itemStack == bStat.itemStack)
        {
            //�ƹ��ϵ�����
            Debug.Log("���Ƽ� �ƹ��ϵ� �Ͼ�� �ʾƿ�");
        }
        else if (aStat.itemStack > bStat.itemStack)
        {
            //b�� ���ڸ� �긲
            //Debug.Log("a�� �� ����");
            if(aStat.stats.type == UnitType.Ai_Tank || aStat.stats.type == UnitType.Ai_Tank2)
            {
                aStat.GetComponent<AiBehaviour>().isCrush = true;
            }    

            bStat.DropItem(forceToB);
            if(aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.4f, 0.2f);
                SoundManager.Instance.Vibrate();

                //����
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
            //a�� ���ڸ� �긲.
            //Debug.Log("b�� �� ����");
            if (bStat.stats.type == UnitType.Ai_Tank || bStat.stats.type == UnitType.Ai_Tank2)
            {
                bStat.GetComponent<AiBehaviour>().isCrush = true;
            }

            aStat.DropItem(forceToA);
            if (aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.4f, 0.2f);
                SoundManager.Instance.Vibrate();

                //����
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
