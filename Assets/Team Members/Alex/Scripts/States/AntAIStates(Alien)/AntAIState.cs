using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class AntAIState : Anthill.AI.AntAIState
    {
        protected Sensor sensor;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            sensor = aGameObject.GetComponent<Sensor>();
        }
    }
}