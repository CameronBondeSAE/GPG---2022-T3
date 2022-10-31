using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Lloyd;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Alex
{

    public class GridGenerator : MonoBehaviour
    {
        public static GridGenerator singleton;
        
        

        public Node[,] gridNodeReferences;
        public Vector3Int gridSpaceSize;
        public Vector3Int totalGridSize;
        public Vector3Int startPos;
        public Vector3Int endPos;
        public List<Node> path;
        public bool isPathable;
        public float alpha = .5f;
        private AStar astar;
        public float textOffSet = .2f;
        public float yOffSet = 1f;
        public LayerMask layerMask;

        void Awake()
        {
            singleton = this;
        }
        
            
        public void Scan()
        {
            astar = GetComponent<AStar>();   
            
            gridNodeReferences = new Node[totalGridSize.x, totalGridSize.z];

            for (int x = 0; x < totalGridSize.x; x++)
            {
                for (int z = 0; z < totalGridSize.z; z++)
                {
                    gridNodeReferences[x, z] = new Node();
                    gridNodeReferences[x, z].gridPosition = new Vector2Int(x, z);
                    gridNodeReferences[x, z].worldPosition = new Vector3(x, 0, z);
                    gridNodeReferences[x, z].isPathNode = false;

                    if (Physics.OverlapBox(new Vector3(x * gridSpaceSize.x, yOffSet, z * gridSpaceSize.z),
                                new Vector3(gridSpaceSize.x, gridSpaceSize.y, gridSpaceSize.z) / 2, Quaternion.identity, layerMask).Length != 0)
                    {
                        gridNodeReferences[x, z].isBlocked = true;
                        if (gridNodeReferences[x, z].isBlocked)
                        {
                            gridNodeReferences[x, z].gCost = 0;
                            gridNodeReferences[x, z].hCost = 0;
                        }
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
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);

                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }

                    else if (node.worldPosition == startPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (node.worldPosition == endPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    
                    else if (astar.isPathable.Contains(node))
                    {
                        Gizmos.color = new Color(0, 0, 0, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (astar.closedNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (astar.openNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 1, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
 

                    else
                    { 
                        Gizmos.color = new Color(0, 1, 0, alpha); Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        
                    }
                }
                Gizmos.color = new Color(1, 1, 0, alpha);
                //Gizmos.DrawCube(new Vector3(astar.currentNode.gridPosition.y, yOffSet, astar.currentNode.gridPosition.y), Vector3.one);
                Gizmos.DrawCube(new Vector3(astar.currentNode.gridPosition.x, yOffSet,  astar.currentNode.gridPosition.y), Vector3.one);
            }
        }
    }
}
