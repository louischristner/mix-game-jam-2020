using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 50f;
    public bool canBePressed = false;

    public MapGenerator mapGenerator = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator") {
            canBePressed = true;
            other.gameObject.GetComponent<ButtonController>().SetPressable();
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator") {
            canBePressed = false;
            other.gameObject.GetComponent<ButtonController>().SetUnpressable();
        }
    }

    private void OnMouseDown()
    {
        if (canBePressed) {
            gameObject.SetActive(false);
            mapGenerator.ReduceBuildingCost();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
