using System;
using Shapes;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Oscar
{
    public class Radar_View : Radar_Model
    {
        //for the drawing of the lines in the game view
        public Color colour = Color.green;
        public float lineThickness = 1f;
        public float intensity = 1f;

        //override used because it is from the inherited script
        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);

            if (RadarSwitch() == true)
            {
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
                        //if (RadarSwitch() == true)
                        {
                            Draw.Line(dir,Vector3.zero,Color.clear,Draw.Color);
                        }
                    }
                }
            }
        }
    }
}
