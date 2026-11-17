using Godot;
using System;

[GlobalClass]
public partial class MetalComponent : Node //Компонент металла. Объект с этим компонентом стоит определенное количество металла для установки и дает сколько-то металла при смерти
{
	[Export] public int Cost = 5; // Стоимость установки
	[Export] public int DieBonus = 1; // Бонус к металлу при смерти

	public override void _Ready ()
	{
		OnBuild();
	}

	public void OnBuild () // Отнятие стоимости при постройке
	{
		if (GetTree().GetFirstNodeInGroup("MetalManager") is MetalManager metalManager)
		{
			metalManager.metalCount -= Cost;
		}
	}

	public void OnDie () // Добавление бонуса при смерти
	{
		
		if (GetTree().GetFirstNodeInGroup("MetalManager") is MetalManager metalManager)
		{
			metalManager.metalCount += DieBonus;
		}

	}
}
