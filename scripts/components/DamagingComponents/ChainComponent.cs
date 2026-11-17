using Godot;
using System;

public partial class ChainComponent : DamageComponent // Цепной снаряд. После попадания по врагу спавнит новый снаряд, который летит в другого врага
{
	public int ChainLength = 5; // Длина цепочки снарядов
	[Export] Area3D DetectZone; // Зона обнаружения врагов
	private Vector3 GetTargetPosition (Node3D ExceptionNode) // Получение позиции найденной цели
	{
		Vector3 targetPos = new Vector3 (0, -1000, 0);
		
		foreach (Node3D body in DetectZone.GetOverlappingBodies())
		{
			if (ParentNode.Position.DistanceTo(body.Position) < ParentNode.Position.DistanceTo(targetPos) && body != ExceptionNode && body.IsInGroup(targetGroup))
			{
				targetPos = body.Position;
			}
		}
		return targetPos;
	}
	public override void OnBodyEntered (Node node) // Переопределение метода столкновения с объектом
	{
		
		if (node.IsInGroup(targetGroup) && node != ParentNode) 
		{
			
		foreach (Node3D body in GetOverlappingBodies())
		{

			if (body != ParentNode && body.IsInGroup(targetGroup)) 
			{
				if (ChainLength > 0){ // Спавн пули
				Bullet BulletInstance = GD.Load<PackedScene>("res://scenes/bullet.tscn").Instantiate<Bullet>();
				BulletInstance.Position = ParentNode.GlobalPosition + new Vector3(0, 3, 0); // Берется чуть выше чтобы не пересечься с тем же самым врагом
				
				ChainComponent dc = GD.Load<PackedScene>("res://scenes/DamagingComponents/chain_component.tscn").Instantiate<ChainComponent>();
				
				dc.DamageAmount = DamageAmount;
				dc.targetGroup = targetGroup; 
				dc.ChainLength = ChainLength - 1;
				BulletInstance.LifeTime = 0.5;
				Vector3 targetPos = GetTargetPosition(body);
				if (targetPos != new Vector3(0, -1000, 0))
				{
				BulletInstance.AddChild(dc);
				ParentNode.AddSibling(BulletInstance);
				BulletInstance.LookAt(targetPos);
				ChainLength = 0;
				}

				}
				ApplyDamage(body);
			}
		}
		
		ParentNode.QueueFree();
		}
	}
}
