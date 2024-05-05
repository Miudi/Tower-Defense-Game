using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Spot : MonoBehaviour
{

    public Color hoverColor;
    public GameObject towerPrefab;

    private Renderer render;
    private Color oldColor;
    private GameObject currentTower;

    void Start() {
        render = GetComponent<Renderer>();
        oldColor = render.material.color;
    }

    void OnMouseEnter() {
        GetComponent<Renderer>().material.color = hoverColor;

    }

    void OnMouseExit() {
        render.material.color = oldColor;
    }

    void OnMouseDown()
    {
        if (currentTower == null)
        {
            Vector3 towerPosition = transform.position + new Vector3(0, 0.10f, 0);
            currentTower = Instantiate(towerPrefab, towerPosition, Quaternion.identity);
        }
    }
}