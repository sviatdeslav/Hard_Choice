using Godot;
using System;

public partial class MusicScene : Node2D
{
	private AudioStreamPlayer2D _music; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	_music = GetNode<AudioStreamPlayer2D>("music");
		PlayMusic();
	}

	private void PlayMusic()
	{
		if (!_music.Playing)
		{
			_music.Play();
		}
	}
}
		
