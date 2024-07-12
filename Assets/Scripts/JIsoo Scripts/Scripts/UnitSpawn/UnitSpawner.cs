using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner instance { get; private set; }

    public UnitData_SO selectedUnit;
    public bool isElixirEnough = false;
    public bool spawnComplete = false;

    private Camera mainCamera;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCamera = Camera.main;

        StartCoroutine(ClickSpawnUnit());
    }
    IEnumerator ClickSpawnUnit()
    {
        while (true)
        {
            yield return null;

            if (selectedUnit != null && isElixirEnough && Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Map"))
                    {
                        Instantiate(selectedUnit.prefab, hit.point, Quaternion.identity);

                        spawnComplete = true;  
                        selectedUnit = null;    // Reset selected unit after spawning
                        isElixirEnough = false; // Reset "
                    }
                }
            }
        }
    }

    public void SelectUnit(UnitData_SO unit)
    {
        Debug.Log("Å×½ºÆ®");
        selectedUnit = unit;
    }
}
