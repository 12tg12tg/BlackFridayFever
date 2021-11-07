using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        GoogleMobileADTest.Init();

        StartCoroutine(CoFadeInOut());
    }

    public Image logo;
    private IEnumerator CoFadeInOut()
    {
        float timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            var lerp = Mathf.Lerp(0, 1, timer / 0.3f);
            var curColor = logo.color;
            curColor.a = lerp;
            logo.color = curColor;
            yield return null;
        }

        timer = 0f;
        yield return new WaitForSeconds(0.8f);

        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            var lerp = Mathf.Lerp(1, 0, timer / 0.3f);
            var curColor = logo.color;
            curColor.a = lerp;
            logo.color = curColor;
            yield return null;
        }

        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("Stage0", LoadSceneMode.Additive);
    }
}
