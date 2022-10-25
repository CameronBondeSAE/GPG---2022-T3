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
        public Vector3 startPos;
        public Vector3 endPos;
        public int xPosInGrid;
        public int zPosInGrid;
        public Vector3 currentPos;
        public List<Node> path;
        public bool isPathable;

        void Start()
        {
            currentPos = startPos;
            xPosInGrid = (int)currentPos.x;
            zPosInGrid = (int)currentPos.z;
        }

        public void Update()
        {
            gridNodeReferences = new Node[totalGridSize.x, totalGridSize.z];

            for (int x = 0; x < totalGridSize.x; x++)
            {
                for (int z = 0; z < totalGridSize.z; z++)
                {
                    gridNodeReferences[x,z] = new Node(x,z);
                    if (Physics.OverlapBox(new Vector3(x * gridSpaceSize.x, 0, z * gridSpaceSize.z),
                            new Vector3(gridSpaceSize.x, gridSpaceSize.y, gridSpaceSize.z)/2, Quaternion.identity).Length > 0)
                    {
                        gridNodeReferences[x, z].isBlocked = true;
                    }
                }
            }
        }


        private void OnDrawGizmos()
        {
            if (gridNodeReferences != null && gridNodeReferences.Length > 0)
            {
                for (int x = 0; x < totalGridSize.x; x++)
                {
                    for (int z = 0; z < totalGridSize.z; z++)
                    {
                        foreach (Node node in gridNodeReferences)
                        {
                            
                            if (gridNodeReferences[x, z] != null && gridNodeReferences[x, z].isBlocked)
                            {
                                Gizmos.color = Color.red;
                            }

                            else if (startPos.x == x && startPos.z == z)
                            {
                                Gizmos.color = Color.blue;
                            }

                            else if (endPos.x == x && endPos.z == z)
                            {
                                Gizmos.color = Color.blue;
                            }

                            else if (path != null)
                            {
                                if (path.Contains(node))
                                {
                                    Gizmos.color = Color.black;
                                }
                            }
                          
                            else
                            {
                                Gizmos.color = Color.green;
                            }

                            Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
                        }
                    }
                }
            }
        }

        public List<Node>GetNeighbors(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0)
                        continue;

                    int checkX = xPosInGrid + x;
                    int checkZ = zPosInGrid + z;

                    if (checkX >= 0 && checkX < totalGridSize.x && checkZ >= 0 && checkZ < totalGridSize.y)
                    {
                        neighbours.Add(gridNodeReferences[checkX,checkZ]);
                    }
                }
            }
            return neighbours;
        }
    }
}
