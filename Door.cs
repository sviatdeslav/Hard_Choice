using Godot;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Randi // Для генерации случайных чисел
{
	public static Random rand = new Random(); // Объект класса Random
	
	public static int Gauss(double mean, double stdDev)
	{
		// использование метода Бокса-Мюллера для генерации нормального распределения
		double u1 = 1.0 - rand.NextDouble(); // uniform(0,1] рандомное значение
		double u2 = 1.0 - rand.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		return (int)(mean + stdDev * randStdNormal); // преобразование в нормальное распределение
	}
}

public class Mimik // Мимик + Poison
{
	private static int _mimik;    // Индекс мимика (-1/-2 если нет)
	private static int _poison_door; // Индекс poison-двери (-1 если нет)
	private int _mimik_door;
	private static bool _allowPoison; 
	private static Random _rand_mimik = new Random();

	public Mimik(int count, int pois)
	{
		if (count == 1)
			_mimik = -1; // Мимик есть, но дверь ещё не выбрана
		else
			_mimik = -2;  // Мимика нет
		_allowPoison = (pois == 1); 
		_poison_door = -1; 
	}

	// Устанавливает мимика и poison-дверь
	public void SetMimik(int count, int[] mas, int monstr)
	{
		// Выбор мимика (если он есть)
		while (_mimik == -1)
		{
			_mimik_door = _rand_mimik.Next(0, count);
			if ((_rand_mimik.Next(0, 101)) <= mas[_mimik_door]) 
			{
				if (_mimik_door != monstr) // Не может быть монстром
					_mimik = _mimik_door;
			}
		}

		// Выбор poison-двери (не совпадает с мимиком и монстром)
		if ((_rand_mimik.Next(0, 10) > 7) && (_allowPoison))
			while (_poison_door == -1)
			{
				int door = _rand_mimik.Next(0, count);
				if (door != monstr && door != _mimik) // Не монстр и не мимик
				{
					if (_rand_mimik.Next(0, 101) <= mas[door]) // Проверка вероятности
						_poison_door = door;
				}
			}
	}

	// Возвращает индекс мимика (-2 если нет)
	public int GetMimik()
	{
		return _mimik;
	}

	// Возвращает индекс poison-двери (-1 если нет)
	public int GetPoisonDoor()
	{
		return _poison_door;
	}

	// Проверяет, является ли дверь мимиком
	public bool IsMimik(int cur_loc)
	{
		return cur_loc == _mimik;
	}

	// Проверяет, является ли дверь poison-дверью
	public bool IsPoison(int cur_loc)
	{
		return cur_loc == _poison_door;
	}
}

public class Snakes // Змеи
{
	private static int _count_snakes;
	private static int[] _snakes;
	private Random _rand_snake = new Random();
	
	public Snakes(int count){
		_count_snakes = count;
		_snakes = new int[count];
	}
	
	public void SetSnakes(int count_doors){ // Выбор дверей для змей
		HashSet<int> numbers = new HashSet<int>();
		
		while (numbers.Count < _count_snakes){
			int newnum = _rand_snake.Next(0, count_doors);
			numbers.Add(newnum);
		}
		numbers.CopyTo(_snakes);
	}
	public bool IsSnake(int cur_loc){
		bool snake_rez = false;
		for (int i = 0; i < _count_snakes; i++){
			if (cur_loc == _snakes[i])
				snake_rez = true;
		}
		return snake_rez;
	}
}

public class Ghost // Поломка и призраки
{
	private static int _count_breaks; // Количество одержимых дверей
	private static int _count_ghosts; // Количество испорченных дверей
	private static int[] _ghosts;
	private static int[] _breaks;
	private Random _rand_ghost = new Random();
	
	public Ghost(int ghosts, int breaks){
		_count_ghosts = ghosts;
		_ghosts = new int[ghosts];
		_count_breaks = breaks;
		_breaks = new int[breaks];
	}
	
	public void SetGhost(int count_doors){ // Выбор одержимых дверей
		HashSet<int> numbers = new HashSet<int>();
		
		while (numbers.Count < _count_ghosts){
			int newnum = _rand_ghost.Next(0, count_doors);
			numbers.Add(newnum);
		}
		while (numbers.Count < _count_breaks){
			int newnum = _rand_ghost.Next(0, count_doors);
			numbers.Add(newnum);
		}
		numbers.CopyTo(_ghosts);
	}
	
