using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public List<float> notesInterval;
    public MapGenerator mapGenerator;

    private float speed = 200f;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (notesInterval.Count > 0) {
            if (elapsedTime >= (notesInterval[0] * speed)) {
                GameObject newArrow = Instantiate(arrow, transform.position, Quaternion.identity);
                newArrow.GetComponent<ArrowController>().mapGenerator = mapGenerator;
                notesInterval.RemoveAt(0);
            }
        }
    }

    public void ResetElapsedTime()
    {
        elapsedTime = 0f;
    }

    public void StartMusic(float speed, List<float> notesInterval)
    {
        ResetElapsedTime();
        this.speed = speed;
        this.notesInterval = notesInterval;
    }
}
