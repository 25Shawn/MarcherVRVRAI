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
    public TextMeshProUGUI textePrixActuel;
    public TMP_InputField champQuantite;
    public Button boutonAcheter;
    public Button boutonVendre;

    void Update()
    {
        // Prot�ge contre les exceptions si l�objet est d�truit � runtime
        if (graphique == null || joueur == null) return;

        try
        {
            // --- Affichage dynamique du prix actuel ---
            if (textePrixActuel != null && !textePrixActuel.IsDestroyed())
            {
                textePrixActuel.text = $"Prix actuel : {graphique.prixActuel:F2}$";
            }

            // --- Interaction du bouton "Acheter" ---
            if (boutonAcheter != null && champQuantite != null && !champQuantite.IsDestroyed())
            {
                boutonAcheter.interactable = joueur.PeutAcheter(graphique.prixActuel, GetQuantite());
            }

            // --- Interaction du bouton "Vendre" ---
            if (boutonVendre != null)
            {
                boutonVendre.interactable = joueur.PeutVendre(nomMarche);
            }
        }
        catch (MissingReferenceException e)
        {
            Debug.LogWarning($"[UITradingMarche - {nomMarche}] R�f�rence d�truite : {e.Message}");
        }
    }

    int GetQuantite()
    {
        if (champQuantite == null || champQuantite.text == "") return 0;
        if (int.TryParse(champQuantite.text, out int quantite))
            return Mathf.Max(quantite, 1);
        return 0;
    }

    public void ActionAcheter()
    {
        if (joueur == null || graphique == null) return;
        int quantite = GetQuantite();
        float prix = graphique.prixActuel;
        joueur.Acheter(nomMarche, prix, quantite);
    }

    public void ActionVendre()
    {
        if (joueur == null || graphique == null) return;
        float prix = graphique.prixActuel;
        joueur.VendreTout(nomMarche, prix);
    }
}
