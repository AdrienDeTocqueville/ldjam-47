using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{
    bool firstRun = true;

    Vector2 intialPosition;
    Quaternion intialRotation;

    // Start is called before the first frame update
    void Start()
    {
        intialPosition = transform.position;
        intialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (firstRun)
            {
                firstRun = false;
                ResetScene();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }

        }
    }

    void ResetScene()
    {
        // Reset Player state to initial
        transform.position = intialPosition;
        transform.rotation = intialRotation;

        GetComponent<PlayerAttack>().Loop();

        GetComponent<GrabBarrel>().Ungrab();

        // Reset activable platforms
        var activables = GameObject.FindObjectsOfType<Activable>();
        foreach (var activable in activables)
            activable.Loop();

        // Loop mob movement
        var loopers = GameObject.FindObjectsOfType<MotionLooper>();
        foreach (var looper in loopers)
            looper.Loop();
    }
}
