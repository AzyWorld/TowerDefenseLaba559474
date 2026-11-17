using Godot;
using System;

[GlobalClass]
public partial class ShootComponent : Node // Компонент стрельбы
{
	private CharacterBody3D ParentNode;
	public override void _Ready()
	{
		ParentNode = GetParent<CharacterBody3D>();
	}

	[Export] private VisibilityComponent visibilityComponent; // Компонент зрения

	[Export] private PackedScene BulletScene; // Сцена пули которую спавнит компонент

	[Export] public float ShootingCooldown = 1.0f; // Скорость перезарядки

	[Export] private Godot.Collections.Array<PackedScene> BulletComponents; // Компоненты, навешиваемые на пули

	private Node3D Target; // Цель стрельбы

	private float CooldownProgress = 0f; // Оставшееся время до выстрела

	[Export] public float BulletDamage = 50f; // Урон пули

	public override void _Process(double delta)
	{
		if (IsInstanceValid(Target)) {
			if (CooldownProgress <= 0.0) {
				CooldownProgress = ShootingCooldown;
				SpawnBullet();
			}
		}
		else if (visibilityComponent != null) {
			Target = visibilityComponent.GetTarget();
		}
		CooldownProgress -= (float)delta;
	}

	private void SpawnBullet () { // Спавн пули, выстрел
		Bullet BulletInstance = BulletScene.Instantiate<Bullet>();
		BulletInstance.Position = ParentNode.GlobalPosition;
		foreach (PackedScene item in BulletComponents)
		{
			DamageComponent dc = item.Instantiate<DamageComponent>();
			dc.DamageAmount = BulletDamage;
			bool isAlly = ParentNode.IsInGroup("allies");
			if (isAlly) {
				dc.targetGroup = "enemies";
			}
			else {
				dc.targetGroup = "allies";
			}
			BulletInstance.AddChild(dc);
		}
		

		ParentNode.AddSibling(BulletInstance);
		if (IsInstanceValid(Target)) {
			BulletInstance.LookAt(Target.Position);
		}
	}
}
