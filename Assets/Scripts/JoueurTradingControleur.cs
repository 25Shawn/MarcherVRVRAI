using System.Collections.Generic;
using UnityEngine;

public class JoueurTradingControleur : MonoBehaviour
{
    [Header("Finances du joueur")]
    public float argentInitial = 1000f;
    public float argentActuel;

    [Header("Perte automatique")]
    public float perteParSeconde = 1f;

    // Structure pour représenter un achat en cours
    [System.Serializable]
    public class Achat
    {
        public float prixUnitaire;
        public int quantite;

        public Achat(float prix, int qte)
        {
            prixUnitaire = prix;
            quantite = qte;
        }
    }

    // Structure pour représenter une transaction (historique)
    [System.Serializable]
    public class Transaction
    {
        public enum Type { Achat, Vente }

        public Type type;
        public string marche;
        public float prix;
        public int quantite;
        public float montantTotal;
        public float profit;

        public Transaction(Type type, string marche, float prix, int quantite, float profit = 0f)
        {
            this.type = type;
            this.marche = marche;
            this.prix = prix;
            this.quantite = quantite;
            this.montantTotal = prix * quantite;
            this.profit = profit;
        }
    }

    // Portefeuille : marché → liste d’achats en cours
    private Dictionary<string, List<Achat>> portefeuille = new Dictionary<string, List<Achat>>();

    // Historique complet des transactions
    public List<Transaction> historiqueTransactions = new List<Transaction>();

    void Start()
    {
        argentActuel = argentInitial;
    }

    private void Update()
    {
        AppliquerPertePassive();
    }

    /// <summary>
    /// Le joueur achète un actif sur un marché donné
    /// </summary>
    public bool Acheter(string nomMarche, float prixUnitaire, int quantite)
    {
        float coutTotal = prixUnitaire * quantite;
        if (argentActuel < coutTotal)
        {
            Debug.LogWarning("Pas assez d’argent pour cet achat.");
            return false;
        }

        argentActuel -= coutTotal;

        if (!portefeuille.ContainsKey(nomMarche))
        {
            portefeuille[nomMarche] = new List<Achat>();
        }

        portefeuille[nomMarche].Add(new Achat(prixUnitaire, quantite));

        // Enregistrer dans l'historique
        historiqueTransactions.Add(new Transaction(Transaction.Type.Achat, nomMarche, prixUnitaire, quantite));

        Debug.Log($"Achat de {quantite} unités sur {nomMarche} à {prixUnitaire:F2}$ - Total dépensé : {coutTotal:F2}$ - Solde : {argentActuel:F2}$");
        return true;
    }

    /// <summary>
    /// Le joueur vend tout ce qu’il possède sur un marché donné au prix actuel
    /// </summary>
    public bool VendreTout(string nomMarche, float prixVente)
    {
        if (!portefeuille.ContainsKey(nomMarche) || portefeuille[nomMarche].Count == 0)
        {
            Debug.LogWarning($"Aucun actif à vendre sur le marché {nomMarche}.");
            return false;
        }

        float gainTotal = 0f;
        float coutTotal = 0f;
        int totalQuantite = 0;

        foreach (Achat achat in portefeuille[nomMarche])
        {
            gainTotal += prixVente * achat.quantite;
            coutTotal += achat.prixUnitaire * achat.quantite;
            totalQuantite += achat.quantite;
        }

        float profit = gainTotal - coutTotal;
        argentActuel += gainTotal;

        portefeuille[nomMarche].Clear(); // vide les actifs du marché

        // Enregistrer dans l'historique
        historiqueTransactions.Add(new Transaction(Transaction.Type.Vente, nomMarche, prixVente, totalQuantite, profit));

        Debug.Log($"Vente de {totalQuantite} unités sur {nomMarche} à {prixVente:F2}$ - Gain : {gainTotal:F2}$ - Profit net : {profit:F2}$ - Solde : {argentActuel:F2}$");
        return true;
    }

    public bool PeutAcheter(float prixUnitaire, int quantite)
    {
        return argentActuel >= prixUnitaire * quantite;
    }

    public bool PeutVendre(string nomMarche)
    {
        return portefeuille.ContainsKey(nomMarche) && portefeuille[nomMarche].Count > 0;
    }

    private void AppliquerPertePassive()
    {
        float perte = perteParSeconde * Time.deltaTime;
        argentActuel = Mathf.Max(0f, argentActuel - perte);
    }
}
