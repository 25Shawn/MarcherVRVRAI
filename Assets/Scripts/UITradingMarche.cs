using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Gère l'interface d'achat/vente pour un marché.
/// </summary>
public class UITradingMarche : MonoBehaviour
{
    [Header("Références logiques")]
    public string nomMarche;
    public JoueurTradingControleur joueur;
    public Graph3D graphique;

    [Header("Composants UI")]
    public TextMeshPro textePrixActuel;
    public TextMeshPro champQuantite;
    private int quantite = 1;


    void Update()
    {
        if (graphique == null || joueur == null) return;

        try
        {
            // Prix
            if (textePrixActuel != null && !textePrixActuel.IsDestroyed())
            {
                textePrixActuel.text = $"Prix actuel : {graphique.prixActuel:F2}$";
            }

            // Quantité (à afficher chaque frame pour garantir la synchro)
            if (champQuantite != null && !champQuantite.IsDestroyed())
            {
                champQuantite.text = quantite.ToString();
            }
        }
        catch (MissingReferenceException e)
        {
            Debug.LogWarning($"[UITradingMarche - {nomMarche}] Référence détruite : {e.Message}");
        }
    }


    int GetQuantite()
    {
        return Mathf.Max(quantite, 1);
    }

    public void ActionAcheter()
    {
        if (joueur == null || graphique == null) return;
        joueur.Acheter(nomMarche, graphique.prixActuel, GetQuantite());
    }

    public void ActionVendre()
    {
        if (joueur == null || graphique == null) return;
        joueur.VendreTout(nomMarche, graphique.prixActuel);
    }

    public void AugmenterQuantite()
    {
        quantite++;
        UpdateChampQuantite();
    }

    public void DiminuerQuantite()
    {
        quantite = Mathf.Max(1, quantite - 1);
        UpdateChampQuantite();
    }

    private void UpdateChampQuantite()
    {
        Debug.Log("[UITradingMarche] Mise à jour quantité affichée : " + quantite);

        if (champQuantite != null)
        {
            champQuantite.text = quantite.ToString();
        }
        else
        {
            Debug.LogWarning("[UITradingMarche] champQuantite n'est pas assigné !");
        }
    }
}
