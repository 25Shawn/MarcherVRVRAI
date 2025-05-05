using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVR : MonoBehaviour
{
    public void CommencerJeu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitterJeu()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Arr�te le mode Play dans l'�diteur
#else
        Application.Quit(); // Ferme dans un vrai jeu (build)
#endif
    }
}
