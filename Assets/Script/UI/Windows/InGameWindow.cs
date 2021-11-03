using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameWindow : GenericWindow
{
    public Indicator_AI[] indicators;

    public Text moneyText;
    public CharacterStats playerStats;

    public void SetMoneyText()
    {
        moneyText.text = playerStats.money.ToString();
    }
}
