using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
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
                    //������ - ���
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

    //Camera
    public CameraMove mainCam;

    //�������������� ���� �ҷ����� : ScriptableObject�� �̿��Ͽ� ���� �ҷ��� �� ���� �ʱ�ȭ �� ��.
    public Stage curStageInfo;

    //������
    private CharacterStats winner;

    //�������� UI
    private float endUITimer;
    private bool endSceneAdded;

    private void Awake()
    {
        //Debug.Log($"GameManager Awake : {Time.time}");
        //�̱��� �� ���� �ʱ�ȭ : �ϴ� Idle�� �ʱ�ȭ ���� ����. 
        instance = this;

    }
    private void Start()
    {
        State = GameState.Idle;

        //Ai�� �÷��̾��� ��ȣ ����
        StartLevel();
        Debug.Log($"�� �غ� �Ϸ�.");

        //�÷��̾�Ȱ��ȭ
        player.SetActive(true);

        //Ʈ������ Init
        player.GetComponent<PlayerController>().Init();
        for (int i = 0; i < curStageInfo.Ais.Length; i++)
        {
            curStageInfo.Ais[i].GetComponent<AiBehaviour>().Init();
        }

        //ī�޶� Ȱ��ȭ
        mainCam = Camera.main.GetComponent<CameraMove>();
        mainCam.Init();

        //���ӸŴ��� ���� ����
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
                if(mainCam.isSecondWinnerMoveEnd)
                {
                    //2���� �����̶���
                    endUITimer += Time.deltaTime;
                    if (!endSceneAdded && endUITimer > 2f)
                    {
                        GoEndUi();
                        endSceneAdded = true;
                    }
                }
                break;
        }
    }

    private void StartLevel()
    {
        //��ư�� ������, �ش� �������� ����.
        curStageInfo = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();

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

        player.GetComponent<CharacterStats>().truck = trucks[0]; //0��Ʈ���� �÷��̾�, 1, 2, 3���� AI

        for (int i = 0; i < AIs.Length; i++)
        {
            AIs[i].truck = trucks[i+1];
        }
    }

    private void GoToMain()
    {
        SceneManager.LoadScene("main");
    }
    private void GoNextLevel()
    {

    }
    private void GoEndUi()
    {
        SceneManager.LoadScene("EndUi", LoadSceneMode.Additive);
    }

    public void EndingScene(CharacterStats winner) //1ȸ ȣ��.
    {
        //�¸��ڼ���
        this.winner = winner;

        //���� ��Ʈ�� ī�޶��̵�
        mainCam.EndingInit(winner);
    }














    /*=========================�ܺ� ȣ��� �Լ�=========================*/
    //�¸����
    public void LetsDance()
    {
        winner.EndAnimation(true);
    }

    public void LetsDeafeated()
    {
        var playerStats = player.GetComponent<CharacterStats>();
        var Ais = curStageInfo.Ais;

        if (winner != playerStats)
        {
            playerStats.EndAnimation(false);
            playerStats.BoxUnFreeze();
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
            this.money.text = unit.money.ToString();
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
            unit.GetComponentInChildren<LiftLoad>().LiftPurchased(item); //Character�� ���ǿø���
            //item.SetActive(false);  //��Ȱ��ȭ

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

    //������ ī�޶�� �����ؼ� ���� ���� �������� �ϴ� �Լ�.
    public bool IsEnd(CharacterStats unit)
    {
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
            bStat.DropItem(forceToB);
            if(aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.5f, 0.2f);
                //Debug.Log("��鸲�� ������");
            }
        }
        else
        {
            //a�� ���ڸ� �긲.
            //Debug.Log("b�� �� ����");
            aStat.DropItem(forceToA);
            if (aStat.stats.type == UnitType.Player || bStat.stats.type == UnitType.Player)
            {
                Camera.main.GetComponent<CameraMove>().ShakeCamera(1.5f, 0.5f, 0.2f);
                //Debug.Log("��鸲�� ������");
            }
        }

    }
}
