using Godot;
using System;

public partial class FreeGame : Node2D
{
	// Ползунки и Label
	private Label _labelЗамки;
	private HSlider _sliderЗамки;
	private Label _labelЗмеи;
	private HSlider _sliderЗмеи;
	private Label _labelПризраки;
	private HSlider _sliderПризраки;
	private Label _labelПоломка;
	private HSlider _sliderПоломка;
	private Label _labelМимик;
	private HSlider _sliderМимик;
	private Label _labelЯдоВампир;
	private HSlider _sliderЯдоВампир;
	private Label _labelВентилятор;
	private HSlider _sliderВентилятор;
	private Label _labelГаз;
	private HSlider _sliderГаз;
	private Label _labelПатроны;
	private HSlider _sliderПатроны;
	private Label _labelКлючи;
	private HSlider _sliderКлючи;
	private Label _labelТаймер;
	private HSlider _sliderТаймер;
	private Label _labelДальтонизм;
	private HSlider _sliderДальтонизм;

	public override void _Ready()
	{
		// Получаем узлы
		GetNodes();
		
		// Загружаем сохраненные настройки
		LoadSettings();
		
		// Инициализируем Label
		UpdateAllLabels();
		
		// Подключаем сигналы изменения значений
		ConnectSliders();
	}

	private void GetNodes()
	{
		_sliderЗамки = GetNode<HSlider>("Замки");
		_labelЗамки = GetNode<Label>("LabelЗамки");
		_sliderЗмеи = GetNode<HSlider>("Змеи");
		_labelЗмеи = GetNode<Label>("LabelЗмеи");
		_sliderПоломка = GetNode<HSlider>("Поломка");
		_labelПоломка = GetNode<Label>("LabelПоломка");
		_sliderПризраки = GetNode<HSlider>("Призраки");
		_labelПризраки = GetNode<Label>("LabelПризраки");
		_sliderМимик = GetNode<HSlider>("Мимик");
		_labelМимик = GetNode<Label>("LabelМимик");
		_sliderЯдоВампир = GetNode<HSlider>("ЯдоВампир");
		_labelЯдоВампир = GetNode<Label>("LabelЯдоВампир");
		_sliderВентилятор = GetNode<HSlider>("Вентилятор");
		_labelВентилятор = GetNode<Label>("LabelВентилятор");
		_sliderГаз = GetNode<HSlider>("Газ");
		_labelГаз = GetNode<Label>("LabelГаз");
		_sliderПатроны = GetNode<HSlider>("Патроны");
		_labelПатроны = GetNode<Label>("LabelПатроны");
		_sliderКлючи = GetNode<HSlider>("Ключи");
		_labelКлючи = GetNode<Label>("LabelКлючи");
		_sliderТаймер = GetNode<HSlider>("Таймер");
		_labelТаймер = GetNode<Label>("LabelТаймер");
		_sliderДальтонизм = GetNode<HSlider>("Дальтонизм");
		_labelДальтонизм = GetNode<Label>("LabelДальтонизм");
	}

	private void LoadSettings()
	{
		_sliderЗамки.Value = Settings.Colors;
		_sliderЗмеи.Value = Settings.Snakes;
		_sliderПризраки.Value = Settings.Ghosts;
		_sliderПоломка.Value = Settings.Breaks;
		_sliderМимик.Value = Settings.Mimiks;
		_sliderЯдоВампир.Value = Settings.Poison;
		_sliderВентилятор.Value = Settings.Ventilyator ? 1 : 0;
		_sliderГаз.Value = Settings.ToxicAir ? 1 : 0;
		_sliderПатроны.Value = Settings.Bullets;
		_sliderКлючи.Value = Settings.Keys;
		_sliderТаймер.Value = Settings.Timer;
		_sliderДальтонизм.Value = Settings.Daltonism ? 1 : 0;
	}

	private void UpdateAllLabels()
	{
		UpdateЗамкиLabel(_sliderЗамки.Value);
		UpdateЗмеиLabel(_sliderЗмеи.Value);
		UpdateПоломкаLabel(_sliderПоломка.Value);
		UpdateПризракиLabel(_sliderПризраки.Value);
		UpdateМимикLabel(_sliderМимик.Value);
		UpdateЯдоВампирLabel(_sliderЯдоВампир.Value);
		UpdateВентиляторLabel(_sliderВентилятор.Value);
		UpdateГазLabel(_sliderГаз.Value);
		UpdateПатроныLabel(_sliderПатроны.Value);
		UpdateКлючиLabel(_sliderКлючи.Value);
		UpdateТаймерLabel(_sliderТаймер.Value);
		UpdateДальтонизмLabel(_sliderДальтонизм.Value);
	}

