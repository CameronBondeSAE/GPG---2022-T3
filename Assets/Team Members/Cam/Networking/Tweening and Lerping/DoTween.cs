using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTween : MonoBehaviour
{
	public float duration = 4;

	// Start is called before the first frame update
    void Start()
    {
	    DOTween.To(Setter, 0, 4f, duration);
    }

    private void Setter(float pnewvalue)
    {
	    transform.localScale = Vector3.one * pnewvalue;
    }
}
