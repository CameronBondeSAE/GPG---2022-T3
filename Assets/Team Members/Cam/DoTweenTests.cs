using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenTests : MonoBehaviour
{
	public AnimationCurve easeCurve;
	
    // Start is called before the first frame update
    void Start()
    {
		Sequence mySequence = DOTween.Sequence();
	    
		mySequence.Append(transform.DOPunchScale(Vector3.one * 2f, 1.5f));
		mySequence.Append(transform.DOMoveY(transform.position.y - 2f, 2.5f).SetEase(easeCurve));
		mySequence.onComplete += () => Debug.Log("Finished!");
		mySequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
