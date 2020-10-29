using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour
{
    public float width = 0.1f;

    float round(float x)
    {
        return x >= 0.0f ? (int)x + 0.5f : (int)x - 0.5f;
    }

    void Start()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = width;

        int i = 0;
        var positions = new Vector3[transform.childCount];
        foreach (Transform child in transform)
        {
            var pos = child.position;
            positions[i++] = new Vector3(round(pos.x), round(pos.y), 0.0f);
        }

        lineRenderer.positionCount = transform.childCount;
        lineRenderer.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
