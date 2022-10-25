using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    [Serializable]
    public class Node : MonoBehaviour
    {
        public Vector3 worldPosition;
        public bool isBlocked;
        public int gCost;
        public int hCost;
        public int gridX;
        public int gridZ;
        public Node parent;


        
        public Node(int gridX, int gridZ)
        {
            gridX = this.gridX;
            gridZ = this.gridZ;
        }
        
        

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}
