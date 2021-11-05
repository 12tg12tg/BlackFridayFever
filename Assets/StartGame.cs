using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject roamingAI;
    public GameObject ragdollPrefab;
    public GameObject moneyPrefab;
    public GameObject lowBoxPrefab;
    public GameObject midBoxPrefab;
    public GameObject highBoxPrefab;
    public GameObject item_Low_Donut_Prefab;
    public GameObject item_Low_Hamburger_Prefab;
    public GameObject item_Low_Burrito_Prefab;
    public GameObject item_Low_Pizza_Prefab;
    public GameObject item_Low_Shoe_Prefab;
    public GameObject item_Mid_Dog_Prefab;
    public GameObject item_Mid_Cat_Prefab;
    public GameObject item_Mid_Album_Prefab;
    public GameObject item_Mid_Banana_Prefab;
    public GameObject item_Mid_Cake_Prefab;
    public GameObject item_Mid_Pineapple_Prefab;
    public GameObject item_Mid_Shoe_Prefab;
    public GameObject item_High_Ring_Prefab;
    public GameObject item_High_Necklace_Prefab;
    public GameObject item_High_Guitar_Prefab;
    public GameObject item_High_Monitor_Prefab;
    public GameObject item_High_Sushi_Prefab;

    private void Start()
    {
        GameObjectPool.Instance.Init(this);

        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("Stage0", LoadSceneMode.Additive);
    }
}
