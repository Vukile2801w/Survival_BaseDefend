using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Main_Menu_Manager : MonoBehaviour
{
    

    public void Play(string scene_name)
    {
        Debug.LogAssertion($"Loading {scene_name}");

        // Proverite da li je ime scene postavljeno
        if (!string.IsNullOrEmpty(scene_name))
        {
            SceneManager.LoadScene(scene_name);
        }
        else
        {
            Debug.LogError("Ime scene nije postavljeno u 'sceneToLoad'.");
        }
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
