using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Alex
{

    public class GridGenerator : MonoBehaviour
    {

        public Node[,] gridNodeReferences;
        public Vector3Int gridSpaceSize;
        public Vector3Int totalGridSize;
        public Vector3Int startPos;
        public Vector3Int endPos;
        public List<Node> path;
        public bool isPathable;
        public float alpha = .5f;
        private AStar astar;
        
        
            
        public void Awake()
        {
            astar = GetComponent<AStar>();   
            
            gridNodeReferences = new Node[totalGridSize.x, totalGridSize.z];

            for (int x = 0; x < totalGridSize.x; x++)
            {
                for (int z = 0; z < totalGridSize.z; z++)
                {
                    gridNodeReferences[x,z] = new Node();
                    gridNodeReferences[x, z].gridPosition = new Vector2Int(x, z);
                    gridNodeReferences[x, z].worldPosition = new Vector3(x,0,z);
                    gridNodeReferences[x, z].isPathNode = false;

                    if (Physics.OverlapBox(new Vector3(x * gridSpaceSize.x, 0, z * gridSpaceSize.z),
                            new Vector3(gridSpaceSize.x, gridSpaceSize.y, gridSpaceSize.z)/2, Quaternion.identity).Length > 0)
                    {
                        gridNodeReferences[x, z].isBlocked = true;
                    }
                    else
                    {
                        gridNodeReferences[x, z].isBlocked = false;
                    }
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if (gridNodeReferences != null && gridNodeReferences.Length > 0)
            {
                foreach (Node node in gridNodeReferences)
                {
                    if (node.isBlocked)
                    {
                        Gizmos.color = new Color(1, 0, 0, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }

                    else if (node.worldPosition == startPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                    
                    else if (node.worldPosition == endPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                    
                    
                    else if (node.isPathNode)
                    {
                        Gizmos.color = new Color(0, 0, 0, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                    
                    else if (astar.closedNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                    
                    else if (astar.openNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 1, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                    
                    else
                    { 
                        Gizmos.color = new Color(0, 1, 0, alpha); Gizmos.DrawCube(new Vector3(node.worldPosition.x, 0, node.worldPosition.z), Vector3.one);
                    }
                }
            }
        }
    }
}
