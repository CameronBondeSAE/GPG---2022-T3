using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class AStar : MonoBehaviour
    {
        GridGenerator grid;

        void Awake()
        {
            grid = GetComponent<GridGenerator>();
        }

        public void FindPath(Vector3 startPos, Vector3 endPos)
        {
            Node startNode = grid.gridNodeReferences[(int)grid.startPos.x, (int)grid.startPos.z];
            Node endNode = grid.gridNodeReferences[(int)grid.endPos.x, (int)grid.endPos.z];

            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes[0];
                for (int i = 1; i < openNodes.Count; i++)
                {
                    if (openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost && openNodes[i].hCost < currentNode.hCost)
                    {
                        currentNode = openNodes[i];
                    }
                }

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode == endNode)
                {
                    RetracePath(startNode, endNode);
                    return;
                }

                foreach (Node neighbours in grid.GetNeighbors(currentNode))
                {
                    if (!neighbours.isBlocked || closedNodes.Contains(neighbours))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbours);
                    if (newMovementCostToNeighbour < neighbours.gCost || !openNodes.Contains(neighbours))
                    {
                        neighbours.gCost = newMovementCostToNeighbour;
                        neighbours.hCost = GetDistance(neighbours, endNode);
                        neighbours.parent = currentNode;
                        
                        if(!openNodes.Contains(neighbours))
                            openNodes.Add(neighbours);
                    }
                }
            }
        }

        public void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            
            grid.path = path;
        }


        int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

            if (distX > distZ)
                return 14 * distZ + 10 * (distX - distZ);
            return 14 * distX + 10 * (distZ - distX); 
        }
        
    }
}
