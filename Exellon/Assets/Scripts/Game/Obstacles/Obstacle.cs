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

//This class handles the behaviour of an obstacle object
public class Obstacle : PauseObject, Hitable {

   public float width = 1;
	public AudioSource hitSound = null;
   
   private static bool _initiated;
   private static Camera _tarCam = null;

   private Transform[] _escapes = null;
   private Transform[] _foams = null;
	private Transform _escape = null;
	private AudioSource _waveSound = null;
   
	//Sets target camera
   public static void init(Camera tarCam){
      if(!_initiated && tarCam != null){
         setTarCam(tarCam);
         _initiated = true;
      }
   }
   
   public static void setTarCam(Camera tarCam){
      _tarCam = tarCam;
   }
   
   //--------------------------------------------------------------------------------
   
	//Spawns an obstacle at the position 'pos'
   public void spawn(Vector3 pos){
      transform.position = pos;
      transform.rotation = Quaternion.Euler(Vector3.up * Random.value * 360);
		if(_escape != null) _escape.rotation = Quaternion.identity;
      wake();
   }

   public Transform[] getEscapes(){
      return _escapes;
   }
   
	//Register to player's displacement effect
   void OnEnable(){
      Tracker.ZMove += playerDis;
   }
   
	//Dismiss from player's displacement effect
   void OnDisable(){
      Tracker.ZMove -= playerDis;
   }
   
   // Use this for initialization
   void Awake () {
      add(this);
      _escape = transform.FindChild("Escape");
      if(_escape.childCount != 0 && _escapes == null){
         _escapes = new Transform[_escape.childCount];
         for(int i=0; i<_escape.childCount; ++i)    _escapes[i] = _escape.GetChild(i);
      }
      Transform foam = transform.FindChild("Foam");
      if(foam.childCount != 0 && _foams == null){
         _foams = new Transform[foam.childCount];
         for(int i=0; i<foam.childCount; ++i){
            _foams[i] = foam.GetChild(i);
            _foams[i].gameObject.SetActive(false);
         }
      }
		_waveSound = transform.GetComponent<AudioSource>() as AudioSource;
      gameObject.SetActive(false);
   }

   public override void restart(){
      _paused = true;
      sleep();
   }

	public void hitSoundPlay(){
		if(GameSystem.soundOn && hitSound != null) hitSound.Play();
	}

   // Update is called once per frame
   void Update () {
		if(_waveSound != null && _waveSound.isPlaying != GameSystem.soundOn){
			if(GameSystem.soundOn)	_waveSound.Play();
			else							_waveSound.Pause();
		}
      if(!_paused){
         //check for inactivity
         checkWithinLimits();
      }
   }
   
	//Checks within active area, being deactivated if outside
   private void checkWithinLimits(){
      Vector3 relPos = transform.position - _tarCam.transform.position;
      if(relPos.z < -8) sleep();
   }
   
	//Process player's displacement effect
   private void playerDis(float zDisp){
      //movement
      transform.position += -Vector3.forward * zDisp;
   }
   
   private void wake(){
      if(!_initiated)   sleep();
      else{
         gameObject.SetActive(true);
         foamSetActive(true);
         resume();
      }
   }

	public void hit(Shot aShot){}
   
	//Activates the foam effect
   private void foamSetActive(bool value){
      if(_foams != null){
         for(int i=0; i<_foams.Length; ++i){
            _foams[i].gameObject.SetActive(value);
         }
      }
   }
   
   private void sleep(){
      if(gameObject.activeSelf){
         ObsSpawner.recicleObs(this);
         gameObject.SetActive(false);
         foamSetActive(false);
      }
   }
}
