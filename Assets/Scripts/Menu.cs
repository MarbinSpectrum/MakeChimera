using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum UI
{
    Equipment,
    Inventory,
    Craft,
    Reinforce,
    Field,
    Decompose
}

public class Menu : MonoBehaviour
{
    public void OpenUI(string msg)
    {
        UI enumState = (UI)Enum.Parse(typeof(UI), msg);
        switch(enumState)
        {
            case UI.Inventory:
            {
                Inventory.OnOffWindow(true);
            }
            break;
            case UI.Craft:
            {
                Craft.OnOffWindow(true);
            }
            break;
            case UI.Equipment:
            {
                EquipmentUI.OnOffWindow(true);
            }
            break;
            case UI.Field:
            {
                MoveScene.LoadScene("World");
            }
            break;
            case UI.Reinforce:
            {
                Reinforce.OnOffWindow(true);
            }
            break;
            case UI.Decompose:
            {
                DecomposeUI.OnOffWindow(true);
            }
            break;
        }
    }
}
