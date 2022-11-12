using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Oscar
{
    public class DrawingRaycast : ImmediateModeShapeDrawer
    {
        //for the drawing of the lines in the game view
        public Color colour = Color.green;
        public float lineThickness = 1f;
        public float intensity = 1f;
        public float length = 3f;
        
        //for the actual raycast
        private float timer;
        private float radarSpeed = 100f;
        private Vector3 dir;

        private RaycastHit hitInfo;

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            
            timer += Time.deltaTime * radarSpeed;
            if (timer >= 360f)
            {
                timer = 0f;
            }

            dir = Quaternion.Euler(0, timer, 0) * transform.forward;

            Physics.Raycast(transform.position, dir, out hitInfo);
            
            //draw the lines in the game space.
            using (Draw.Command(Camera.main))
            {
                //aspects for the lines
                Draw.ResetAllDrawStates();
                Draw.BlendMode = ShapesBlendMode.Additive;
                Draw.Thickness = lineThickness;
                Draw.LineGeometry = LineGeometry.Billboard;
                Draw.ThicknessSpace = ThicknessSpace.Meters;
                Draw.Color = colour * intensity;
                

                //draw the lines
                for (int i = 0; i < 360; i++)
                {
                    Draw.Line(dir, Vector3.zero,Color.clear,Draw.Color);
                }
            }
        }
    }
}
