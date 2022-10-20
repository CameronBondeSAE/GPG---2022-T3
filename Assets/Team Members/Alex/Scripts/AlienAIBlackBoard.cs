using System.Collections;
using System.Collections.Generic;
using Alex;
using NodeCanvas.Framework;
using UnityEngine;

namespace Alex
{
    public class AlienAiBlackBoard : MonoBehaviour
    {


        public Blackboard blackboard;

        public bool enemyFound;
        public bool atEnemy;
        public bool movingToResource;

        public bool inventoryFull;

        public bool dropOffPointFound;
        public bool atDropOffPoint;
        public bool seeResource;
        public bool atResource;
        public bool atBase;
        public bool gatherResource;
        public bool findResorce;

        // Start is called before the first frame update
        void Start()
        {
            enemyFound = false;
            atEnemy = false;
            seeResource = false;
            movingToResource = false;
            inventoryFull = false;
            dropOffPointFound = false;
            atDropOffPoint = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}