	public bool IsGhost(int cur_loc){
		bool ghost_rez = false;
		for (int i = 0; i < _count_ghosts; i++){
			if (cur_loc == _ghosts[i])
				ghost_rez = true;
		}
		return ghost_rez;
	}
	
	public bool IsBreak(int cur_loc){
		bool breaks_rez = false;
		for (int i = 0; i < _count_breaks; i++){
			if (cur_loc == _breaks[i])
				breaks_rez = true;
		}
		return breaks_rez;
	}
}

public class Locks // Замки
{
	private static int _count_colors; // Количество цветов
	private static int[] _colors;
	private Random _rand_color = new Random();
	
	public Locks(int count, int doors){
		_count_colors = count;
		_colors = new int[doors];
	}
	
	public void SetLocks(int count_doors){ // Расстановка замковprivate static int[] ver = new int[_count_doors]; // Вероятность каждой двери
		int _non_lock = _rand_color.Next(0, count_doors); // Открытая дверь
		for (int i = 0; i < count_doors; i++){
			if (i != _non_lock)
				_colors[i] = _rand_color.Next(0,_count_colors);
			else
				_colors[i] = -1;
		}
	}
	
	public int IsLock(int cur_loc){
		return _colors[cur_loc];
	}
}

public partial class Door : Node2D
{
	// Объявление логических переменных
	private bool _is_open;
	private static bool _grey_now; // Выбор, будет ли следующая дверь серой
	private static bool _ventilyator = false; // Наличие вентилятора
	private static bool _toxic_air;
	private static bool _isOutOfBullets = false;
	private static bool _dalt;
	private bool _vent_here = false;
	private bool _vent_ini = false; // Инициализация вентилятора
	private bool _isShootingChoiceActive = false;
	
	// Объявление целочисленных переменных
	private int _is_it_here; // Случайная переменная для выбора двери для проверки
	private int _current_location = 0; // Дверь, на которую смотрит игрок
	private int sum_ver = 0; // Суммарная вероятность дверей
	
	// Объявление статичных целочисленных переменных
	private static int _count_doors = 5; // Количество дверей
	private static int _monstr = -1; // Положение монстра
	private static int _save_toxic;
	private static int _count_bullets; // Количество пуль
	private static int _count_keys_red = 3;
	private static int _count_keys_blue = 3;
	private static int _count_keys_purple = 3;
	private static int _counter_doors = 0;
	private static int _count_colors;
	private static int _count_real_ghosts;
	private static int _count_of_breaks;
	private static int _count_snakes;
	private static int _count_mimiks;
	private static int _poisons;
	private static int CountdownTime = 60;
	private static int[] ver = new int[_count_doors]; // Вероятность каждой двери
	
	private static float _toxicTimer;
	
	// Строковые переменные
	private string _record_doors; // Для считывания рекорда из файла
	
	// Обявление Label
	private Label _doors_open;
	private Label _record_count;
	private Label _chance;
	private Label _bullets; // Информация о количестве пуль
	private Label _toxic;
	private Label _keys_red;
	private Label _keys_blue;
	private Label _keys_purple;
	private Label _timer_label;
	private Label _shoot_label;
	
	private Timer _timer;
	
	// Объявление ProgressBar
	private static ProgressBar _toxic_bar;
	
	// Объявление изображений (Sprite2D)
	private static Sprite2D _open_door;
	private static Sprite2D _dark;
	private static Sprite2D _eye;
	private static Sprite2D _eyes;
	private Sprite2D _blue_vis;
	private Sprite2D _purple_vis;
	private Sprite2D _grey_vis;
	private Sprite2D _aspid; // Изображение змеи
	private Sprite2D _open;
	private Sprite2D _close;
	private Sprite2D _danger; // Знак опасности
	private Sprite2D _vent; // Вентилятор
	
	// Обявление переменных других классов
	private static Randi _randi_main = new Randi();
	private static Random _rand_main = new Random();
	private Mimik Mim = new Mimik(_count_mimiks, _poisons);
	private Snakes Zmey = new Snakes(_count_snakes);
	private Ghost Prizrak = new Ghost(_count_real_ghosts, _count_of_breaks);
	private Locks Zamok = new Locks(_count_colors, _count_doors);
	
	public static int GetBullets(){
		return _count_bullets;
	}
	
	public static void SetBullets(int new_bullets){
		_count_bullets = new_bullets;
	}
	
