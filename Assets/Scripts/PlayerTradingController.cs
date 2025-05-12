using UnityEngine;

public class PlayerTradingController : MonoBehaviour
{
    [Header("Finances du joueur")]
    public float startingMoney = 1000f;
    public float currentMoney;
    public bool hasAsset = false;
    public float buyPrice = 0f;

    void Start()
    {
        currentMoney = startingMoney;
    }

    // Appel� quand le joueur ach�te
    public bool Buy(float marketPrice)
    {
        if (hasAsset) return false; // d�j� un actif, pas de double achat
        if (currentMoney < marketPrice) return false; // pas assez d'argent

        buyPrice = marketPrice;
        hasAsset = true;
        currentMoney -= marketPrice;
        Debug.Log($"Achat � {marketPrice:F2} - Argent restant : {currentMoney:F2}");
        return true;
    }

    // Appel� quand le joueur vend
    public bool Sell(float marketPrice)
    {
        if (!hasAsset) return false;

        float profit = marketPrice - buyPrice;
        currentMoney += marketPrice;
        hasAsset = false;

        Debug.Log($"Vente � {marketPrice:F2} - Profit : {profit:F2} - Argent total : {currentMoney:F2}");
        return true;
    }
}
