using UnityEngine;

public interface IInteractable
{
	public void Interact(GameObject interactor);

    public void CancelInteract();
	
	/// <summary>
	/// Only works if you're holding the object
	/// </summary>
	/// <param name="interactor"></param>
	public void AltInteract(GameObject interactor);
    
    public void CancelAltInteract();

}