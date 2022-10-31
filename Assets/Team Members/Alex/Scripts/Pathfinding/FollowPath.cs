using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;

namespace Alex
{

    public class FollowPath : MonoBehaviour
    {
        GridGenerator grid;
        AStar astar;
        Controller controller;
        Vision vision;
        public List<Node> pathableNodesToMove;
        public List<Node> openNodes;
        public List<Node> closedNodes;
        Target target;
        public Vector3 myTarget;
        public Node[,] pathableNodeArray;
        public Node[] path;
        public int currentPathIndex;
        
        public Vector3 startPos;
        public Vector3 endPos;
        public Vector3Int myPos;
        public Vector3Int targetPos;




        // Start is called before the first frame update
        void Start()
        {
                        
            //pathableNodeArray = grid.gridNodeReferences;


            //startPos = grid.startPos;
            //Debug.Log(startPos);

            //endPos = grid.endPos;

            //astar.endLocation.worldPosition = myTarget;
            //endPos = astar.endLocation.worldPosition;
            
            
            grid = GetComponent<GridGenerator>();
            astar = GetComponent<AStar>();
            target = GetComponent<Target>();
            controller = GetComponent<Controller>();

            //myTarget = target.transform.position;
            myPos = new Vector3Int((int)controller.rb.transform.position.x, (int)controller.rb.transform.position.y, (int)controller.rb.transform.position.z);
            targetPos = new Vector3Int((int)target.transform.position.x, (int)target.transform.position.y,
                (int)target.transform.position.z);



            astar.FindPath(myPos, targetPos);
            
            Debug.Log(myPos);
            Debug.Log(targetPos);

            //Debug.Log(astar.isPathable[0]);
            //StartCoroutine(FindPathToTarget(myPos, endPos.));

        }

        public IEnumerator FindPathToTarget(Vector3Int startPosition, Vector3Int endPosition)
        {
            startPosition = myPos;
            endPosition = targetPos;
            
            
            yield return new WaitForSeconds(.01f);
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
        
        