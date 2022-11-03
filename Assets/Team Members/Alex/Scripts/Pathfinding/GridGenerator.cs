using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Alex
{

    public class GridGenerator : MonoBehaviour
    {
        public static GridGenerator singleton;
        public Node[,] gridNodeReferences;
        public Vector3Int gridSpaceSize;
        public Vector3Int totalGridSize;
        public float alpha = .5f;
        public float textOffSet = .2f;
        public float yOffSet = 1f;
        public LayerMask layerMask;
        public bool debug = false;


        void Awake()
        {
            singleton = this;

            if (debug)
            {
	            Scan();
            }
        }

        public Vector3Int ConvertVector3ToVector3Int(Vector3 vector3)
        {
            return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }




        public void Scan()
        {
            gridNodeReferences = new Node[totalGridSize.x, totalGridSize.z];

            for (int x = 0; x < totalGridSize.x; x++)
            {
                for (int z = 0; z < totalGridSize.z; z++)
                {
                    gridNodeReferences[x, z] = new Node();
                    gridNodeReferences[x, z].gridPosition = new Vector2Int(x, z);
                    gridNodeReferences[x, z].worldPosition = new Vector3(x, 0, z);
                    gridNodeReferences[x, z].isPathNode = false;

                    if (Physics.OverlapBox(  transform.position + new Vector3(x * gridSpaceSize.x, yOffSet, z * gridSpaceSize.z),
                            new Vector3(gridSpaceSize.x, gridSpaceSize.y, gridSpaceSize.z) / 2, Quaternion.identity,
                            layerMask).Length != 0)
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
            if (debug == false)
                return;
            if (gridNodeReferences != null && gridNodeReferences.Length > 0)
            {
                foreach (Node node in gridNodeReferences)
                {

                    if (node.isBlocked)
                    {
                        Gizmos.color = new Color(1, 0, 0, alpha);
                        Gizmos.DrawCube(transform.position + new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);

                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
/*
                    else if (node.worldPosition == astar.startPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (node.worldPosition == astar.endPos)
                    {
                        Gizmos.color = new Color(0, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (astar.isPathable.Contains(node))
                    {
                        Gizmos.color = new Color(0, 0, 0, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (astar.closedNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 0, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else if (astar.openNodes.Contains(node))
                    {
                        Gizmos.color = new Color(1, 1, 1, alpha);
                        Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        //Handles.Label(new Vector3(node.worldPosition.x - .5f, yOffSet, node.worldPosition.z +.2f), "gcost " + node.gCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z), "hcost " + node.hCost.ToString(""));
                        //Handles.Label(new Vector3(node.worldPosition.x -.5f, yOffSet, node.worldPosition.z -.2f), "fcost " + node.fCost.ToString(""));
                    }
                    
                    else
                    { 
                        Gizmos.color = new Color(0, 1, 0, alpha); Gizmos.DrawCube(new Vector3(node.worldPosition.x, yOffSet, node.worldPosition.z), Vector3.one);
                        
                    }
                }
                
                Gizmos.color = new Color(1, 1, 0, alpha);
                //Gizmos.DrawCube(new Vector3(astar.currentNode.gridPosition.y, yOffSet, astar.currentNode.gridPosition.y), Vector3.one);
                Gizmos.DrawCube(new Vector3(astar.currentNode.gridPosition.x, yOffSet,  astar.currentNode.gridPosition.y), Vector3.one);
            */
                }
            }
        }
    }
}
