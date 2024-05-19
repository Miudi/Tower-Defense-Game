using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake()
    {
        StartCoroutine(WaitForStartObject());
    }

    IEnumerator WaitForStartObject()
    {
        yield return new WaitUntil(() => GameObject.Find("Start") != null);

        List<Transform> pointsList = new List<Transform>();
        pointsList.Add(GameObject.Find("Start").transform);


        var waypoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name.StartsWith("Waypoint_"))
            {
                waypoints.Add(child);
            }
        }
        waypoints = waypoints.OrderBy(w => int.Parse(w.name.Split('_')[1])).ToList();
        pointsList.AddRange(waypoints);

        pointsList.Add(GameObject.Find("End").transform);

        points = pointsList.ToArray();
    }
}