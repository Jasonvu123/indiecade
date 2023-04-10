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
    private GameObject altar;
    private GameObject lightCircle;

    private void Start()
    {
        altar = GameObject.FindGameObjectWithTag("Altar");
        lightCircle = altar.transform.GetChild(0).gameObject;

        hitboxRenderer = this.GetComponent<SpriteRenderer>();
        numberOfTurretCollisions = 0;

        hitboxRenderer.color = invalid;
        isBeingPlaced = true;
        isPlaceable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingPlaced)
        {
            if (isWithinAltarArea() && numberOfTurretCollisions == 0)
            {
                SetHitboxColor("valid");
                isPlaceable = true;
            }
            else
            {
                SetHitboxColor("invalid");
                isPlaceable = false;
            }
        }

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
        }
    }

    public void SetHitboxColor(string color)
    {
        if (string.Equals(color, "valid"))
        {
            hitboxRenderer.color = valid;
        }
        else if (string.Equals(color, "invalid"))
        {
            hitboxRenderer.color = invalid;
        }
        else if (string.Equals(color, "selected"))
        {
            hitboxRenderer.color = selected;
        }
    }

    private bool isWithinAltarArea()
    {
        Vector3 target = this.transform.position - altar.transform.position;
        float targetAngle = Vector3.Angle(altar.transform.position, target) * Mathf.Deg2Rad;
        Vector3 pointToCheck;

        if (this.transform.position.y >= altar.transform.position.y)
        {
            pointToCheck = this.transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxRenderer.size.x / 2, Mathf.Sin(targetAngle) * hitboxRenderer.size.y / 2 + .3f, 0);
        }
        else
        {
            pointToCheck = this.transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxRenderer.size.x / 2, Mathf.Sin(targetAngle) * -1 * hitboxRenderer.size.y / 2 - .3f, 0);
        }

        return Vector3.Distance(altar.transform.position, pointToCheck) <= lightCircle.transform.localScale.x / 2 + 1;
    }
}
