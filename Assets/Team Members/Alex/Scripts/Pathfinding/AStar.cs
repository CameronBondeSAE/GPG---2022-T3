using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class AStar : MonoBehaviour
    {
        public List<Node> openNodes;
        public List<Node> closedNodes;
        public List<Node> isPathable;
        public List<Node> neighbours;
        public float alpha = .5f;
        public Node currentNode;
        public event Action PathFoundEvent;
        
        public Vector3Int startPos;
        public Vector3Int endPos;
        Coroutine co;
        public bool debugDraw = false;
        public bool debugScan = false;
        private PerlinCube_Model perlinCubeModel;
 
       
        
        void Awake()
        {
            ScanWorld();
        }

        public void Update()
        {
            //perlinCubeModel.wallDestruction += ScanWorld;
        }
        

        public void ActivateCoroutine(Vector3Int _startPos, Vector3Int _endPos)
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(FindPath(_startPos, _endPos));
        }

        public void ScanWorld()
        {
            if (debugScan == true)
            {
                GridGenerator.singleton.Scan();
            }
            openNodes = new List<Node>();
            closedNodes = new List<Node>();
        }

        public IEnumerator FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            openNodes.Clear();
            closedNodes.Clear();
            isPathable.Clear();
            neighbours.Clear();
            
            Node startNode = GridGenerator.singleton.gridNodeReferences[startPos.x, startPos.z];
            Node endNode = GridGenerator.singleton.gridNodeReferences[endPos.x, endPos.z];
            openNodes.Add(startNode);
            

            currentNode = openNodes[0];
            while (openNodes.Count > 0)
            {
                if (currentNode == endNode)
                {
                    RetracePath(startNode, endNode);
                    break;
                }
                
                if(!closedNodes.Contains(currentNode))
                    closedNodes.Add(currentNode);

                int lowestFCost = 99999;
                foreach (Node openNode in openNodes)
                {
                    if (openNode.fCost <= lowestFCost) //&& openNodes[i].hCost < currentNode.hCost)
                    {
                        lowestFCost = openNode.fCost;
                        currentNode = openNode;
                    }
                }
                openNodes.Remove(currentNode);
                
                

                
                foreach (Node neighbour in GetNeighbors(currentNode))
                {
                    if (neighbour.isBlocked || closedNodes.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;
                        
                        if(!openNodes.Contains(neighbour))
                            openNodes.Add(neighbour);
                    }
                }
                
                yield return new WaitForSeconds(.01f);
            }
        }
        
        public List<Node>GetNeighbors(Node node)
        {
            neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0)
                        continue;

                    int checkX = node.gridPosition.x + x;
                    int checkZ = node.gridPosition.y + z;

                    if (checkX >= 0 && checkX < GridGenerator.singleton.totalGridSize.x && checkZ >= 0 && checkZ < GridGenerator.singleton.totalGridSize.z)
                    {
                        neighbours.Add(GridGenerator.singleton.gridNodeReferences[checkX,checkZ]);
                        
                    }
                }
            }
            return neighbours;
        }

        public void RetracePath(Node startNode, Node endNode)
        {
            isPathable = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                isPathable.Add(currentNode);
                currentNode = currentNode.parent;
                // Debug.DrawRay(currentNode.worldPosition ,Vector3.up,Color.green);
            }
            isPathable.Reverse();

            //grid.path = isPathable;
            
            PathFoundEvent?.Invoke();
        }


        int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x );
            int distZ = Mathf.Abs(nodeA.gridPosition.y  - nodeB.gridPosition.y);

            if (distX > distZ)
                return 14 * distZ + 10 * (distX - distZ);
            return 14 * distX + 10 * (distZ - distX); 
        }

        private void OnDrawGizmos()
        {
	        if (isPathable.Count>0)
	        {
		        Gizmos.color = Color.blue;
		        for (int i = 0; i < isPathable.Count-2; i++)
		        {
			        Gizmos.DrawLine(isPathable[i].worldPosition, isPathable[i+1].worldPosition);
		        }
	        }
        }
    }
}
