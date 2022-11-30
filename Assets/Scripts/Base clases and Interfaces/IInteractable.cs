using UnityEngine;

public interface IInteractable
{
	public void Interact(GameObject interactor);
	
	/// <summary>
	/// Only works if you're holding the object
	/// </summary>
	/// <param name="interactor"></param>
	public void AltInteract(GameObject interactor);
}