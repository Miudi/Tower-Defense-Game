using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    const int PopulationSize = 1000;
    const int MapSize = 5;
    const float MutationRate = 0.01f;
    const int MaxGenerations = 1000;
    static System.Random random = new System.Random();
    static int StartPoint = random.Next(MapSize);
    static int EndPoint = random.Next(MapSize);

    void Start()
    {
        //StartCoroutine(Map(1));
    }

    public int[,] Map(int debug)
    {
        List<int[,]> population = InitializePopulation(PopulationSize);
        int generation = 0;
        var bestIndividual = population.First();

        while (generation < MaxGenerations)
        {
            population = EvolvePopulation(population);
            generation++;
            bestIndividual = population.OrderByDescending(CalculateFitness).First();
            int fitness = CalculateFitness(bestIndividual);
            if (debug == 1)
            {
                Debug.Log($"Generation {generation}: Best Fitness = {fitness}");
            }
            if (fitness >= ((MapSize * MapSize) * 4))
            {
                Debug.Log("Found optimal solution:");
                break;
            }
        }
        PrepareMap(bestIndividual); // Add this method if needed
        if (debug == 1)
        {
            PrintMap(bestIndividual); // Add this method if needed
            Debug.Log($"StartPoint: {StartPoint}:0, EndPoint: {EndPoint}:{MapSize - 1}");
        }
        return bestIndividual;
    }

    static List<int[,]> InitializePopulation(int size)
    {
        List<int[,]> population = new List<int[,]>();
        for (int i = 0; i < size; i++)
        {
            population.Add(GenerateRandomMap());
        }
        return population;
    }

    static int[,] GenerateRandomMap()
    {
        int[,] map = new int[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                map[i, j] = random.Next(2);
            }
        }
        map[StartPoint, 0] = 0; // Start point
        map[EndPoint, MapSize - 1] = 0; // End point
        return map;
    }

    static List<int[,]> EvolvePopulation(List<int[,]> population)
    {
        List<int[,]> newPopulation = new List<int[,]>();

        for (int i = 0; i < population.Count; i++)
        {
            int[,] parent1 = SelectParent(population);
            int[,] parent2 = SelectParent(population);
            int[,] child = Crossover(parent1, parent2);
            Mutate(child);
            newPopulation.Add(child);
        }

        return newPopulation;
    }

    static int[,] SelectParent(List<int[,]> population)
    {
        return population[random.Next(population.Count)];
    }

    static int[,] Crossover(int[,] parent1, int[,] parent2)
    {
        int[,] child = new int[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                child[i, j] = random.NextDouble() > 0.5 ? parent1[i, j] : parent2[i, j];
            }
        }
        return child;
    }

    static void Mutate(int[,] individual)
    {
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if ((i != StartPoint || j != 0) && (i != EndPoint || j != MapSize - 1))
                {
                    if (random.NextDouble() < MutationRate)
                    {
                        individual[i, j] = 1 - individual[i, j];
                    }
                    else
                    {
                        if (random.NextDouble() < MutationRate)
                        {
                            individual[i, j] = 1 - individual[i, j];
                        }

                    }
                }
                if (individual[i, j] > 1)
                {
                    individual[i, j] = 1;
                }
                if (individual[i, j] < 1)
                {
                    individual[i, j] = 0;
                }
            }
        }
    }

    static int CalculateFitness(int[,] map)
    {
        Node start = new Node(StartPoint, 0);
        Node end = new Node(EndPoint, MapSize - 1);
        List<Node> OPEN = new List<Node>();
        List<Node> CLOSED = new List<Node>();
        start.H_cost = ManhattanDistance(start, end);

        OPEN.Add(start);
        Node current = start;
        while (OPEN.Count > 0)
        {
            current = FindLowestF(OPEN);
            OPEN.Remove(current);
            CLOSED.Add(current);

            if (current.x == end.x && current.y == end.y)
            {
                ReconstructPath(current, map);
                return current.F_cost;
            }
            List<Node> neighbours = FindNeighbours(map, current);
            foreach (Node Neighbour in neighbours)
            {
                if (CLOSED.FirstOrDefault(n => n.x == Neighbour.x && n.y == Neighbour.y) != null)
                {
                    continue;
                }
                Neighbour.G_cost = current.G_cost + 10;
                Neighbour.H_cost = ManhattanDistance(Neighbour, end);
                Neighbour.F_cost = Neighbour.G_cost + Neighbour.H_cost;
                Neighbour.parent = current;

                Node existingNeighbour = OPEN.FirstOrDefault(n => n.x == Neighbour.x && n.y == Neighbour.y);
                if (existingNeighbour != null)
                {
                    if (existingNeighbour.F_cost > Neighbour.F_cost)
                    {
                        existingNeighbour.G_cost = Neighbour.G_cost;
                        existingNeighbour.H_cost = Neighbour.H_cost;
                        existingNeighbour.F_cost = Neighbour.F_cost;
                        existingNeighbour.parent = Neighbour.parent;
                    }
                }
                else
                {
                    OPEN.Add(Neighbour);
                }
            }
        }

        //Console.Write("\nNie znaleziono trasy!\n");
        return 0; // No path found
    }

    static List<Node> FindNeighbours(int[,] map, Node current)
    {
        List<Node> Neighbours = new List<Node>();

        if (current.x + 1 < MapSize && (map[current.y, current.x + 1] == 0 || map[current.y, current.x + 1] >= 3))
        {
            Neighbours.Add(new Node(current.x + 1, current.y));
        }
        if (current.x - 1 >= 0 && (map[current.y, current.x - 1] == 0 || map[current.y, current.x - 1] >= 3))
        {
            Neighbours.Add(new Node(current.x - 1, current.y));
        }
        if (current.y + 1 < MapSize && (map[current.y + 1, current.x] == 0 || map[current.y + 1, current.x] >= 3))
        {
            Neighbours.Add(new Node(current.x, current.y + 1));
        }
        if (current.y - 1 >= 0 && (map[current.y - 1, current.x] == 0 || map[current.y - 1, current.x] >= 3))
        {
            Neighbours.Add(new Node(current.x, current.y - 1));
        }
        return Neighbours;
    }

    static Node FindLowestF(List<Node> OPEN)
    {
        Node lowest = OPEN.First();
        foreach (Node node in OPEN)
        {
            if (lowest.F_cost > node.F_cost)
            {
                lowest = node;
            }
        }
        return lowest;
    }

    static int ManhattanDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static void ReconstructPath(Node current, int[,] map)
    {
        int number = MapSize * MapSize - 1;

        while (current.parent != null)
        {
            map[current.y, current.x] = number--;
            current = current.parent;
        }
    }

    class Node
    {
        public int x;
        public int y;
        public int G_cost;
        public int H_cost;
        public int F_cost;
        public Node parent;

        public Node(int X, int Y)
        {
            x = X;
            y = Y;
            G_cost = 0;
            H_cost = 0;
            F_cost = 0;
            parent = null;
        }
    }
    static void PrepareMap(int[,] map)
    {
        map[0, StartPoint] = 3; //Starting_Point
        //map[MapSize - 1, EndPoint] = MapSize * MapSize; //Ending_Point
    }

    public static void PrintMap(int[,] map)
    {
        int MapSize = map.GetLength(0); // Pobierz rozmiar mapy

        for (int i = 0; i < MapSize; i++)
        {
            string row = "";
            for (int j = 0; j < MapSize; j++)
            {
                row += $"{map[i, j],3} ";
            }
            Debug.Log(row);
        }
    }
}
