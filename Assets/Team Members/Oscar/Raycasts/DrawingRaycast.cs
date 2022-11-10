using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Oscar
{
    public class DrawingRaycast : ImmediateModeShapeDrawer
    {
        public Color colour = Color.green;
        public float lineThickness = 1f;
        public float intensity = 1f;

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            
            //fill this in at home
        }
    }

}
