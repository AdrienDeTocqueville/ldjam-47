using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Activable : MonoBehaviour
{
    public float range = 2.0f;
    public float speed = 1.0f;
    public bool rollback = true;

    bool activated = false;
    Vector3 initial;
    float alpha = 0.0f;

    private void Awake()
    {
        initial = transform.position;
    }

    void Update()
    {
        float newAlpha = alpha + speed * Time.deltaTime * (activated ? 1.0f : (rollback ? -1.0f : 0.0f));

        if ((newAlpha <= 0.0f && !activated) || (newAlpha >= range && activated))
            return;

        alpha = Mathf.Clamp(newAlpha, 0.0f, range);
        transform.position = initial + alpha * transform.right;
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
    
    public void Loop()
    {
        transform.position = initial;
        alpha = 0.0f;
        activated = false;
    }
}


// Code to display a gizmo to visualize the platform range
#if UNITY_EDITOR
[CustomEditor(typeof(Activable))]
public class ActivableEditor: Editor
{
    public void OnSceneGUI()
    {
        Activable o = (Activable)target;

        Vector3 start = o.transform.position;
        Vector3 end = start + o.transform.right * o.range;
        float size = HandleUtility.GetHandleSize(start) * 0.1f;

        Handles.DrawLine(start, end);
        Handles.SphereHandleCap(0, end, Quaternion.identity, size, EventType.Repaint);
    }
}
#endif