	public static int GetMonstr(){
		return _monstr;
	}
	
	private string ReadFile(string filePath)
	{
		using (StreamReader reader = new StreamReader(filePath))
		{
			return reader.ReadToEnd(); // Чтение всего содержимого файла
		}
	}
	
	private void WriteFile(string filePath, string content)
	{
		using (StreamWriter writer = new StreamWriter(filePath, false)) // false означает, что файл будет перезаписан
		{
			writer.Write(content);
		}
	}
	
	public static void ResetGame(int count_bullets, int count_colors, int count_real_ghosts, int count_of_breaks,
	int count_snakes, int count_mimiks, bool toxic_air, int count_keys_red, int count_keys_blue, int count_keys_purple,
	int Countdown, bool ventilyator, int poisons, bool dalt)
	{
		_count_bullets = count_bullets;
		_count_colors = count_colors;
		_count_real_ghosts = count_real_ghosts;
		_count_of_breaks = count_of_breaks;
		_count_snakes = count_snakes;
		_count_mimiks = count_mimiks;
		_counter_doors = 0;
		_toxic_air = toxic_air;
		_count_keys_red = count_keys_red;
		_count_keys_blue = count_keys_blue;
		_count_keys_purple = count_keys_purple;
		_save_toxic = 0;
		CountdownTime = Countdown;
		_ventilyator = ventilyator;
		_poisons = poisons;
		_dalt = dalt; // Дальтонизм
		
		Settings.SaveSettings();
	}
	
	public void LabelUse()
	{
		// Привязка Label
		_keys_red = GetNode<Label>("Keys_red");
		_keys_blue = GetNode<Label>("Keys_blue");
		_keys_purple = GetNode<Label>("Keys_purple");
		_toxic = GetNode<Label>("Toxic");
		_chance = GetNode<Label>("Chance");
		_bullets = GetNode<Label>("Bullets");
		_doors_open = GetNode<Label>("CountDoors");
		_record_count = GetNode<Label>("Record");
		_timer_label = GetNode<Label>("TimerLabel");
		_shoot_label = GetNode<Label>("Shooting");
		
		_timer = GetNode<Timer>("Timer");
		
		// Привязка шкал
		_toxic_bar = GetNode<ProgressBar>("ToxicBar");
		
		// Привязка изображений
		_aspid = GetNode<Sprite2D>("Змея");
		_open = GetNode<Sprite2D>("Галочка");
		_close = GetNode<Sprite2D>("Крестик");
		_open_door = GetNode<Sprite2D>("OpenDoor");
		_dark = GetNode<Sprite2D>("ЧёрныйФон");
		_eye = GetNode<Sprite2D>("ГлазМонстра");
		_eyes = GetNode<Sprite2D>("ГлазаМонстра");
		_blue_vis = GetNode<Sprite2D>("Синий");
		_purple_vis = GetNode<Sprite2D>("Фиолетовый");
		_grey_vis = GetNode<Sprite2D>("Серый");
		_danger = GetNode<Sprite2D>("Внимание"); // Знак опасности
		_vent = GetNode<Sprite2D>("Вентилятор"); // Вентилятор
		
		_record_doors = ReadFile("Рекорд.txt");
		
		UpdateTimerLabel();
		_timer.Timeout += OnTimerTimeout;
		_timer.Start();
	}
	
