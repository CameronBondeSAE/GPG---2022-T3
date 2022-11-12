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
        
        public LayerMask pingLayer;

        //overide used because it is from the inherited script
        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            
            //create the loop for the radar using time.deltatime
            timer += Time.deltaTime * radarSpeed;
            if (timer >= 360f)
            {
                timer = 0f;
            }
            
            //defined direction over time
            dir = Quaternion.Euler(0, timer, 0) * transform.forward * length;
            
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
                Draw.Position = transform.position;
                Draw.Rotation = Quaternion.identity;
                

                //draw the lines
                for (int i = 0; i < 360; i++)
                {
                    Draw.Line(dir,Vector3.zero,Color.clear,Draw.Color);
                }
            }
        }

        private void Update()
        {            
            //the actual raycast that will read the collisions if there are any
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, length))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == pingLayer)
                    {
                        print("ping");
                    }
                }
            }
        }
    }
}
