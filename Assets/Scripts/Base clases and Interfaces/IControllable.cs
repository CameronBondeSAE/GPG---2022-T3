using UnityEngine;

public interface IControllable
{
	public void Move(Vector2 direction);
	public void Aim(Vector2 direction);
	public void Action1();
	public void Action2();
	public void Action3();
	public void Action4Performed();
	public void Action4Cancelled();
	public void Action5Performed();
	public void Action5Cancelled();
}