using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFieldName : MonoBehaviour
{
    [SerializeField] private string fieldName;

    private void Start()
    {
        FieldManager.nowField = fieldName;
    }
}
