using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node // Компонент жизни, объект может получать урон и умереть
{

	[Export] public float max_health = 100; // Максимальное количество жизней
	public float current_health; // Текущий уровень жизней

	private Node3D ParentNode;

	[Export] Label HealthLabel; // Текстовая плашка для интерфейса

	private bool Died = false; // Умер ли объект


	public override void _Ready()
	{
		ParentNode = GetParent<CharacterBody3D>();

		current_health = max_health;
	}

	public override void _Process (double delta)
	{
		if (HealthLabel != null)
		{
			HealthLabel.Text = "Здоровье: " + current_health.ToString();
		}
	}

	public void takeDamage (float damageAmount) { // Получение урона
		current_health -= damageAmount;

		if (current_health <= 0) {
			Die();
		}
	} 

	public void Die () { // Смерть
		if (!Died) {
			Died = true;
			foreach (Node item in ParentNode.GetChildren())
			{
				if (item is MetalComponent metalComponent)
				{
					metalComponent.OnDie();
				}
			}
			ParentNode.QueueFree();
			if (ParentNode is Core)
			{
			GetTree().Quit();
			}
		}
		
	}

}
