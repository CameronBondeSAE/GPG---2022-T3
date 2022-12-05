using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Alex
{
    public class TestShapes : ImmediateModeShapeDrawer
    {
        public PolygonPath polygonPath = new PolygonPath();
        public Color colour = Color.green;
        public float lineThickness = 1f;
        public float intensity = 1;
        public Vector3 worldPosition;
        public Transform main;
        

        private void FixedUpdate()
        {
            worldPosition = main.localPosition;
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);

            using (Draw.Command(Camera.main))
            {
                // all immediate mode drawing should happen within these using-statements

                // Set up draw state
                Draw.ResetAllDrawStates(); // ensure everything is set to their defaults
                Draw.BlendMode = ShapesBlendMode.Additive;
                Draw.Thickness = lineThickness;
                //Draw.LineGeometry = LineGeometry.Billboard;
                Draw.ThicknessSpace = ThicknessSpace.Meters;
                Draw.Color = colour * intensity;

                //for (int i = 0; i < 100; i++)
                //{
                    //Draw.Line(new Vector3(Random.Range(0, 50f), Random.Range(0, 50f), Random.Range(0, 50f)),
                        //new Vector3(Random.Range(0, 50f), Random.Range(0, 50f), Random.Range(0, 50f)));
                    
                 
                        Draw.Rotation = Quaternion.Euler(90f,0,0);
                        Draw.Position = new Vector3(0, 0.5f, 0);

                        Draw.Polygon(polygonPath);
                //}
            }
        }
    }
}