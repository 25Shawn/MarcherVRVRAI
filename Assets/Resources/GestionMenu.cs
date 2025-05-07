using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe qui fait la gestion des boutons de menu 
/// </summary>
public class MainMenuVR : MonoBehaviour
{
    /// <summary>
    /// M�thode qui fait charger la scene de jeu
    /// </summary>
    public void CommencerJeu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// M�thode qui quitte le jeu
    /// </summary>
    public void QuitterJeu()
    {

        EditorApplication.isPlaying = false;
    }
}
