using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<Activable> activations = new List<Activable>();

    private void Awake()
    {
        if (activations.Count == 0)
            Debug.LogError("This pressure plate has nothing to activate !");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var a in activations)
            a.Activate();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        foreach (var a in activations)
            a.Deactivate();
    }
}
