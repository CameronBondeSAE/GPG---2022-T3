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
        public Vector3Int size;
        public Vector3Int gridSize;
        public Vector3 startPos;
        public Vector3 endPos;
        public int xPosInArray;
        public int zPosInArray;
        public Vector3 currentPos;

        public void Awake()
        {
            gridNodeReferences = new Node[gridSize.x, gridSize.z];

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    gridNodeReferences[x,z] = new Node();
                    if (Physics.OverlapBox(new Vector3(x * size.x, 0, z * size.z),
                            new Vector3(gridSize.x, gridSize.y, gridSize.z)/2f, Quaternion.identity).Length > 0)
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
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int z = 0; z < gridSize.z; z++)
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
                        
                        else
                        {
                            Gizmos.color = Color.green;
                        }
                        
                        
                        Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
                    }
                }
            }
        }
/*

        public void CheckForNeighbors()
        {
            
            grid[x, y] = new Node();
            grid[x, y].gridPosition = new Vector2Int(x, y);

            for (int x = -1; x < 2; x++)
            {
            }
            
            for (int neighbourX = -1; neighbourX < 2; neighbourX++)
            {
                grid[currentPosition.x + neighbourX, currentPosition.y + neighbourY];
 
                Open.Add(grid[x,y]);
            }
            
        }
        
        
    }
    */
    }
}
