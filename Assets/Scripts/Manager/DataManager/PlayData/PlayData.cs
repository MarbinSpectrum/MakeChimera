using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayData
{
    public int nowStage;
    public List<string> equipmentsData = new List<string>();
    public List<string> materialsData = new List<string>();
    public List<string> nowEquipmentsData = new List<string>();
    public List<string> nowUpgradesData = new List<string>();
    public bool firstPlay = true;

    public void SaveData()
    {
        equipmentsData.Clear();
        materialsData.Clear();
        nowEquipmentsData.Clear();
        nowUpgradesData.Clear();

        foreach (ItemClass temp in equipments)
            equipmentsData.Add(ItemClass.MakeString(temp));
        foreach (ItemClass temp in materials)
            materialsData.Add(ItemClass.MakeString(temp));
        foreach (ItemClass temp in nowEquipments)
            nowEquipmentsData.Add(ItemClass.MakeString(temp));
        foreach (UpgradeClass temp in nowUpgrades)
            nowUpgradesData.Add(UpgradeClass.MakeString(temp));
    }
    public void LoadData()
    {
        equipments.Clear();
        materials.Clear();
        nowEquipments.Clear();
        nowUpgrades.Clear();

        foreach (string temp in equipmentsData)
            equipments.Add(ItemClass.MakeItemClass(temp));
        foreach (string temp in materialsData)
            materials.Add(ItemClass.MakeItemClass(temp));
        foreach (string temp in nowEquipmentsData)
            nowEquipments.Add(ItemClass.MakeItemClass(temp));
        foreach (string temp in nowUpgradesData)
            nowUpgrades.Add(UpgradeClass.MakeItemClass(temp));
    }

    public List<ItemClass> equipments = new List<ItemClass>();
    public List<ItemClass> materials = new List<ItemClass>();
    public List<ItemClass> nowEquipments = new List<ItemClass>();
    public List<UpgradeClass> nowUpgrades = new List<UpgradeClass>();
    public int GetMaterialNum(string itemDataName)
    {
        foreach (ItemClass item in materials)
            if (item.itemDataName == itemDataName)
                return item.itemNum;
        return 0;
    }
    public bool HasMaterial(string itemDataName, int itemNum = 1)
    {
        foreach (ItemClass item in materials)
            if (item.itemDataName == itemDataName && item.itemNum >= itemNum)
                return true;
        return false;
    }
    public bool AddMaterial(string itemDataName, int itemNum)
    {
        if (HasMaterial(itemDataName) == false)
        {
            materials.Add(new ItemClass(itemDataName, ItemType.material, itemNum));
            return true;
        }
        else
        {
            foreach (ItemClass item in DataManager.Instance.playData.materials)
                if (item.itemDataName == itemDataName)
                {
                    item.itemNum += itemNum;
                    DataManager.SaveData();
                    return true;
                }
            return false;
        }
    }
    public bool RemoveMaterial(string itemDataName, int itemNum = 1)
    {
        for(int i = 0; i < materials.Count; i++)
            if (materials[i].itemDataName == itemDataName)
            {
                materials[i].itemNum -= itemNum;
                if(materials[i].itemNum == 0)
                {
                    materials.RemoveAt(i);
                    DataManager.SaveData();
                }
                return true;
            }
        return false;
    }
    public bool HasEquipment(string itemDataName, int itemNum = 1)
    {
        foreach (ItemClass item in equipments)
            if (item.itemDataName == itemDataName && item.itemNum >= itemNum)
                return true;
        foreach (ItemClass item in nowEquipments)
            if (item.itemDataName == itemDataName && item.itemNum >= itemNum)
                return true;
        return false;
    }
    public bool RemoveEquipment(string itemDataName, int itemNum = 1)
    {
        foreach (ItemClass item in equipments)
            if (item.itemDataName == itemDataName)
            {
                item.itemNum -= itemNum;
                DataManager.SaveData();
                return true;
            }
        foreach (ItemClass item in nowEquipments)
            if (item.itemDataName == itemDataName)
            {
                item.itemNum -= itemNum;
                DataManager.SaveData();
                return true;
            }
        return false;
    }
    //�κ��丮�� ����
    public bool RemoveEquipment(ItemClass itemClass)
    {
        for (int i = 0; i < equipments.Count; i++)
            if (equipments[i].Equals(itemClass))
            {
                equipments.RemoveAt(i);
                DataManager.SaveData();
                return true;
            }
        return false;
    }
    public ItemClass GetNowEquip(EquipmentType equipmentType)
    {
        ItemManager itemManager = ItemManager.Instance;

        foreach (ItemClass item in nowEquipments)
        {
            ItemData itemData = itemManager.equipment_Items[item.itemDataName];
            EquipmentData equipmentData = (EquipmentData)itemData;
            if (equipmentData.equipmentType == equipmentType)
                return item;
        }

        return null;
    }
    public int GetNowEquipIdx(EquipmentType equipmentType)
    {
        ItemManager itemManager = ItemManager.Instance;

        for (int i = 0; i < nowEquipments.Count; i++)
        {
            ItemData itemData = itemManager.equipment_Items[nowEquipments[i].itemDataName];
            EquipmentData equipmentData = (EquipmentData)itemData;
            if (equipmentData.equipmentType == equipmentType)
                return i;
        }

        return -1;
    }
    public bool HasItem(string itemName, int itemNum = 1)
    {
        return HasMaterial(itemName, itemNum) || HasEquipment(itemName, itemNum);
    }
    public bool RemoveItem(string itemName,int itemNum = 1)
    {
        if (!HasItem(itemName, itemNum))
            return false;
        if (RemoveMaterial(itemName, itemNum))
            return true;
        if (RemoveEquipment(itemName, itemNum))
            return true;

        return true;
    }
    public uint GetUpgradeLevel(Upgrade upgrade)
    {
        foreach (UpgradeClass up in nowUpgrades)
            if (up.upgrade == upgrade)
                return (uint)up.level;
        return 0;
    }
    public bool HasUpgrade(Upgrade upgrade)
    {
        foreach (UpgradeClass up in nowUpgrades)
            if (up.upgrade == upgrade)
                return true;
        return false;
    }
    public bool AddUpgradeLevel(Upgrade upgrade)
    {
        foreach (UpgradeClass up in nowUpgrades)
        {
            if (up.upgrade == upgrade)
            {
                //�����ϴ� ���׷��̵� ������ �÷��ش�.
                up.level++;
                DataManager.SaveData();
                return true;
            }
        }

        //���׷��̵� ������ ������ ������ �� ���׷��̵带 �߰�.
        nowUpgrades.Add(new UpgradeClass(upgrade, 1));
        DataManager.SaveData();
        return true;
    }
    public bool PutOnItem(ItemClass newitem)
    {
        //���� �ҷ��� ������ Ÿ�� Ȯ��
        ItemManager itemManager = ItemManager.Instance;
        ItemData newitemData = itemManager.equipment_Items[newitem.itemDataName];
        EquipmentData newEquipmentData = (EquipmentData)newitemData;
        EquipmentType newItemType = newEquipmentData.equipmentType;

        //�ش� Ÿ���� ������ �׸��� ��� ������ �˻�
        int idx = GetNowEquipIdx(newItemType);

        if (idx != -1)
        {
            //���ٸ� �ش� idx�� �������� �Ҵ��ϰ�
            ItemClass tempItemClass = new ItemClass(nowEquipments[idx]);
            nowEquipments[idx] = newitem;

            //������ ����
            if (RemoveEquipment(newitem) == false)
            {
                nowEquipments[idx] = tempItemClass;
                Debug.LogError("������ ���Ű� �����ʴ´�.");
            }
            equipments.Add(tempItemClass);

            DataManager.SaveData();

            return true;
        }
        else
        {
            //���� ���������� �߰�
            nowEquipments.Add(newitem);

            //������ ����
            if(RemoveEquipment(newitem) == false)
            {
                Debug.LogError("������ ���Ű� �����ʴ´�.");
            }

            DataManager.SaveData();

            return true;
        }
    }
    public bool TakeOffItem(ItemClass newitem)
    {
        //���� �ҷ��� ������ Ÿ�� Ȯ��
        ItemManager itemManager = ItemManager.Instance;
        ItemData newitemData = itemManager.equipment_Items[newitem.itemDataName];
        EquipmentData newEquipmentData = (EquipmentData)newitemData;
        EquipmentType newItemType = newEquipmentData.equipmentType;

        //�ش� Ÿ���� ������ �׸��� ��� ������ �˻�
        int idx = GetNowEquipIdx(newItemType);

        if (idx != -1)
        {
            //������ ����
            nowEquipments.RemoveAt(idx);

            //���â�� �ٽ� �ֱ�
            equipments.Add(newitem);

            DataManager.SaveData();

            return true;
        }
        return false;
    }
}
