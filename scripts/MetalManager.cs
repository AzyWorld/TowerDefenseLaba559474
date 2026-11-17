using Godot;
using System;
[GlobalClass]
public partial class MetalManager : Node // Менеджер металла. Определяет количество металла, отображение этого для игрока и возможность установки построек
{
	public int metalCount = 20; // Количество металла на данный момент

	

	[Export] Label MetalLabel; // Текстовая плашка для количества металла

	public override void _Ready () 
	{
		AddToGroup("MetalManager");
	}

	public override void _Process (double delta)
	{
		if (MetalLabel != null)
		{
			MetalLabel.Text = "Металл: " + metalCount.ToString();
		}
	}

	public bool CanBuild (Node node) // Возможно ли установить постройку
	{
		foreach (Node item in node.GetChildren())
		{
			if (item is MetalComponent MetalComponent)
			{
				return MetalComponent.Cost <= metalCount;
			}
		}
		return false;
	}
}
