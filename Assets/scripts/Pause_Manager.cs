using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool Is_Paused = false; // Praćenje stanja pauze

    public GameObject Pause_Menu_UI; // UI element za meni pauze (opciono)

    // Poziv ove metode prebacuje stanje pauze
    public void Toggle_Pause()
    {
        if (Is_Paused)
        {
            Resume_Game();
        }
        else
        {
            Pause_Game();
        }
    }

    // Pauzira igru
    public void Pause_Game()
    {
        Time.timeScale = 0f; // Pauzira sve vremenski zavisne funkcije
        Is_Paused = true;

        if (Pause_Menu_UI != null)
        {
            Pause_Menu_UI.SetActive(true); // Prikazuje meni pauze (opciono)
        }
    }

    // Nastavlja igru
    public void Resume_Game()
    {
        Debug.Log("Resume button clicked"); // Dodato za testiranje

        Time.timeScale = 1f; // Vraća igru u normalno stanje
        Is_Paused = false;

        if (Pause_Menu_UI != null)
        {
            Pause_Menu_UI.SetActive(false); // Sakriva meni pauze (opciono)
        }
    }

    
}
