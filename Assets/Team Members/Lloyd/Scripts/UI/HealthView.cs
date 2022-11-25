using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;
using Lloyd;

public class HealthView : ImmediateModeShapeDrawer
{
    public Color colour;

    [SerializeField] private float lineLength;

    [SerializeField] private float lineThickness;
    [SerializeField] private float intensity;
    private float origIntens;
    private float endValue;

    [SerializeField] private float lerpWait;

    public Vector3 healthPos;
    public Vector3 endPos;

    private float HP;

    private bool pulsing;
    [SerializeField] private float lerpDuration;

    public void SetColor(Color x)
    {
        colour = x;
    }

    public void ChangeHP(float x)
    {
        HP = x;
        lineLength = HP;
        endPos.x = healthPos.x + lineLength;

        if (HP <= 10)
        {
            pulsing = true;
            StartCoroutine(Pulsing());
        }
        else
            pulsing = false;
    }

//Set Line Color 
//Set Start / End of Line Pos
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

            Draw.Line(healthPos, endPos, lineThickness);
        }
    }

    IEnumerator Pulsing()
    {
        while (pulsing)
        {
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                intensity = Mathf.Lerp(intensity, intensity * 2, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return new WaitForSeconds(lerpWait);
                timeElapsed = 0;
                timeElapsed += Time.deltaTime;
                intensity = Mathf.Lerp(intensity, origIntens, timeElapsed / lerpDuration);
            }
        }
    }
}