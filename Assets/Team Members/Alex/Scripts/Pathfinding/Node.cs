using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Alex
{
    [Serializable]
    public class Node
    {
        public Vector3 worldPosition;
        public bool isBlocked;
        public int gCost;
        public int hCost;

        public Node parent;
        public bool isPathNode;

        public Vector2Int gridPosition;


        public void Awake()
        {
           // isPathable = false;
        }
/*
        public Vector3 gridPosVector3
        {
            get
            {
                return new Vector3(gridPosition.x, 0, gridPosition.y);
            }
          }  
   */     

        
      
        

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}
