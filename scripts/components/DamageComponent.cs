using Godot;
using System;

[GlobalClass]
public partial class DamageComponent : Area3D // Компонент нанесения урона
{

	[Export] public float DamageAmount = 50; // Наносимый компонентом урон
	[Export] public string targetGroup = "enemies"; // Группа на которую распространяется нанесение урона

	

	protected Node3D ParentNode;
	public override void _Ready()
	{
		ParentNode = GetParent<Node3D>();
		BodyEntered += OnBodyEntered;
	}

	public virtual void OnBodyEntered (Node node) 
	{
		
		if (node.IsInGroup(targetGroup) && node != ParentNode) 
		{
			
		foreach (Node body in GetOverlappingBodies())
		{

			if (body != ParentNode && body.IsInGroup(targetGroup)) 
			{
				
				ApplyDamage(body);
			}
		}
		ParentNode.QueueFree();
		}
	}

	public void ApplyDamage (Node damagedBody) // Функция нанесения урона
	{
		foreach (Node component in damagedBody.GetChildren())
		{
			if (component is HealthComponent healthComponent) 
			{
				healthComponent.takeDamage(DamageAmount);
			}
		}
	}
}
