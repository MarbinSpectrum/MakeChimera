using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPlayerSlot : MonoBehaviour
{
    [SerializeField] private EquipmentSlot slot;
    [SerializeField] private string slotType;

    public void ClickSlot()
    {
        EquipmentUI.SelectMenu(slotType);
        slot.ShowItem();
    }
}
