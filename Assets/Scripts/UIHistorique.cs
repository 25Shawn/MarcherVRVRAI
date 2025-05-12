using UnityEngine;
using TMPro;

public class UIHistorique : MonoBehaviour
{
    [Header("Références")]
    public JoueurTradingControleur joueur;
    public TextMeshPro texteHistorique;

    [Header("Paramètres d'affichage")]
    public int maxLignes = 10;

    private int derniereTailleHistorique = 0;

    void Update()
    {
        if (joueur == null || texteHistorique == null)
            return;

        // Seulement mettre à jour si l'historique a changé
        if (joueur.historiqueTransactions.Count != derniereTailleHistorique)
        {
            derniereTailleHistorique = joueur.historiqueTransactions.Count;
            MettreAJourHistorique();
        }
    }

    void MettreAJourHistorique()
    {
        var lignes = joueur.historiqueTransactions;
        int debut = Mathf.Max(0, lignes.Count - maxLignes);

        string texte = "Historique :\n";

        for (int i = debut; i < lignes.Count; i++)
        {
            var t = lignes[i];
            if (t.type == JoueurTradingControleur.Transaction.Type.Vente)
            {
                string signe = t.profit >= 0 ? "+" : "-";
                texte += $"Vente - {t.marche} - {t.quantite}unité(s) à {t.prix:F2}$ ({signe}{Mathf.Abs(t.profit):F2}$)\n";
            }
            else
            {
                texte += $"Achat - {t.marche} - {t.quantite}unité(s) à {t.prix:F2}$\n";
            }
        }

        texteHistorique.text = texte;
    }
}
