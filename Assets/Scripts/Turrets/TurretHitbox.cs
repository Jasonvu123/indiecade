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

    public SpriteRenderer hitboxRenderer;
    private int numberOfCollisions;
    private GameObject altar;
    private GameObject lightCircle;

    private void Start()
    {
        altar = GameObject.FindGameObjectWithTag("Altar");
        lightCircle = altar.transform.GetChild(0).gameObject;

        hitboxRenderer = this.GetComponent<SpriteRenderer>();
        numberOfCollisions = 0;

        hitboxRenderer.color = invalid;
        isBeingPlaced = true;
        isPlaceable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingPlaced)
        {
            if (isWithinAltarArea() && numberOfCollisions == 0)
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
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, this.transform.position.z);
            //hitboxRenderer.enabled = true;
            hitboxRenderer.color = selected;

            if (Vector3.Distance(mousePosition, this.transform.position) <= hitboxRenderer.size.x / 2)
            {
                this.transform.parent.parent.gameObject.GetComponent<Turret>().SelectTurret();
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBeingPlaced)
        {
            if (collision.gameObject.CompareTag("TurretHitbox") || collision.gameObject.CompareTag("Level Object"))
            {
                numberOfCollisions++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBeingPlaced)
        {
            if (collision.gameObject.CompareTag("TurretHitbox") || collision.gameObject.CompareTag("Level Object"))
            {
                numberOfCollisions--;
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

        if (Vector3.Distance(altar.transform.position, pointToCheck) <= lightCircle.transform.localScale.x / 2 + .2f)
        {
            return true;
        }
        else
        {
            GameObject[] snails = GameObject.FindGameObjectsWithTag("SnailLight");

            for (int i = 0; i < snails.Length; i++)
            {
                target = this.transform.position - snails[i].transform.position;
                targetAngle = Vector3.Angle(snails[i].transform.position, target) * Mathf.Deg2Rad;

                if (this.transform.position.y >= snails[i].transform.position.y)
                {
                    pointToCheck = this.transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxRenderer.size.x / 2, Mathf.Sin(targetAngle) * hitboxRenderer.size.y / 2 + .3f, 0);
                }
                else
                {
                    pointToCheck = this.transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxRenderer.size.x / 2, Mathf.Sin(targetAngle) * -1 * hitboxRenderer.size.y / 2 - .3f, 0);
                }
                if (Vector3.Distance(snails[i].transform.position, pointToCheck) <= snails[i].transform.localScale.x / 2 + .12f)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
