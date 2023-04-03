using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHitbox : MonoBehaviour
{
    [SerializeField] public Color selected;
    [SerializeField] public Color valid;
    [SerializeField] public Color invalid;
    [SerializeField] public bool isBeingPlaced;
    [SerializeField] public bool isPlaceable;

    private SpriteRenderer hitboxRenderer;
    private int numberOfTurretCollisions;

    private void Start()
    {
        hitboxRenderer = this.GetComponent<SpriteRenderer>();
        numberOfTurretCollisions = 0;
        hitboxRenderer.color = invalid;
        isBeingPlaced = true;
        isPlaceable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if ()

    }

    private void OnMouseDown()
    {
        hitboxRenderer.color = selected;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBeingPlaced)
        {
            if (collision.gameObject.tag == "TurretHitbox")
            {
                numberOfTurretCollisions++;
                hitboxRenderer.color = invalid;
                isPlaceable = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBeingPlaced)
        {
            if (collision.gameObject.tag == "TurretHitbox")
            {
                numberOfTurretCollisions--;
            }
            if (numberOfTurretCollisions == 0)
            {
                hitboxRenderer.color = valid;
                isPlaceable = true;
            }
        }
    }
}
