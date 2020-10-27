using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{
    public GameObject recordIcon = null;
    public GameObject replayIcon = null;

    enum Mode { Initial, Record, Replay };

    Mode mode = Mode.Initial;
    GameObject child = null;

    void SwitchMode()
    {
        if (child)
            Destroy(child);

        if (mode == Mode.Initial)
        {
            mode = Mode.Record;
            SetIcon(recordIcon);
            StartRecord();
        }
        else if (mode == Mode.Record)
        {
            mode = Mode.Replay;
            SetIcon(replayIcon);
            Loop();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }

    void SetIcon(GameObject icon)
    {
        var pos = icon.transform.position;
        child = Instantiate(icon);
        child.transform.parent = transform;
        child.transform.localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchMode();
        }
    }

    void StartRecord()
    {
        var mobs = GameObject.FindObjectsOfType<MobAI>();
        foreach (var mob in mobs)
            mob.enabled = true;
    }

    void Loop()
    {
        // Reset Player state to initial
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().Loop();

        // Reset activable platforms
        var activables = GameObject.FindObjectsOfType<Activable>();
        foreach (var activable in activables)
            activable.Loop();

        // Loop mob movement
        var loopers = GameObject.FindObjectsOfType<MotionLooper>();
        foreach (var looper in loopers)
            looper.Loop();
    }

    public void TriggerRecordMode()
    {
        if (mode == Mode.Initial)
            SwitchMode();
    }
}
