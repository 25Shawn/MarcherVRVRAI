using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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
    public TextMeshPro texteArgentRestant;
    private int quantite = 1;

    [Header("Effets sonores")]
    public AudioClip sonAchat;
    public AudioClip sonVente;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (graphique == null || joueur == null) return;

        try
        {
            // --- Affichage du prix actuel ---
            if (textePrixActuel != null && !textePrixActuel.IsDestroyed())
            {
                textePrixActuel.text = $"Prix actuel : {graphique.prixActuel:F2}$";
            }

            // --- Affichage de la quantit� ---
            if (champQuantite != null && !champQuantite.IsDestroyed())
            {
                champQuantite.text = $"Qtt : {quantite.ToString()}";
            }

            // --- Affichage de l'argent ---
            if (texteArgentRestant != null && !texteArgentRestant.IsDestroyed())
            {
                texteArgentRestant.text = $"Argent : {joueur.argentActuel:F2}$";
            }

            // --- Contr�les clavier (Input System) ---
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.aKey.wasPressedThisFrame)
            {
                ActionAcheter();
                Debug.Log(">>> Achat d�clench� par touche A");
            }

            if (keyboard.vKey.wasPressedThisFrame)
            {
                ActionVendre();
                Debug.Log(">>> Vente d�clench�e par touche V");
            }

            if (keyboard.pKey.wasPressedThisFrame)
            {
                AugmenterQuantite();
                Debug.Log(">>> Quantit� augment�e (P)");
            }

            if (keyboard.mKey.wasPressedThisFrame)
            {
                DiminuerQuantite();
                Debug.Log(">>> Quantit� diminu�e (M)");
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
        if (sonAchat != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonAchat);
        }
    }

    public void ActionVendre()
    {
        if (joueur == null || graphique == null) return;
        joueur.VendreTout(nomMarche, graphique.prixActuel);

        if (sonAchat != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonVente);
        }

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
