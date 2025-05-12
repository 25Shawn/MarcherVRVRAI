using UnityEngine;

public class ScriptGestionGameOver : MonoBehaviour
{
    public GameObject GameOver;

    public static bool estEnGameOver = false;

    void Start()
    {
        GameOver.SetActive(false);
        estEnGameOver = false;
    }

    void Update()
    {

        if (estEnGameOver)
        {
            GameOver.gameObject.SetActive(true);
        }
        else
        {
            GameOver.gameObject.SetActive(false);
        }
    }
}
