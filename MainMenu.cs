using Godot;
using System;
using System.IO;

public partial class MainMenu : Node2D
{
	private Label _chance;
	private Label _start;
	private Label _settings;
	private Label _exit;
	private Label _version;
	private Label _record;
	public int ver = 1; // Переменная для выбора выводимого над дверью
	
	private string ReadFile(string filePath)
	{
		using (StreamReader reader = new StreamReader(filePath))
		{
			return reader.ReadToEnd(); // Чтение всего содержимого файла			}
		}
	}
	
	public override void _Ready()
	{
		_chance = GetNode<Label>("Chance");
		_start = GetNode<Label>("Start");
		_start.Text = "Старт (Enter)";
		_settings = GetNode<Label>("Settings");
		_settings.Text = "Настройки";
		_exit = GetNode<Label>("Exit");
		_exit.Text = "Выход (Esc)";
		_record = GetNode<Label>("Record");
		_version = GetNode<Label>("Version");
		_version.Text = "v1.0.0";
		_record.Text = "Рекорд: ";
		_record.Text += ReadFile("Рекорд.txt");
	}
	
	private void _on_start_2d_mouse_entered()
	{
		// Replace with function body.
		ver = 1;
	}
	
	private void _on_settings_2d_mouse_entered()
	{
		// Replace with function body.
		ver = 2;
	}
	
	private void _on_exit_2d_mouse_entered()
	{
		// Replace with function body.
		ver = 3;
	}
	
	private void Starting(){
		Door.ResetGame(3, 3, 0, 0, 0, 0, false, 3, 3, 3, -100, false, 1, true);
		GetTree().ChangeSceneToFile("res://Сцены/free_game.tscn");
	}
	
	private void _on_start_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		// Replace with function body.
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			Starting();
	}
	
	private void _on_settings_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		// Replace with function body.
	}
	
	private void _on_exit_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		// Replace with function body.
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			GetTree().Quit();
	}
	
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
			GetTree().Quit();
		if (Input.IsActionJustPressed("ui_accept"))
			Starting();
			
		switch(ver){
			case 1:
				_chance.Text = "100 %";
				break;
			case 2:
				_chance.Text = "50 %";
				break;
			case 3:
				_chance.Text = "0 %";
				break;
		}
	}
}
