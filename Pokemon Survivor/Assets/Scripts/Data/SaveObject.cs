using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    DataController dataController;

    private void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            dataController.SaveGame();
        }
    }
}
