using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

[Serializable]
public class ShopItem
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public int Object_DB_ID { get; private set; }

    [field: SerializeField]
    public List<Materilals.Materials> Materials { get; private set; }

    [field: SerializeField]
    public List<int> MaterialsAmmount { get; private set; }

    [field: SerializeField]
    public GameObject UI_Can_u_Build_it{ get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI UI_Txt_Ammount { get; private set; }
}