	private void ConnectSliders()
	{
		_sliderЗамки.ValueChanged += (value) => {
			Settings.Colors = (int)value;
			UpdateЗамкиLabel(value);
		};
		
		_sliderЗмеи.ValueChanged += (value) => {
			Settings.Snakes = (int)value;
			UpdateЗмеиLabel(value);
		};
		
		_sliderПризраки.ValueChanged += (value) => {
			Settings.Ghosts = (int)value;
			UpdateПризракиLabel(value);
		};
		
		_sliderПоломка.ValueChanged += (value) => {
			Settings.Breaks = (int) value;
			UpdateПоломкаLabel(value);
		};
		
		_sliderМимик.ValueChanged += (value) => {
			Settings.Mimiks = (int)value;
			UpdateМимикLabel(value);
		};
		
		_sliderЯдоВампир.ValueChanged += (value) => {
			Settings.Poison = (int)value;
			UpdateЯдоВампирLabel(value);
		};
		
		_sliderВентилятор.ValueChanged += (value) => {
			Settings.Ventilyator = value == 1;
			UpdateВентиляторLabel(value);
		};
		
		_sliderГаз.ValueChanged += (value) => {
			Settings.ToxicAir = value == 1;
			UpdateГазLabel(value);
		};
		
		_sliderПатроны.ValueChanged += (value) => {
			Settings.Bullets = (int)value;
			UpdateПатроныLabel(value);
		};
		
		_sliderКлючи.ValueChanged += (value) => {
			Settings.Keys = (int)value;
			UpdateКлючиLabel(value);
		};
		
		_sliderТаймер.ValueChanged += (value) => {
			Settings.Timer = (int)value;
			UpdateТаймерLabel(value);
		};
		
		_sliderДальтонизм.ValueChanged += (value) => {
			Settings.Daltonism = value == 1;
			UpdateДальтонизмLabel(value);
		};
	}

	// Методы обновления Label (остаются без изменений)
	private void UpdateЗамкиLabel(double value) => _labelЗамки.Text = $"Количество Замков: {value}";
	private void UpdateЗмеиLabel(double value) => _labelЗмеи.Text = $"Количество Змей: {value}";
	private void UpdateПоломкаLabel(double value) => _labelПоломка.Text = value == 1 ? "Поломки: Да" : "Поломки: Нет";
	private void UpdateПризракиLabel(double value) => _labelПризраки.Text = $"Призраков: {value}";
	private void UpdateМимикLabel(double value) => _labelМимик.Text = value == 1 ? "Мимик: Да" : "Мимик: Нет";
	private void UpdateЯдоВампирLabel(double value) => _labelЯдоВампир.Text = value == 1 ? "ЯдоВампир: Да" : "ЯдоВампир: Нет";
	private void UpdateВентиляторLabel(double value) => _labelВентилятор.Text = value == 1 ? "Вентилятор: Да" : "Вентилятор: Нет";
	private void UpdateГазLabel(double value) => _labelГаз.Text = value == 1 ? "Ядовитый газ: Да" : "Ядовитый газ: Нет";
	private void UpdateДальтонизмLabel(double value) => _labelДальтонизм.Text = value == 1 ? "Дальтонизм: Да" : "Дальтонизм: Нет";
	private void UpdateПатроныLabel(double value) => _labelПатроны.Text = $"Патроны: {value}";
	private void UpdateКлючиLabel(double value) => _labelКлючи.Text = $"Ключи: {value}";
	
	private void UpdateТаймерLabel(double value)
	{
		_labelТаймер.Text = value == 0 ? "Таймер: Нет" : $"Таймер: {value} сек";
	}

	private void Starting()
	{
		// Сохраняем настройки перед запуском
		Settings.SaveSettings();
		
		Door.ResetGame(
			Settings.Bullets,
			Settings.Colors,
			Settings.Ghosts,
			Settings.Breaks,
			Settings.Snakes,
			Settings.Mimiks,
			Settings.ToxicAir,
			Settings.Keys,
			Settings.Keys,
			Settings.Keys,
			Settings.Timer == 0 ? -100 : Settings.Timer,
			Settings.Ventilyator,
			Settings.Poison,
			Settings.Daltonism
		);
		
		GetTree().ChangeSceneToFile("res://Сцены/door.tscn");
	}

	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			Starting();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
			GetTree().Quit();
		if (Input.IsActionJustPressed("ui_accept"))
			Starting();
	}

	// Кнопка сброса настроек
	private void _on_reset_button_pressed()
	{
		Settings.ResetToDefaults();
		GetTree().ReloadCurrentScene();
	}
}
