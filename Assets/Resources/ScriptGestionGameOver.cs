using UnityEngine;

public class ScriptGestionGameOver : MonoBehaviour
{


    public GameObject GameOver;
    void Start()
    {
        GameOver.gameObject.SetActive(false);
    }


    public bool ArretDuJeu(GameObject gameObject)
    {
        if (gameObject.gameObject.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
