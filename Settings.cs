using Godot;
using System;

public partial class Settings : Node
{
	private const string CONFIG_PATH = "user://settings.cfg";
	
	// Значения по умолчанию
	public static int Colors { get; set; } = 3;
	public static int Snakes { get; set; } = 0;
	public static int Ghosts { get; set; } = 0;
	public static int Breaks { get; set; } = 0;
	public static int Mimiks { get; set; } = 0;
	public static int Poison { get; set; } = 0;
	public static bool Ventilyator { get; set; } = false;
	public static bool ToxicAir { get; set; } = false;
	public static int Bullets { get; set; } = 3;
	public static int Keys { get; set; } = 3;
	public static int Timer { get; set; } = 0;
	public static bool Daltonism { get; set; } = false;

	public override void _Ready()
	{
		LoadSettings();
	}

	public static void SaveSettings()
	{
		var config = new ConfigFile();
		
		// Сохраняем все настройки
		config.SetValue("game", "colors", Colors);
		config.SetValue("game", "snakes", Snakes);
		config.SetValue("game", "ghosts", Ghosts);
		config.SetValue("game", "breaks", Breaks);
		config.SetValue("game", "mimiks", Mimiks);
		config.SetValue("game", "poison", Poison);
		config.SetValue("game", "ventilyator", Ventilyator);
		config.SetValue("game", "toxic_air", ToxicAir);
		config.SetValue("game", "bullets", Bullets);
		config.SetValue("game", "keys", Keys);
		config.SetValue("game", "timer", Timer);
		config.SetValue("game", "daltonism", Daltonism);
		
		config.Save(CONFIG_PATH);
	}
	
	public static void ResetToDefaults()
	{
		Colors = 3;
		Snakes = 0;
		Ghosts = 0;
		Breaks = 0;
		Mimiks = 0;
		Poison = 0;
		Ventilyator = false;
		ToxicAir = false;
		Bullets = 3;
		Keys = 3;
		Timer = 0;
		Daltonism = false;
	
		// Сохраняем настройки по умолчанию
		SaveSettings();
	}
	
	public static void LoadSettings()
	{
		var config = new ConfigFile();
		Error err = config.Load(CONFIG_PATH);
		
		if (err == Error.Ok)
		{
			Colors = (int)config.GetValue("game", "colors", 3);
			Snakes = (int)config.GetValue("game", "snakes", 0);
			Ghosts = (int)config.GetValue("game", "ghosts", 0);
			Breaks = (int)config.GetValue("game", "breaks", 0);
			Mimiks = (int)config.GetValue("game", "mimiks", 0);
			Poison = (int)config.GetValue("game", "poison", 0);
			Ventilyator = (bool)config.GetValue("game", "ventilyator", false);
			ToxicAir = (bool)config.GetValue("game", "toxic_air", false);
			Bullets = (int)config.GetValue("game", "bullets", 3);
			Keys = (int)config.GetValue("game", "keys", 3);
			Timer = (int)config.GetValue("game", "timer", 0);
			Daltonism = (bool)config.GetValue("game", "daltonism", false);
		}
	}
}
