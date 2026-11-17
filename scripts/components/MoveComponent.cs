using Godot;
using System;

[GlobalClass]
public partial class MoveComponent : Node // Компонент движения
{
	private CharacterBody3D ParentNode;
	public override void _Ready()
	{
		ParentNode = GetParent<CharacterBody3D>();
	}

	[Export] public float MoveSpeed = 100; // Скорость объекта
	[Export] public float AttackDistance = 5; // Дистанция атаки

	[Export] private VisibilityComponent visibilityComponent; // Компонент зрения

	public Node3D Target; // Цель движения

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	public void Move(double delta) { // Функция движения
		Vector3 targetPosition = ParentNode.Position;

		if (IsInstanceValid(Target)) {
			targetPosition = new Vector3(Target.Position.X, ParentNode.Position.Y, Target.Position.Z);
		}
		else if (visibilityComponent != null) {
			Target = visibilityComponent.GetTarget();
			targetPosition = new Vector3(0, ParentNode.Position.Y, 0);
		}
		else {
			targetPosition = new Vector3(0, ParentNode.Position.Y, 0);
		}
		ParentNode.Velocity = (targetPosition-ParentNode.Position).Normalized()*MoveSpeed*(float)delta;
		if (ParentNode.Position.DistanceTo(targetPosition) > AttackDistance) {
			ParentNode.MoveAndSlide();
		}
	}

}
