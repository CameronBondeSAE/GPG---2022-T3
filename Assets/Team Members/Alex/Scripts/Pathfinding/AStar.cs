using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Alex
{
    public class AStar : MonoBehaviour
    {
        public GridGenerator grid;
        public List<Node> openNodes;
        public List<Node> closedNodes;
        public List<Node> isPathable;
        public List<Node> neighbours;
        public float alpha = .5f;
        public Node currentNode;
        
        void Start()
        {
            grid = GetComponent<GridGenerator>();
            openNodes = new List<Node>();
            closedNodes = new List<Node>();
            StartCoroutine(FindPath(grid.startPos, grid.endPos));
        }


        public IEnumerator FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            openNodes.Clear();
            closedNodes.Clear();
            Node startNode = grid.gridNodeReferences[startPos.x, startPos.z];
            Node endNode = grid.gridNodeReferences[endPos.x, endPos.z];

            openNodes.Add(startNode);
            

            currentNode = openNodes[0];
            while (openNodes.Count > 0)
            {
                for (int i = 1; i < openNodes.Count; i++)
                {
                    if (openNodes[i].fCost <= currentNode.fCost) //&& openNodes[i].hCost < currentNode.hCost)
                    {
                        currentNode = openNodes[i];
                    }
                }
                openNodes.Remove(currentNode);
                
                if(!closedNodes.Contains(currentNode))
                    closedNodes.Add(currentNode);

                if (currentNode == endNode)
                {
                    RetracePath(startNode, endNode);
                    yield return null;
                }

                foreach (Node neighbour in GetNeighbors(currentNode))
                {
                    if (neighbour.isBlocked || closedNodes.Contains(neighbour))
                    {
                        if(!closedNodes.Contains(neighbour))
                            closedNodes.Add(neighbour);
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

                    if (checkX >= 0 && checkX < grid.totalGridSize.x && checkZ >= 0 && checkZ < grid.totalGridSize.z)
                    {
                        neighbours.Add(grid.gridNodeReferences[checkX,checkZ]);
                        Debug.DrawRay(new Vector3(checkX,0,checkZ),Vector3.up,Color.green);
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
                currentNode.isPathNode = true;
                isPathable.Add(currentNode);
                currentNode = currentNode.parent;
            }
            isPathable.Reverse();

            grid.path = isPathable;
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
            Gizmos.color = new Color(1, 1, 0, alpha);
            Gizmos.DrawCube(new Vector3(currentNode.gridPosition.x, 0, currentNode.gridPosition.y), Vector3.one);
        }
    }
}
