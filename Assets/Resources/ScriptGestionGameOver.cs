using UnityEngine;

public class ScriptGestionGameOver : MonoBehaviour
{
    public GameObject GameOver;

    public static bool estEnGameOver = false;

    void Start()
    {
        GameOver.SetActive(true);
        estEnGameOver = false;
    }

    void Update()
    {
        
        if (GameOver.activeInHierarchy)
        {
            estEnGameOver = true;
        }
        else
        {
            estEnGameOver = false;
        }
    }
}
