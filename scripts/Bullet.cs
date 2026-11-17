using Godot;
using System;

public partial class Bullet : Node3D // Пуля
{
	[Export] public float BulletSpeed = 100; // Скорость пули
	public double LifeTime = 10f; // Время спустя которое пуля удаляется
	public override void _Process(double delta)
	{
		Position += -Transform.Basis.Z*BulletSpeed*(float)delta;
		LifeTime -= delta;
		if (LifeTime <= 0f)
		{
			QueueFree();	
		}
	}
}
