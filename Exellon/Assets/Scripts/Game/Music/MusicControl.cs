/**
<<Copyright 2017 AlduinSG (Silvio Jimenez Osma)>>

This file is part of Exellon.

 Exellon is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 Exellon is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with Exellon.  If not, see <http://www.gnu.org/licenses/>.
**/

using UnityEngine;
using System.Collections;

//This class handles different audio clips in order to achieve the effect of an infinite event dependant audio clip
public class MusicControl : MonoBehaviour {

	public AudioClip softIntro = null, softTheme = null, softToHard = null, hardTheme = null, hardToSoft = null;
	private AudioSource _transition;
	private AudioSource _loop;
	private bool _initiated;
	private bool _softToHard;
	private bool _hardToSoft;
	private bool _restoreHard;
	private bool _restoreSoft;
	private bool _start;
	private float _softThemeRate, _softToHardRate, _hardThemeRate, _hardToSoftRate;

	// Use this for initialization
	//Checks if every clip is defined and adds two audio source components to the object, in order to handle the clips transition
	void Start () {
		_initiated = softIntro != null && softTheme != null && softToHard != null && hardTheme != null && hardToSoft != null;
		_softToHard = _hardToSoft = _restoreHard = _restoreSoft = false;
		_start = _initiated;
		if(_initiated){
			_transition = gameObject.AddComponent<AudioSource>();
			_transition.playOnAwake = false;
			_loop = gameObject.AddComponent<AudioSource>();
			_loop.playOnAwake = false;
			_transition.clip = softIntro;
			_loop.clip = softTheme;
			_softThemeRate = 1f / softTheme.frequency;
			_softToHardRate = 1f / softToHard.frequency;
			_hardThemeRate = 1f / hardTheme.frequency;
			_hardToSoftRate = 1f / hardToSoft.frequency;
		}
	}
	
	// Update is called once per frame
	//Updates the audio clip to be played depending on the last menu transition
	void Update () {
		if(_initiated){
			_transition.volume = _loop.volume = GameSystem.musicOn ? 0.4f : 0;		//Checks for music active or not
			if(!_transition.isPlaying) {										//if there is no transition being played
				if(_start) {																	//if start event active
					//set the transition clip to intro theme and set a delayed play of the loop soft theme when the intro theme has finished
					_transition.clip = softIntro;
					_transition.loop = false;
					_loop.clip = softTheme;
					_loop.loop = true;
					_transition.Play();
					_loop.PlayDelayed(_transition.clip.length);
					_start = false;
				}else if(_softToHard){										//if soft to hard event active
					//interrupt loop and play transition theme from soft to hard when the looping current theme has finished
					_loop.loop = false;
					_transition.clip = softToHard;
					_transition.loop = false;
					_transition.PlayDelayed(_loop.clip.length - _loop.timeSamples * _softThemeRate);
					_softToHard = false;
					_restoreHard = true;
				}else if(_hardToSoft){										//if hard to soft event active
					//interrupt loop and play transition theme from hard to soft when the looping current theme has finished
					_loop.loop = false;
					_transition.clip = hardToSoft;
					_transition.loop = false;
					_transition.PlayDelayed(_loop.clip.length - _loop.timeSamples * _hardThemeRate);
					_softToHard = false;
					_hardToSoft = false;
					_restoreSoft = true;
				}
			}else if(!_loop.isPlaying){										//if a transition theme is being played
				if(_restoreHard){													//if there is a transition from soft to hard being played
					//Activate looping on the hard theme and start playing it after the transition theme has finished
					_loop.clip = hardTheme;
					_loop.loop = true;
					_loop.PlayDelayed(_transition.clip.length - _transition.timeSamples * _softToHardRate);
					_restoreHard = false;
				}else if(_restoreSoft){										//if there is a transition from hard to soft being played
					//Activate looping on the soft theme and start playing it after the transition theme has finished
					_loop.clip = softTheme;
					_loop.loop = true;
					_loop.PlayDelayed(_transition.clip.length - _transition.timeSamples * _hardToSoftRate);
					_restoreSoft = false;
				}
			}
		}
	}

	//Triggers a change from soft music theme to hard music theme
	public void soft2Hard() {_softToHard = true;}

	//Triggers a change from hard music theme to soft music theme
	public void hard2Soft() {_hardToSoft = true;}
}
