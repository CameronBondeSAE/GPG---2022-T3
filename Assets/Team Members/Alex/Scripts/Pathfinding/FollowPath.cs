using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;

namespace Alex
{

    public class FollowPath : MonoBehaviour
    {
        public AStar astar;
        public Controller controller;
        public TurnTowards turntowards;
        public float distanceToTarget;
        public int currentPathIndex;
        public Vector3 myPos;
        public Vector3 nextNodePos;
        public float distanceToTargetCheck = 2.5f; 
        public event Action PathEndReachedEvent; 
        


        // Start is called before the first frame update
        void Start()
        {
            astar.PathFoundEvent += PathFound;
        }

        public void ActivatePathToTarget(Vector3 targetPos)
        {
            myPos = controller.rb.transform.position;

            astar.ActivateCoroutine(Vector3Int.FloorToInt(myPos), Vector3Int.FloorToInt(targetPos));
        }


        void PathFound()
        {
            //Debug.Log("Pathable " + astar.isPathable[0]);
            turntowards.targetPosition = astar.isPathable[0].worldPosition;
        }

        private void FixedUpdate()
        {
            if(astar.isPathable.Count > 0)
                turntowards.targetPosition = astar.isPathable[currentPathIndex].worldPosition;
        }


        void Update()
        {
            myPos = controller.rb.transform.position;
            if (astar.isPathable.Count > 0)
            {
                nextNodePos = astar.isPathable[0].worldPosition;
                distanceToTarget = Vector3.Distance(myPos, nextNodePos);
                
                if (distanceToTarget >= distanceToTargetCheck)
                {
                    turntowards.targetPosition = astar.isPathable[0].worldPosition;
                  
                }
                else
                {
                    astar.isPathable.Remove(astar.isPathable[0]);
                }

                if (astar.isPathable.Count == 0)
                {
                    PathEndReachedEvent?.Invoke();
                    enabled = false;
                }
            }

        }
    }
}
        