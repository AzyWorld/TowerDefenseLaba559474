using Godot;
using System;

[GlobalClass]
public partial class VisibilityComponent : Area3D // Компонент зрения
{
	private Node3D ParentNode; // Родительский узел
	public override void _Ready()
	{
		ParentNode = GetParent<CharacterBody3D>();
	}

	private Node3D currentTarget; // Текущая цель
	public Node3D GetTarget () {
		foreach (var item in GetOverlappingBodies())
		{
			bool isAlly = ParentNode.IsInGroup("allies");
			bool isEnemy = ParentNode.IsInGroup("enemies");
	
			bool differentTeams = (isAlly && item.IsInGroup("enemies")) || (isEnemy && item.IsInGroup("allies"));
			if (item != ParentNode && differentTeams && (!IsInstanceValid(currentTarget) || item.Position.DistanceTo(ParentNode.Position) < currentTarget.Position.DistanceTo(ParentNode.Position))) {
				currentTarget = item;
			}
		}
		return currentTarget;
	}
}
