using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
public GameObject start;
    public GameObject finish;

    Dictionary<GameObject, int> floorTiles = new Dictionary<GameObject, int>();
    
    List<GameObject> path = new List<GameObject>();
    private int currentTargetIndex = 0; 
    private float moveSpeed = 10f; 
    
    private bool pathFound = false;

    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //StartFindingPath();
        player.GetComponent<TilePlayerOn>().playerChangedTile.AddListener(StartFindingPath);
    }

    
    Vector3 RoundPosition(Vector3 pos, float gridSize = 5.0f)
    {
        return new Vector3(
            Mathf.Round(pos.x / gridSize) * gridSize,
            Mathf.Round(pos.y / gridSize) * gridSize,
            Mathf.Round(pos.z / gridSize) * gridSize
        );
    }

    void StartFindingPath()
    {
        FindStart();
        FindFinish();
        path.Clear();
        currentTargetIndex = 1;
        if (start == null || finish == null)
            Debug.Log("Start and/or Finish are not defined!");


        GameObject[] floors = GameObject.FindGameObjectsWithTag("Tile");
        //floors = GameObject.FindGameObjectsWithTag("TileWall");

        int i = 0;
        foreach (GameObject floor in floors)
        {
            if (floor.GetComponent<TileCost>().tileCost == 1)
            {
                floor.GetComponent<Renderer>().material.color = Color.black; 
            }
            floorTiles[floor] = i;
            i++;
        }
        
        AStar();
    }

    // List<GameObject> GetNeighbors2(GameObject floorTile)
    // {
    //     List<GameObject> neighbors = new List<GameObject>();
    //     Vector3 pos = RoundPosition(floorTile.transform.position, 1.0f);
    //
    //     Vector3[] directions = new Vector3[]
    //     {
    //     new Vector3(2, 0, 0),
    //     new Vector3(-2, 0, 0),
    //     new Vector3(0, 0, 2),
    //     new Vector3(0, 0, -2),
    //     };
    //
    //     foreach (Vector3 dir in directions)
    //     {
    //         Vector3 neighborPos = RoundPosition(pos + dir, 1.0f);
    //         if (floorTiles.ContainsKey(neighborPos))
    //         {
    //             neighbors.Add(floorTiles[neighborPos]);
    //         }
    //     }
    //
    //     Debug.Log(neighbors.Count);
    //     return neighbors;
    // }

    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = floorTile.transform.position;

        // Possible movement directions (now including up/down variations)
        Vector3[] directions = new Vector3[]
        {
        new Vector3(10, 0, 0),  // Right
        new Vector3(-10, 0, 0), // Left
        new Vector3(0, 0, 10),  // Forward
        new Vector3(0, 0, -10), // Backward
        new Vector3(10, 10, 0),  // Up slope right
        new Vector3(-10, 10, 0), // Up slope left
        new Vector3(0, 10, 10),  // Up slope forward
        new Vector3(0, 10, -10), // Up slope backward
        new Vector3(10, -10, 0),  // Down slope right
        new Vector3(-10, -10, 0), // Down slope left
        new Vector3(0, -10, 10),  // Down slope forward
        new Vector3(0, -10, -10)  // Down slope backward
        };

        float yTolerance = 2f; // Maximum allowed height difference

        foreach (Vector3 dir in directions)
        {
            //Vector3 neighborPos = RoundPosition(pos + dir, 5.0f);
            RaycastHit hit;
            if (Physics.Raycast(pos, dir, out hit, 5))
            {
                if (floorTiles.ContainsKey(hit.collider.gameObject) && !neighbors.Contains(hit.collider.gameObject))
                {
                    GameObject neighbor = hit.collider.gameObject;
                    neighbors.Add(neighbor);
                }   
            }
        }

        return neighbors;
    }
    
    
    
    void AStar ()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> costSoFar = new Dictionary<GameObject, float>();
        PriorityQueue<GameObject> priorityQueue = new PriorityQueue<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        priorityQueue.Enqueue(start, 0);
        cameFrom[start] = null;
        costSoFar[start] = 0;

        int i = 0;

        while (priorityQueue.Count > 0)
        {
            GameObject current = priorityQueue.Dequeue();

            Debug.Log("CURRENT = " + current.name);

            if (ReferenceEquals(current, finish))
            {
                Debug.Log("Path Found!");
                ReconstructPath(cameFrom, finish);
                pathFound = true;
                return;
            }

            List<GameObject> neighbors = GetNeighbors(current);

            foreach (GameObject neighbor in neighbors)
            {
                
                float newCost = costSoFar[current] + GetCost(neighbor);
                
                if (!cameFrom.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    
                    priorityQueue.EnqueueOrUpdate(neighbor, newCost);
                }
            }
        }

        Debug.Log("No path found!");
    }

    private float GetCost(GameObject obj)
    {
        return obj.GetComponent<TileCost>().tileCost + GetDistance(obj);
    }

    private float GetDistance(GameObject obj)
    {
        float distance = (obj.transform.position - finish.transform.position).magnitude;
        return distance;
    }

    void ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        while (current != null) 
        {
            path.Add(current);
            current = cameFrom[current]; 
        }

        path.Reverse(); // Reverse to get start -> goal order
        
        Debug.Log("Path Length: " + path.Count);
        foreach (GameObject tile in path)
        {
            tile.GetComponent<Renderer>().material.color = Color.green; 
        }
    }

    void MoveAlongPath()
    {        
        if (currentTargetIndex >= path.Count) return; 

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;
        targetPos.y = this.transform.position.y; 
        
        Vector3 direction = (targetPos - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++; 
        }
    }

    void FindStart()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, gameObject.transform.lossyScale.y / 2 + 1f, tileLayer))
        {
            start = hit.collider.gameObject;
        }
    }

    void FindFinish()
    {
        finish = player.GetComponent<TilePlayerOn>().tileOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (start && finish && !GetComponent<EnemyShooting>().shooting)
        {
            MoveAlongPath();
        }
        
    }
}
