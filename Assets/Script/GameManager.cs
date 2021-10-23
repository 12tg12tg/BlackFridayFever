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
        Start,  //���Ӿ� �ε�
        Play,   //������. �¸����� Ȯ��.
        End,    //����UI ����
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

    //�������������� ���� �ҷ����� : ScriptableObject�� �̿��Ͽ� ���� �ҷ��� �� ���� �ʱ�ȭ �� ��.
    private Stage curStage;

    private void Awake()
    {
        Debug.Log($"GameManager Awake : {Time.time}");
    }
    private void Start()
    {
        //�̱��� �� ���� �ʱ�ȭ : �ϴ� Idle�� �ʱ�ȭ ���� ����. 
        instance = this;
        State = GameState.Play;

        //�÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player");

        StartLevel();



    }

    private void StartLevel()
    {
        //��ư�� ������, �ش� �������� ����.
        curStage = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();

        //==== ���� Ȱ�� ������ �ʿ��� ������Ʈ ã�� - ���� ====
        //Scene curScene = SceneManager.GetActiveScene();
        //var gos = curScene.GetRootGameObjects();

        //1. Truck ã��
        //foreach (var go in gos)
        //{
        //    var truckScripts = go.GetComponentsInChildren<TruckScript>();
        //    foreach (var truck in truckScripts)
        //    {
        //        trucks.Add(truck);
        //    }
        //}

        //2. AI ã��
        //foreach (var go in gos)
        //{
        //    if (go.tag == "AI")
        //    {
        //        AIs.Add(go.GetComponent<CharacterStats>());
        //    }
        //}

        /*�����۾�*/
        //Truck - Unit ����.
        var trucks = curStage.trucks;
        var AIs = curStage.Ais;

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




    /*=========================�ܺ� ȣ��� �Լ�=========================*/
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

    public bool IsEnd(CharacterStats unit)
    {
        if(unit.truck.currentScore >= curStage.goalScore)
        {
            //���� ������ �����ߴٸ� ���ӸŴ����� ���¸� End�� �ٲٸ鼭 ���� ����� AI ������? �� ī�޶����ڷ� �̵� �� �÷��̾�� �̵�.
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
        }
        else
        {
            //a�� ���ڸ� �긲.
            //Debug.Log("b�� �� ����");
            aStat.DropItem(forceToA);
        }

    }
}
