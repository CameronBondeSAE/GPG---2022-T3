using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;

namespace Alex
{

    public class FollowPath : MonoBehaviour
    {
        public GridGenerator grid;
        public AStar astar;
        public Controller controller;
        public Vision vision;
        public TurnTowards turntowards;

        public Target target;
        public Vector3 myTarget;

        public int currentPathIndex;

        public Vector3Int myPos;
        public Vector3Int targetPos;





        // Start is called before the first frame update
        void Start()
        {
            astar.pathFoundEvent += PathFound;
            
            
            myPos = new Vector3Int((int)controller.rb.transform.position.x, (int)controller.rb.transform.position.y,
                (int)controller.rb.transform.position.z);
            targetPos = new Vector3Int((int)target.transform.position.x, (int)target.transform.position.y,
                (int)target.transform.position.z);


            astar.FindPathStartCoroutine(myPos,targetPos);
            
            




            //pathableNodesToMove = astar.isPathable;

            //path[0] = astar.isPathable[0];

            
            Debug.Log(myPos);
            Debug.Log(targetPos);
            
            
            //StartCoroutine(FindPathToTarget(myPos, targetPos));

        }

        void PathFound()
        {
            Debug.Log(astar.isPathable[0]);
        }
    }
}



    /*
Start()
Set start pos of AStar
WorldPosition to GridPosition (just use world pos AS grid pos for now)
Set end pos of AStar
eg His base, a field of resources
For now, just a pick a random spot to test
AStar.FindPath()
Sets the "isPathable" list of nodes
Store where he's up to in the path list
eg public int currentPathIndex;
// Inital target
TurnTowards.target = astar.isPathable[currentPathIndex].worldPosition;

Update()
Check distance to astar.isPathable[currentPathIndex].worldPosition
if distance < 0.5f then I'm close enough
currentPathIndex++;
TurnTowards.target = astar.isPathable[currentPathIndex].worldPosition;
Check if he’s a the end
if (currentNodeIndex > finalPathArray.Length-1)
    */
        
        