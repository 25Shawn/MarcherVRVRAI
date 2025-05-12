using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Classe qui fait la gestion du changement de nouvelles
/// </summary>
public class Nouvelles : MonoBehaviour
{
    public TextMeshPro newsText;
    public string[] news = {
        "L'économie mondiale connaît une forte croissance !",
        "La bourse subit une baisse importante des actions.",
        "Une nouvelle fusion d'entreprises promet de nouveaux horizons.",
        "Les taux d'intérêt augmentent, un impact négatif pour les investisseurs."
    };
    private int currentIndex = 0;

    public Animation warningAnimation;
    public GameObject lumiereAlarmeRougeGameObject;
    public float vitesseDApparition = 3f;

    /// <summary>
    /// Méthode de commencement qui met la lumière rouge inactive et par la coroutine pour afficher les nouvelles en les défilants
    /// </summary>
    void Start()
    {
        lumiereAlarmeRougeGameObject.gameObject.SetActive(false);
        StartCoroutine(AffichageNouvellesBoucle());
    }

    /// <summary>
    /// Méthode qui affiche au 3 secondes une nouvelle nouvelle.
    /// </summary>
    /// <returns>le delais en seconde</returns>
    IEnumerator AffichageNouvellesBoucle()
    {

        while (true)
        {
            
            newsText.text = news[currentIndex];
            MiseAjourNouvelle();

            
            yield return new WaitForSeconds(vitesseDApparition);


            // Passer à la prochaine nouvelle
            currentIndex = (currentIndex + 1) % news.Length;
        }
    }

    /// <summary>
    /// Méthode qui fait la gestion du retour de valeur 
    /// Donc,si la nouvelle est bonne on lui retourne une valeur de 5 a 11
    /// si la novuelle est mauvaise on lui retroune une valeur entre 1 et 5
    /// et si la valeur est 1 ou 2 la lumiere rouge s'allume.
    /// </summary>
    void MiseAjourNouvelle()
    {
        if (ScriptGestionGameOver.estEnGameOver) return;

        // utilisation de chatGBT pour cette ligne de code
        // Cela fait que si la nouvelle a la position currentIndex contient le mot croissance ou fusion on lui assigne un true, sinon c'est false.
        bool isGoodNews = news[currentIndex].Contains("croissance") || news[currentIndex].Contains("fusion");

        if (isGoodNews)
        {
            newsText.color = Color.green;
            int randomValue = Random.Range(5, 11);
            lumiereAlarmeRougeGameObject.gameObject.SetActive(false);
           
        }
        else
        {
            newsText.color = Color.red;
            int randomValue = Random.Range(1, 5);
            

            bool activateRedLight = (randomValue == 1 || randomValue == 2);

            if (warningAnimation != null)
            {
                if (activateRedLight)
                {
                    warningAnimation.Play();
                    lumiereAlarmeRougeGameObject.gameObject.SetActive(true);
                   
                }
                else
                {
                    warningAnimation.Stop();
                    lumiereAlarmeRougeGameObject.gameObject.SetActive(false);
                    
                }
            }
        }
    }
}
