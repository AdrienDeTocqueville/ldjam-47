using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class MobAI : MonoBehaviour
{
    public enum Direction { Left, Right };

    public Direction direction = Direction.Left;
    public float speed = 2.0f;

    int collideLayers;

    void Start()
    {
        collideLayers = LayerMask.NameToLayer("Terrain") | LayerMask.NameToLayer("Mob");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var angles = transform.rotation.eulerAngles;
        angles.y = (direction == Direction.Left) ? 180.0f : 0.0f;
        transform.rotation = Quaternion.Euler(angles);

        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);


        if (Input.GetKeyDown("space"))
        {
            var loopers = GameObject.FindObjectsOfType<MotionLooper>();
            foreach (var looper in loopers)
            {
                looper.Loop();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & collideLayers) != 0)
            Switcheroo();
    }


    public void Switcheroo()
    {
        direction = (1 - direction);
    }
}
