using Godot;
using System;
[GlobalClass]
public partial class Spawner : Node3D // Спавнер объектов
{
	[Export] public float SpawnerCooldown = 1; // Перезарядка спавнера
	private float TimeLeft = 1; // Осталось времени до спавна

	[Export] private PackedScene SceneToSpawn; // Сцена для спавна

	public override void _Ready ()
	{
		TimeLeft = SpawnerCooldown;
	}

	public override void _Process(double delta)
	{
		TimeLeft -= (float)delta;
		if (TimeLeft <= 0) 
		{
			Spawn();
		}
	}

	public void Spawn() // Спавн объекта
	{
		TimeLeft = SpawnerCooldown;
		Node3D SceneInstance = SceneToSpawn.Instantiate<Node3D>();
		SceneInstance.Position = Position;
		SpawnerCooldown = (float)Mathf.Clamp(SpawnerCooldown-0.001f, 0.5f, 100f);
		AddSibling(SceneInstance);
	}
}
