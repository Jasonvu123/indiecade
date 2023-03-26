using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShopManager : MonoBehaviour
{
    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;

    [Header("Turret Settings")]
    [SerializeField] private TurretSettings[] turrets;
    
    private void Start()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            CreateTurretCard(turrets[i]);
        }
    }

    private void CreateTurretCard(TurretSettings turretSettings)
    {
        GameObject newInstance = Instantiate(turretCardPrefab, turretPanelContainer.position, Quaternion.identity);
        newInstance.transform.SetParent(turretPanelContainer);
        newInstance.transform.localScale = Vector3.one;

        TurretCard cardButton = newInstance.GetComponent<TurretCard>();
        cardButton.SetupTurretButton(turretSettings);
    }
    
    private void TurretSelected(TurretSettings turretLoaded)
    {
        GameObject turretInstance = Instantiate(turretLoaded.TurretPrefab);
        float zPosition = turretInstance.transform.position.z;
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        turretInstance.transform.position = new Vector3(MousePosition.x, MousePosition.y, zPosition);
        
    }
    
    private void OnEnable()
    {
        TurretCard.OnPlaceTurret += TurretSelected;
    }

    private void OnDisable()
    {
        TurretCard.OnPlaceTurret -= TurretSelected;
    }
}
