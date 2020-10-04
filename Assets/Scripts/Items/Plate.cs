using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<Activable> activations = new List<Activable>();
    int counter = 0;

    private void Awake()
    {
        if (activations.Count == 0)
            Debug.LogError("This pressure plate has nothing to activate !");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (counter == 0)
            foreach (var a in activations)
                a.Activate();
        counter++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        counter--;
        if (counter == 0)
            foreach (var a in activations)
                a.Deactivate();
    }
}
