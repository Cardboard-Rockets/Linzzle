using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{

    public TMPro.TextMeshProUGUI money_text;

    [SerializeField] private int startingMoney = 100;
    [SerializeField] private int[] startingPrices;

    static public int money;
    static public int[] price;

    
    void Awake()
    {
        money = startingMoney;
        price = startingPrices;
    }

    void Update()
    {
        UpdateBalance();
    }

    public static void buy(int tileid)
    {
        money -= price[tileid];
    }

    void UpdateBalance()
    {
        money_text.text = money.ToString();
    }

    public static bool isAvailable()
    {
        return money >= price[PlaceScript.tileid];
    }
}
