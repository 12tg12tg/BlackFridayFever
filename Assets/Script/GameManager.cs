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
        Play,   //��߽�ȣ - Door ����
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
                    //������
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

    //�������������� ���� �ҷ����� : ScriptableObject�� �̿��Ͽ� ���� �ҷ��� �� ���� �ʱ�ȭ �� ��.
    //Timer
    private float startDelay = 10f;
    private float startTimer;
    private int goalScore = 100;



    private void Awake()
    {
        //�̱��� �� ���� �ʱ�ȭ : �ϴ� Idle�� �ʱ�ȭ ���� ����. 
        instance = this;
        State = GameState.Start;

        //�÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player");

        //���� Ȱ�� ������ �ʿ��� ������Ʈ ã��         //SceneManager.GetActiveScene().GetRootGameObjects();
        //1. Door ã��
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

        //2. Truck ã��
        foreach (var go in gos)
        {
            var truckScripts = go.GetComponentsInChildren<TruckScript>();
            foreach (var truck in truckScripts)
            {
                trucks.Add(truck);
            }
        }

        //3. AI ã��
        foreach (var go in gos)
        {
            if (go.tag == "AI")
            {
                AIs.Add(go.GetComponent<CharacterStats>());
            }
        }

        //�����۾�
        //Truck - Unit ����.
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
                //������
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
            unit.getStack(item);//�����ø���
            unit.GetComponentInChildren<LiftLoad>().LiftPurchased(0); //���ǿø���
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
        if(unit.truck.currentScore >= goalScore)
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

        if(aStat.belongings.Count == 0 && bStat.belongings.Count == 0)
        {
            //Nothing
            //Debug.Log("����");
        }
        else if(aStat.belongings.Count == 0)
        {
            //a�� ���� �긮�� �Լ� ȣ��
            Debug.Log("a�� ���� �긲");
        }
        else if (bStat.belongings.Count == 0)
        {
            //a�� ���� �긮�� �Լ� ȣ��
            Debug.Log("b�� ���� �긲");
        }
        else if (aStat.belongings.Count == bStat.belongings.Count)
        {
            //�ƹ��ϵ�����
            Debug.Log("���Ƽ� �ƹ��ϵ� �Ͼ�� �ʾƿ�");
        }
        else if (aStat.belongings.Count > bStat.belongings.Count)
        {
            //b�� ���ڸ� �긲
            //Debug.Log("a�� �� ����");
            bStat.DropItem();
        }
        else
        {
            //a�� ���ڸ� �긲.
            //Debug.Log("b�� �� ����");
            aStat.DropItem();
        }

    }
}
