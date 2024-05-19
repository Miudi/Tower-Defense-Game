using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject pathPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject waypointPrefab;
    public int waypoints_offset = 1;

    public Transform Ground_Objects_parrent;
    public Transform Path_Objects_parrent;
    public Transform Waypoint_parrent;
    private int[,] map;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {

        GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm();
        map = geneticAlgorithm.Map(0);

        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        int maxNumber = 0; //Find End of the map

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] > maxNumber)
                {
                    maxNumber = map[i, j];
                }
            }
        }

        int num = 1;
        // Generate the map
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 position = new Vector3(j, 0, i);
                Vector3 waypoint_pos = new Vector3(j, waypoints_offset, i);
                if (map[i, j] == 0 || map[i, j] == 1)
                {
                    Instantiate(groundPrefab, position, Quaternion.identity, Ground_Objects_parrent);
                }
                else if (map[i, j] == 3)
                {
                    GameObject startObject = Instantiate(startPrefab, position, Quaternion.identity, Path_Objects_parrent);
                    startObject.name = "Start";
                    GameObject waypoint = Instantiate(waypointPrefab, waypoint_pos, Quaternion.identity, Waypoint_parrent);
                    waypoint.name = "Start";

                }
                else if (map[i, j] == maxNumber)
                {
                    GameObject endObject = Instantiate(endPrefab, position, Quaternion.identity, Path_Objects_parrent);
                    endObject.name = "End";
                    GameObject waypoint = Instantiate(waypointPrefab, waypoint_pos, Quaternion.identity, Waypoint_parrent);
                    waypoint.name = "End";
                }
                else if (map[i, j] > 3 && map[i, j] < maxNumber)
                {
                    GameObject pathObject = Instantiate(pathPrefab, position, Quaternion.identity, Path_Objects_parrent);
                    pathObject.name = "Path_" + map[i, j];
                    GameObject waypoint = Instantiate(waypointPrefab, waypoint_pos, Quaternion.identity, Waypoint_parrent);
                    waypoint.name = "Waypoint_" + map[i,j];
                    num++;
                }
            }
        }
    }

}