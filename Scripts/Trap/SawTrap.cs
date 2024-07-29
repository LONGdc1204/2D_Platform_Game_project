using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3[] wayPointPosition;
    [SerializeField] Transform[] wayPoint;
    private int wayIndex = 1;

    private void Start()
    {
        UpdateWayPointPosition();
        transform.position = wayPointPosition[0];
    }

    private void UpdateWayPointPosition()
    {
        wayPointPosition = new Vector3[wayPoint.Length];
        for (int i = 0; i < wayPoint.Length; i++)
        {
            wayPointPosition[i] = wayPoint[i].position;
        }
    }

    private void Update() {
        // Tao move cho saw trap
        transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayIndex], speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPointPosition[wayIndex]) < .1f) {
            wayIndex++;
            if (wayIndex == wayPointPosition.Length)
                wayIndex = 0;
        }   
    }
}
