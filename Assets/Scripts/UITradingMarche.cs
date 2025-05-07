using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// G�re l'interface d'achat/vente pour un march�.
/// </summary>
public class UITradingMarche : MonoBehaviour
{
    [Header("R�f�rences logiques")]
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

            // Quantit� (� afficher chaque frame pour garantir la synchro)
            if (champQuantite != null && !champQuantite.IsDestroyed())
            {
                champQuantite.text = quantite.ToString();
            }
        }
        catch (MissingReferenceException e)
        {
            Debug.LogWarning($"[UITradingMarche - {nomMarche}] R�f�rence d�truite : {e.Message}");
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
        Debug.Log("[UITradingMarche] Mise � jour quantit� affich�e : " + quantite);

        if (champQuantite != null)
        {
            champQuantite.text = quantite.ToString();
        }
        else
        {
            Debug.LogWarning("[UITradingMarche] champQuantite n'est pas assign� !");
        }
    }
}