	public void WriteLabel()
	{
		// Запись значений в Label
		_bullets.Text = "Пули: ";
		_toxic.Text = "Отравление:";
		_bullets.Text += _count_bullets.ToString();
		_keys_red.Text = "Красные ключи: ";
		_keys_red.Text += _count_keys_red.ToString();
		if (_count_colors > 1){
			_keys_blue.Text = "Синие ключи: ";
			_keys_blue.Text += _count_keys_blue.ToString();
		}
		if (_count_colors > 2){
			_keys_purple.Text = "Фиолетовые ключи: ";
			_keys_purple.Text += _count_keys_purple.ToString();
		}
		_doors_open.Text = "Пройдено дверей: ";
		_doors_open.Text += _counter_doors.ToString();
		_record_count.Text = "Рекорд: ";
		_record_count.Text += _record_doors.ToString();
		if (CountdownTime == -100){
			_timer_label.Visible = false;
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		LabelUse();
		// Настройка случайных параметров
		for (int i = 0; i < _count_doors; i++){
			if (i != _count_doors - 1){
				ver[i] = Randi.Gauss((int)(100/_count_doors), 3);
				sum_ver += ver[i];
			}
			else ver[i] = 100 - sum_ver;
		}
		_monstr = -1;
		_toxic_bar.Value = _save_toxic;
		while (_monstr == -1){
			_is_it_here = _rand_main.Next(0, _count_doors);
			if (_rand_main.Next(0, 101) <= ver[_is_it_here])
				_monstr = _is_it_here;
		}
		Zamok.SetLocks(_count_doors);
		Mim.SetMimik(_count_doors, ver, GetMonstr());
		Zmey.SetSnakes(_count_doors);
		Prizrak.SetGhost(_count_doors);
		WriteLabel();
	}
	
	private void OnTimerTimeout(){
		if (CountdownTime != -100){
			CountdownTime--;
			UpdateTimerLabel();
				
			if (CountdownTime <= 0)
			{
				_timer.Stop();
				if (_record_doors.ToInt() < _counter_doors)
					WriteFile("Рекорд.txt", _counter_doors.ToString());
				GetTree().ChangeSceneToFile("res://Сцены/main_menu.tscn");
			}
		}
	}
	
	private void UpdateTimerLabel(){
		_timer_label.Text = "Оставшееся время: " + CountdownTime + " сек";
		_toxic_bar.Value = _save_toxic; // Обновляем шкалу отравления
	}
	
	private async void VisDoor() // Визуализация двери
	{
		if (Input.IsActionJustPressed("ui_cancel")) // Выход из игры
			GetTree().Quit();
		if (_vent_ini == false){
			_vent_here = ((_rand_main.Next(0, 10) < 5) && (_ventilyator));
			_vent_ini = true;
		}
		if (_vent_here)
			_danger.Visible = true;
		else
			_danger.Visible = false;
		if (Zmey.IsSnake(_current_location)) // Визуализация змей
			_aspid.Visible = true;
		else
			_aspid.Visible = false;
		if (Prizrak.IsBreak(_current_location)) // Визуализация поломки
			_chance.Text = "?? %";
		else if (Prizrak.IsGhost(_current_location)) // Визуализация призрака
			{
				_chance.Text = _rand_main.Next(0,101).ToString() + " %";
				await Task.Delay(100);
			}
		else {
			_chance.Text = ver[_current_location].ToString();
			_chance.Text += " %";
		}
		int lockType = Zamok.IsLock(_current_location); // Проверка типа замка
		_open.Visible = (lockType == -1); // Если замка нет, дверь открыта
		_close.Visible = (lockType != -1); // И наоборот
		if ((_dalt == true) && (_grey_now))
			_grey_vis.Visible = true;
		else
			switch (lockType) // Визуализация цвета двери
			{
				case -1:
				case 0:
					_blue_vis.Visible = false;
					_purple_vis.Visible = false;
					break;
				case 1:
					_blue_vis.Visible = true;
					_purple_vis.Visible = false;
				break;
				case 2:
					_blue_vis.Visible = false;
					_purple_vis.Visible = true;
				break;
			}
	}
	
	private void Survive(){
		_counter_doors += 1;
		_vent_ini = false;
		_shoot_label.Visible = false;
		GetTree().ReloadCurrentScene();
	}
	
	private void Fail(){
		GetTree().ChangeSceneToFile("res://Сцены/main_menu.tscn");
		if (_record_doors.ToInt() < _counter_doors)
			WriteFile("Рекорд.txt", _counter_doors.ToString());
	}
	
	private async Task Shoot()
	{
		if ((_current_location == _monstr) || (Mim.IsMimik(_current_location)))
			{
				// Показываем соответствующие глаза
				if (_current_location == _monstr)
				{
					_eye.Visible = _rand_main.Next(0, 10) > 2;
					_eyes.Visible = !_eye.Visible;
				}
				else
				{
					_eyes.Visible = _rand_main.Next(0, 10) > 2;
					_eye.Visible = !_eyes.Visible;
				}
				if (GetBullets() > 0){
					_isShootingChoiceActive = true;
					_shoot_label.Text = "Выстрелить? Да[Пробел/Enter]/Нет[Tab]";
					_shoot_label.Visible = true;
					bool shoot = false;
					bool choiceMade = false;
					while (!choiceMade && !IsQueuedForDeletion()){
						if (Input.IsActionJustPressed("ui_accept")) // Пробел/Enter
						{
							shoot = true;
							choiceMade = true;
						}
						else if (Input.IsActionJustPressed("ui_focus_next")) // Tab
						{
							shoot = false;
							choiceMade = true;
						}
						await ToSignal(GetTree(), "process_frame"); // Важно: используем Godot's сигнал
					}
					_shoot_label.Visible = false;
					_isShootingChoiceActive = false;
					if (choiceMade){
						if (shoot){
							SetBullets(GetBullets() - 1);
							Survive();
							return;
						}
						else{
							if (_current_location == _monstr){
								Fail();
								return;
							}
							else{
								Survive();
								return;
							}
						}
					}
				}
				else{
					_isOutOfBullets = true;
					_shoot_label.Text = "Пули кончились...";
					_shoot_label.Visible = true;
					await Task.Delay(3000);
					_isOutOfBullets = false;
					if (_current_location == _monstr)
					{
						Fail();
						return;
					}
					else
					{
						Survive();
						return;
					}
			}
		}
		if (Mim.IsPoison(_current_location)){
			_isOutOfBullets = true;
			_shoot_label.Text = "Тут кто-то есть...";
			_shoot_label.Visible = true;
			await Task.Delay(2500);
			_isOutOfBullets = false;
			if (_save_toxic < 20)
			{
				Fail();
				return;
			}
			else
			{
				_save_toxic -= 20;
				_toxic_bar.Value = _save_toxic; // Обновляем шкалу отравления
				Survive();
				return;
			}
		}
	}
	
	private async void MoveLeftRight() // Движение влево-вправо
	{
		if (Input.IsActionJustPressed("ui_right") && _current_location < _count_doors - 1) {
			_current_location += 1;
			_dark.Visible = true;
			await Task.Delay(50);
			_dark.Visible = false;
		}
			
		if (Input.IsActionJustPressed("ui_left") && _current_location > 0){
			_current_location -= 1;
			_dark.Visible = true;
			await Task.Delay(50);
			_dark.Visible = false;
		}
	}
	
	private async void OpeningDoor() // Открытие двери
	{
		_is_open = false;
		if ((Zamok.IsLock(_current_location) == 0) && (_count_keys_red > 0)){
			_count_keys_red -= 1;
			_is_open = true;
		}
		if ((Zamok.IsLock(_current_location) == 1) && (_count_keys_blue > 0)){
			_count_keys_blue -= 1;
			_is_open = true;
		}
		if ((Zamok.IsLock(_current_location) == 2) && (_count_keys_purple > 0)){
			_count_keys_purple -= 1;
			_is_open = true;
		}
		if (Zamok.IsLock(_current_location) == -1)
			_is_open = true;
	}
	
	private void Intoxication(){ // Если на двери змея
		if (Zmey.IsSnake(_current_location))
			_save_toxic += 25;
	}
	
	private async Task DeathFromVent(){ // Смерть от вентилятора
		if ((_vent_here) && (_current_location == _monstr)) {
			_vent.Visible = true;
			_shoot_label.Text = "Пули здесь бессильны";
			_shoot_label.Visible = true;
			await Task.Delay(3000);
			Fail();
		}
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		// Токсичный урон (если активен)
		if (_toxic_air)
		{
			_toxicTimer += (float)delta;
			if (_toxicTimer >= 1.0f) // Каждую секунду
			{
				_save_toxic += 1;
				 _toxicTimer = 0f;
				_toxic_bar.Value = _save_toxic; // Обновляем шкалу
			}
		}
		
		if (_isShootingChoiceActive || _isOutOfBullets){
			if (Input.IsActionJustPressed("ui_cancel"))
				GetTree().Quit();
			return; // Пропускаем обработку ввода, если идет выбор
		}
		VisDoor();
		if (_save_toxic == 100)
			Fail();
		MoveLeftRight();
		if (Input.IsActionJustPressed("ui_up")){
			OpeningDoor();
			if (_is_open){
				_grey_vis.Visible = false;
				_grey_now = (_rand_main.Next(0, 10) > 4);
				Intoxication();
				_open_door.Visible = true;
				await DeathFromVent();
				await Shoot();
				if ((_current_location != _monstr) && (!Mim.IsMimik(_current_location))) // Если за дверью нет монстра
					Survive();
			}
		}
		GD.Print(_monstr, " ", _count_bullets, " ", Mim.GetMimik(), " ", _record_doors);
	}
}
