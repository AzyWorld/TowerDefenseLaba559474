using Godot;
using System;
[GlobalClass]
public partial class ControllerComponent : Node // Компонент для управления игрока
{
	[Export] Camera3D PlayerCamera; // Камера игрока
	[Export] public float Speed = 20.0f; // Скорость игрока
	[Export] public float CameraOffsetPower = 200.0f; // Сила эффекта динамики для камеры

	public override void _Ready()
	{	
		ParentNode = GetParent<CharacterBody3D>();
	}
	private CharacterBody3D ParentNode;

	private Vector3 MoveDirection; // Направление движения

	private Plane GamePlane = new Plane(new Vector3(0, -1, 0), 0); // Плоскость в которой находятся все объекты

	[Export] PackedScene BulletScene; // Сцена пули

	[Export] private PackedScene BlueprintScene; // Сцена чертежа
	private Node3D BlueprintInstance; // Объект чертежа

	[Export] private Godot.Collections.Array<PackedScene> AvaibleTowers; // Доступные для установки башни

	[Export] private Control BuildMenu; // Контейнер для кнопок постройки башни
	[Export] public float BulletDamage = 50f; // Урон пуль игрока

	[Export] private MetalManager metalManager; // Менеджер металла

	public override void _Process(double delta)
	{
		ParentNode.Velocity = new Vector3(Input.GetVector("a", "d", "w", "s").X, 0, Input.GetVector("a", "d", "w", "s").Y)*Speed;
		ParentNode.MoveAndSlide();
		PlayerCamera.HOffset = (float)((float)GetViewport().GetMousePosition().X-(float)DisplayServer.WindowGetSize().X/2.0)/CameraOffsetPower; // Динамичная камера
		PlayerCamera.VOffset = -(float)((float)GetViewport().GetMousePosition().Y-(float)DisplayServer.WindowGetSize().Y/2.0)/CameraOffsetPower;

		if (Input.IsActionJustPressed("click")) { // Стрельба
			SpawnBullet();
		}
		if (Input.IsActionJustPressed("rightClick")) // Постройка
		{
			if (BuildMenu.Visible)
			{
				BlueprintInstance.QueueFree();
				CloseBuildMenu();
			}
			else if (IsInstanceValid(BlueprintInstance))
			{
				BuildBlueprint();
			}
			else
			{
				SpawnBlueprint();
			}
		}
		if (IsInstanceValid(BlueprintInstance) && !BuildMenu.Visible)
		{
			BlueprintInstance.Position = PlanePosition(GetViewport().GetMousePosition());
		}
		if (IsInstanceValid(BlueprintInstance))
		{
			BuildMenu.Position = PlayerCamera.UnprojectPosition(BlueprintInstance.Position);
		}
		ParentNode.GetNode<Node3D>("MeshInstance3D").LookAt(PlanePosition(GetViewport().GetMousePosition()));

	}
	public Vector3 PlanePosition (Vector2 Coord) { // Проецирование позиции мыши на плоскость
		return (Vector3)GamePlane.IntersectsRay(GetViewport().GetCamera3D().ProjectRayOrigin(Coord), GetViewport().GetCamera3D().ProjectRayNormal(Coord));
	} 

	private void SpawnBullet () { // Выстрел
		Bullet BulletInstance = BulletScene.Instantiate<Bullet>();
		BulletInstance.Position = ParentNode.GlobalPosition;
		DamageComponent dc = GD.Load<PackedScene>("res://scenes/DamagingComponents/damage_component.tscn").Instantiate<DamageComponent>();
		dc.DamageAmount = BulletDamage;
		bool isAlly = ParentNode.IsInGroup("allies");
		if (isAlly) {
			dc.targetGroup = "enemies";
		}
		else {
			dc.targetGroup = "allies";
		}
		BulletInstance.AddChild(dc);

		ParentNode.AddSibling(BulletInstance);
		BulletInstance.LookAt(PlanePosition(GetViewport().GetMousePosition()));
	}

	public void SpawnBlueprint () // Спавн чертежа
	{
		BlueprintInstance = BlueprintScene.Instantiate<Node3D>();
		ParentNode.AddSibling(BlueprintInstance);
	}

	public void BuildBlueprint () // Меню установки постройки
	{
		foreach (Node item in BuildMenu.GetNode("Towers").GetChildren())
		{
			item.QueueFree();
		}
		foreach (PackedScene item in AvaibleTowers)
		{
			BuildTowerButton TowerButton = new BuildTowerButton();
			Node3D TowerInstance = item.Instantiate<Node3D>();
			TowerButton.Text = TowerInstance.Name + " (" + GetNodeMetalComponent(TowerInstance).Cost.ToString() + ")";
			TowerButton.Disabled = !metalManager.CanBuild(TowerInstance);
			TowerInstance.QueueFree();
			TowerButton.MyTower = item;
			TowerButton.SpawnPosition = BlueprintInstance.Position;
			TowerButton.Pressed += CloseBuildMenu;
			BuildMenu.GetNode("Towers").AddChild(TowerButton);
		}
		
		BuildMenu.Visible = true;
	}

	public void CloseBuildMenu () // Закрытие меню установки постройки
	{
		BuildMenu.Visible = false;
		BlueprintInstance.QueueFree();
	}

	private MetalComponent GetNodeMetalComponent (Node node) // Получение компонента металла у объекта. Если компонента металла у объекта нет, функция возвращает базовый
	{
		foreach (Node item in node.GetChildren())
		{
			if (item is MetalComponent metalComponent)
			{
				return metalComponent;
			}
			
		}
		return new MetalComponent();
	}

}
