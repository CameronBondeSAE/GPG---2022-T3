using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Lloyd;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.MeshOperations;

public class HealthView : ImmediateModeShapeDrawer
{
    public Health healthModel;
    
    public Color colour;

    [SerializeField] private float lineLength;

    [SerializeField] private float lineThickness;
    [SerializeField] private float intensity;
    [SerializeField] private float intensityMultiplier;
    private float currentIntens;
    private float origIntens;
    private float endValue;

    [SerializeField] private float lerpWait;

    public Vector3 healthPos;
    public Vector3 endPos;

    private float HP;

    private bool pulsing;
    [SerializeField] private float lerpDuration;
    

    private void Start()
    {
        healthModel = GetComponentInParent<Health>();
        origIntens = intensity;
        colour = Color.white;
        
        youDied.enabled = false;
        hintText.enabled = false;
        respawnText.enabled = false;
        respawnButton.interactable = false;

        healthModel.ChangeHealth += ChangeHP;
        healthModel.YouDied += YouDied;
        healthModel.Spawn += Spawn;
    }

    private void Spawn()
    {
        youDied.enabled = false;
        hintText.enabled = false;
        respawnText.enabled = false;
        respawnButton.interactable = false;
    }

    public void ChangeHP(float x)
    {
        HP = x;
        lineLength = HP;
        endPos.x = healthPos.x + lineLength;

        if (HP <= 50)
        {
            pulsing = true;
            StartCoroutine(Pulsing());

            if (HP <= 10)
            {
                lerpWait = 0.5f;
                lerpDuration = 0.5f;
            }
        }
        
        if (HP <= 0)
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
            Debug.Log("pulse1");
            {
                DOTween.To(()=> intensity, x=> intensity = x, intensityMultiplier, lerpDuration);
                currentIntens = intensity;
                yield return new WaitForSeconds(lerpWait);
            }
            {
                Debug.Log("pulse2");
                DOTween.To(()=> currentIntens, x=> currentIntens = x, origIntens, lerpDuration);
                intensity = origIntens;
                yield return new WaitForSeconds(lerpWait);
            }
        }
    }
    
    //GAME OVER STUFF
    //
    
    public Image youDied;

    public TMP_Text hintText;

    public TMP_Text respawnText;

    private string hintString;

    public Button respawnButton;

    private void YouDied()
    {
        int random = Random.Range(1, 6);

        switch (random)
        {
            case 1:
                hintString = "HINT [FLAMETHROWER]: Press [LEFT CLICK] to shoot fire! Press [RIGHT CLICK] to shoot an explosive barrel! Burn stuff!";
                break;
            
            case 2:
                hintString =
                    "HINT [FLAMETHROWER]: Setting things alight is tight! But be careful! Play with fire and the consequences could be dire!";
                break;
            
            case 3:
                hintString = "HINT: Check your surroundings! Press [ADD MAP BUTTON HERE] to get a bird's eye view!";
                break;
            
            case 4:
                hintString = "HINT: HINT: BURN, BABY, BURN! AHAHAHA!";
                break;
            
            case 5:
                hintString = "HINT: Run over Plants with [WASD] to pick them up! Take them back to your Base and press [E] to deposit them!";
                break;
        }

        hintText.text = hintString;
        
        youDied.enabled = true;
        hintText.enabled = true;
        respawnText.enabled = true;
        respawnButton.interactable = true;
    }

    private void OnDisable()
    {
        healthModel.ChangeHealth -= ChangeHP;
        healthModel.YouDied -= YouDied;
        healthModel.Spawn -= Spawn;
    }
}