using Godot;
using System;

public partial class ExplosionComponent : DamageComponent // Компонент взрыва. Спавнит огромную неподвижную пулю, которая пропадает через короткий срок
{
	public override void OnBodyEntered (Node node)  // Переопределение метода столкновения с объектом
	{
		
		if (node.IsInGroup(targetGroup) && node != ParentNode) 
		{
		foreach (Node body in GetOverlappingBodies())
		{
			if (body != ParentNode && body.IsInGroup(targetGroup)) 
			{
				Bullet BulletInstance = GD.Load<PackedScene>("res://scenes/bullet.tscn").Instantiate<Bullet>();
				BulletInstance.Position = ParentNode.GlobalPosition;
				BulletInstance.GetNode<Node3D>("MeshInstance3D").Scale = new Vector3(5f, 5f, 12.5f);
				
				DamageComponent dc = GD.Load<PackedScene>("res://scenes/DamagingComponents/damage_component.tscn").Instantiate<DamageComponent>();
				dc.Scale = new Vector3(15, 15, 15);
				
				BulletInstance.BulletSpeed = 0;
				dc.DamageAmount = DamageAmount;
				dc.targetGroup = targetGroup; 
				BulletInstance.LifeTime = 0.1;
				BulletInstance.AddChild(dc);
				ParentNode.AddSibling(BulletInstance);
				ParentNode.QueueFree();
				break;
			}
		}
		
		}
	}
}
