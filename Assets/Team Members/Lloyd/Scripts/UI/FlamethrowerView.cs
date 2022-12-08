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

namespace UI
{

    public class FlamethrowerView : MonoBehaviour
    {
        private FlamethrowerModelView modelView;

        [SerializeField] private float overheatLevel;

        [SerializeField] private Color color;

        [SerializeField] private float lerpTime;

        private void OnEnable()
        {
            
        }

        private void Update()
        {
           // HandleColor();
        }

        private void HandleColor()
        {
            Color currentColor = color;
            if (overheatLevel <= 0)
            {
                color = Color.white;
            }

            if ((overheatLevel >= 25) || (overheatLevel <= 50))
            {
                color = Color.yellow;
            }

            if ((overheatLevel >= 51) || (overheatLevel <= 75))
            {
                color = new Color(1f, .5f, 0, 1f);
            }

            else
            {
                currentColor = Color.red;
            }
            while (currentColor != color)
            {
                //
                //how to avoid copy paste using maths?
                //
                    
                //colorVector = new Vector3.Lerp(currentColor, color, lerpTime);
                
                color = currentColor;
            }
            
        }
    }
}