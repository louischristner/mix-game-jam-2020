using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public float rate = 2f;
    public GameObject arrow;

    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = rate;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= rate) {
            GameObject newArrow = Instantiate(arrow, GameObject.Find("Canvas").transform, false);
            newArrow.transform.position = transform.position;
            elapsedTime = 0f;
        }
    }
}
