using Godot;
using System;

[GlobalClass]
public partial class BuildTowerButton : Button // Кнопка для установки постройки. Сохраняет саму постройку, которую нужно поставить и позицию
{
	public PackedScene MyTower; // Башня, которую устанавливает кнопка
	public Vector3 SpawnPosition; // Позиция Спавна

	public override void _Ready () 
	{
		Pressed += OnPressed; 
	}
	public void OnPressed ()
	{
		Node3D TowerInstance = MyTower.Instantiate<Node3D>();
		TowerInstance.Position = SpawnPosition;
		GetTree().GetFirstNodeInGroup("world").AddChild(TowerInstance);
	}
}
