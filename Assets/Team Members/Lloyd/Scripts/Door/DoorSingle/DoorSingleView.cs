using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoorSingleView : ImmediateModeShapeDrawer
{
    public Color colour;
  
    private float numLines;
    
    private float lineLength;
    
    private float lineThickness;
    private float intensity;
    
    private float distanceBetween;

    public Vector3 doorPos;
    public Vector3 endPos;
    
    //Set Line Color 
    //Set Start / End of Line Pos

    public void SetColor(Color x)
    {
        colour = x;
    }

    public void SetPosition(Vector3 x)
    {
        doorPos = x;
        endPos = new Vector3(doorPos.x, doorPos.y + 1 * lineLength, doorPos.z);
    }
    
    //Line Stats

    public void SetLineStats(float a, float b, float c, float d, float e)
    {
        numLines = a;
        lineLength = b;
        lineThickness = c;
        intensity = d;
        distanceBetween = e;
    }
    
    //
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
            Draw.LineGeometry = LineGeometry.Billboard;
            Draw.ThicknessSpace = ThicknessSpace.Meters;
            Draw.Color = colour * intensity;

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < numLines; j++)
                {
                    Draw.Line(new Vector3(doorPos.x/2 + distanceBetween*j, doorPos.y, doorPos.z),
                        new Vector3(endPos.x + distanceBetween*j, endPos.y, endPos.z), lineThickness);
                    
                    /*
                    Draw.Line(doorPos, endPos, lineThickness);
                    Draw.Line(new Vector3(doorPos.x + distanceBetween, doorPos.y, doorPos.z),
                        new Vector3(endPos.x + distanceBetween, endPos.y, endPos.z), lineThickness);
                    Draw.Line(new Vector3(doorPos.x + distanceBetween * 2, doorPos.y, doorPos.z),
                        new Vector3(endPos.x + distanceBetween * 2, endPos.y, endPos.z), lineThickness);
                    Draw.Line(new Vector3(doorPos.x - distanceBetween, doorPos.y, doorPos.z),
                        new Vector3(endPos.x - distanceBetween, endPos.y, endPos.z), lineThickness);
                    Draw.Line(new Vector3(doorPos.x - distanceBetween * 2, doorPos.y, doorPos.z),
                        new Vector3(endPos.x + -distanceBetween * 2, endPos.y, endPos.z), lineThickness);*/
                }
            }
        }
    }
}