using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Alex
{
    public class AntAIStatesSwarmer : Anthill.AI.AntAIState
    {
        protected SensorSwarmer sensorSwarmer;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            sensorSwarmer = aGameObject.GetComponent<SensorSwarmer>();
        }
    }
}
