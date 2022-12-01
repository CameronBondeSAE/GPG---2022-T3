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
using UnityEngine.Rendering.UI;

public class HealthView : ImmediateModeShapeDrawer
{
    public Health healthModel;

    public Color color;

    [SerializeField] private float lineLength;

    [SerializeField] private float lineThickness;
    [SerializeField] private float intensity;
    [SerializeField] private float intensityMultiplier;
    private float origIntens;
    private float endValue;

    //how long lerping between values lasts
    [SerializeField] private float lerpDuration;
    private float origDuration;

    //how long to wait before lerping again :O
    [SerializeField] private float lerpWait;
    private float origWait;

    public Vector3 healthPos;
    public Vector3 endPos;
    private Vector3 origEndPos;

    private float HP;

    private bool pulsing;

    private bool light;


    private void Start()
    {
        light = true;
        //
        healthModel = GetComponentInParent<Health>();
        origIntens = intensity;
        color = Color.white;

        origWait = lerpWait;
        origDuration = lerpDuration;

        //Game Over

        youDied.enabled = false;
        hintText.enabled = false;
        respawnText.enabled = false;
        respawnButton.interactable = false;

        healthModel.ChangeHealth += ChangeHP;
        healthModel.YouDied += YouDied;
        healthModel.Spawn += Spawn;

        CreateHintsList();
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
        intensity = origIntens;

        //bar
        color = Color.white;
        lerpDuration = origDuration;
        lerpWait = origWait;

        pulsing = false;

        if (HP <= 50)
        {
            lerpWait = origWait / 2;
            lerpDuration = origDuration / 2;
            pulsing = true;
            StartCoroutine(Pulsing());
            color = new Color(0.6f, .25f, 0.5f, 0.5f);

            if (HP <= 20)
            {
                lerpWait = origWait / 4;
                lerpDuration = origDuration / 4;
                color = new Color(1f, 0.25f, 0.25f, 1f);
            }
        }

        if (HP <= 0)
        {
            pulsing = false;
            HP = 0;
        }
    }

//Set Line Color 
//Set Start / End of Line Pos
//
    public override void DrawShapes(Camera cam)
    {
	    if (light)
        {
            DetectHealthPos();
            
            base.DrawShapes(cam);

            using (Draw.Command(cam))
            {
                // all immediate mode drawing should happen within these using-statements

                // Set up draw state
                Draw.ResetAllDrawStates(); // ensure everything is set to their defaults
                Draw.BlendMode = ShapesBlendMode.Additive;
                Draw.Thickness = lineThickness;
                Draw.LineGeometry = LineGeometry.Billboard;
                Draw.ThicknessSpace = ThicknessSpace.Meters;
                Draw.Color = color * intensity;

                
                Draw.Line(healthPos, endPos, lineThickness);
            }
        }
    }

    private void DetectHealthPos()
    {
        
    }


    IEnumerator Pulsing()
    {
        while (pulsing)
        {
            {
                DOTween.To(() => intensity, x => intensity = x, intensityMultiplier, lerpDuration);
                yield return new WaitForSeconds(lerpWait);

                DOTween.To(() => intensity, x => intensity = x, origIntens, lerpDuration);
                yield return new WaitForSeconds(lerpWait);
            }
        }
    }

    //GAME OVER STUFF
    //

    public Image youDied;

    [SerializeField] List<string> hintList = new List<string>();

    public TMP_Text hintText;

    public TMP_Text respawnText;

    private string hintString;
    private string currentString;

    public Button respawnButton;
    
    private void YouDied()
    {
        ShuffleHintList();

        youDied.enabled = true;
        hintText.enabled = true;
        respawnText.enabled = true;
        respawnButton.interactable = true;
    }

    private void CreateHintsList()
    {
        hintList.Add(
            "HINT [FLAMETHROWER]: Press [LEFT CLICK] to shoot fire! Press [RIGHT CLICK] to shoot an explosive barrel! Burn stuff!");

        hintList.Add(
            "HINT [FLAMETHROWER]: Setting things alight is tight! But be careful! Play with fire and the consequences could be dire!");

        hintList.Add("HINT: Check your surroundings! Press [ADD MAP BUTTON HERE] to get a bird's eye view!");

        hintList.Add("HINT: BURN, BABY, BURN! AHAHAHA!");

        hintList.Add(
            "HINT: Run over Plants with [WASD] to pick them up! Take them back to your Base and press [E] to deposit them!");
        
        hintList.Add("REFRESH LIST");
    }

    private void ShuffleHintList()
    {
        if (hintList.Count <= 1)
        {
            hintList.Clear();
            CreateHintsList();
            ShuffleHintList();
            return;
        }
        
        if (hintList.Count > 1)
        {
            int index = Random.Range(0, hintList.Count - 1);
            string i = hintList[index];
            currentString = i;
            hintText.text = currentString;
            hintList.RemoveAt(index);
        }
    }

    private void OnDisable()
    {
        healthModel.ChangeHealth -= ChangeHP;
        healthModel.YouDied -= YouDied;
        healthModel.Spawn -= Spawn;

        light = false;
    }
}