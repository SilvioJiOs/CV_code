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

//This class handles the behaviour of the wind lines
public class Wind : MonoBehaviour {

   public float windSpeed = 90;
   public float size = 1;
   public float restartPeriod = 5;
   public float fadeOutTime = 1;
   public Texture2D tex = null;

   private Transform _child;
   private Material _mat = null;
   private float _texAsp = 0;
   private float _minPhase, _maxPhase;
   private float _windSpeed;
   private float _startTime;
   private float _startClosure;
	private float _width, _height;
   private bool _finished;
   private bool _active;
   private static bool _fadeOut = false;
   private static float _startFade;
   private static float _fadeOutTime;

   private static float DEG_90 = Mathf.PI * 0.5f;

   // Use this for initialization
	//Sets the initial animation values up
   void Awake () {
   	if(transform.childCount == 1){
   		_child = transform.GetChild(0);
   		//_child.renderer.material.SetFloat("_Alpha", 0);
         if(_child != null)   _mat = _child.renderer.material;
         if(_mat != null && tex != null)  _mat.SetTexture("_MainTex", tex);
   	}
   	if(_texAsp == 0){
   		Texture aux = _child.renderer.material.mainTexture;
   		_texAsp = aux != null ? aux.width * 1f / aux.height : 1;
   	}
   	transform.localScale = new Vector3(_texAsp * size, size, transform.localScale.z);
      _windSpeed = windSpeed * Mathf.Deg2Rad;
      _minPhase = _maxPhase = 0;
      _startTime = 0;
      _startClosure = -1;
      _finished = true;
      _active = false;
      _fadeOutTime = fadeOutTime;
   	//gameObject.SetActive(false);
   }

   // Update is called once per frame
	//Plays the animation
   void Update () {
      if(!_finished){
         float now = Time.time;
         if(_maxPhase != 1){
            _maxPhase = (Mathf.Sin(Mathf.Clamp((now - _startTime) * _windSpeed, 0, Mathf.PI) - DEG_90) + 1) * 0.5f;
            if(_maxPhase == 1)   _startClosure = now;
         }else if(_minPhase != 1){
            _minPhase = (Mathf.Sin(Mathf.Clamp((now - _startClosure) * _windSpeed, 0, Mathf.PI) - DEG_90) + 1) * 0.5f;
            _finished = _minPhase == 1;
         }
         if(_mat != null){
            _mat.SetFloat("_Min", _minPhase);
            _mat.SetFloat("_Max", _maxPhase);
            if(_fadeOut)
               _mat.SetFloat("_Alpha", Mathf.Clamp(1 - (now - _startFade)/_fadeOutTime, 0, 1));
         }
      }
   }

	//Restarts the animation
   private void restart(){
      if(_active && !_fadeOut){
         _minPhase = _maxPhase = 0;
         _startTime = Time.time;
         _startClosure = _startTime - 1;
			_finished = false;
			Vector3 local = new Vector3();
			local.z = 0;
			local.x = (_width - transform.localScale.x) * (Random.value - 0.5f);
			local.y = (_height - transform.localScale.y) * (Random.value - 0.5f);
			transform.localPosition = local;
         if(_mat != null){
            _mat.SetFloat("_Min", _minPhase);
            _mat.SetFloat("_Max", _maxPhase);
            _mat.SetFloat("_Alpha", 1);
         }
         Invoke("restart", restartPeriod);
      }
   }

	//Spawns a wind line
   public void spawn(float width, float height){
		_width = width;
		_height = height;
		_active = true;
		_fadeOut = false;
		CancelInvoke("restart");
      restart();
      gameObject.SetActive(true);
   }

	//Plays a fade out animation
   public static void fadeOut(){
      _fadeOut = true;
      _startFade = Time.time;
   }

	//Has the wind line finished fading out?
   public static bool finishedFade(){
      return (Time.time - _startFade) > _fadeOutTime;
   }
}
