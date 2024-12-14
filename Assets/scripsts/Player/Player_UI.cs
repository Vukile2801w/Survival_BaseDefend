using Assets.scripsts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Player_UI : MonoBehaviour
{

    [SerializeField] private PauseManager Pause_Menu;

    Player_Controle player_controle;
    InputAction UI_controle_Open;

    private void Awake()
    {
        player_controle = new Player_Controle();
    }

    private void OnEnable()
    {
        UI_controle_Open = player_controle.UI.Open;
        UI_controle_Open.Enable();


    }


    private void OnDisable()
    {
        UI_controle_Open.Disable();
        
    }


    void Update()
    {
        if (UI_controle_Open.triggered)
        {
            Pause_Menu.Toggle_Pause();
        }
    }
}
