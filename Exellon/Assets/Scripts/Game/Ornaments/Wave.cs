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

//This class computes the behaviour of a sea wave
public class Wave : MonoBehaviour {

	public static Vector3 movDir;
	public static float speed;

	private static bool _initiated;
	private static Camera _tarCam = null;
	private static float _witdhLimit;
	private static float _texAsp = 0;

	private Transform _child;
	private float _waveLength;
	private float _startTime;
	private int _section;

	//Initiates class common target camera (in order to know the position the wave must face to) and area limits within the objects remains active
	public static void init(Camera tarCam, float widthLimit){
		if(!_initiated && tarCam != null){
			setTarCam(tarCam);
			_witdhLimit = Mathf.Abs(widthLimit) * 1.2f;
			_initiated = true;
		}
	}

	//Sets the common target camera
	public static void setTarCam(Camera tarCam){
		_tarCam = tarCam;
	}

	//--------------------------------------------------------------------------------

	//Spawns a wave with specific wave length and section in the stated position
	public void spawn(Vector3 pos, float waveL, int section){
		transform.position = pos;
		_waveLength = waveL;
		_startTime = Time.time;
		_section = section;
		wake();
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
	//Sets up the initial state of a wave, getting its renderer component, setting its alpha to 0, and specific parameters to common settings
	void Awake () {
		if(transform.childCount == 1){
			_child = transform.GetChild(0);
			_child.renderer.material.SetFloat("_Alpha", 0);
		}
		if(_texAsp == 0){
			Texture aux = _child.renderer.sharedMaterial.mainTexture;
			_texAsp = aux != null ? aux.width * 1f / aux.height : 1;
		}
		transform.localScale = new Vector3(_texAsp, 0, transform.localScale.z);
		_startTime = Time.time;
		_waveLength = 1;
		_section = -1;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//scale in X and Y axis
		float portion = (Time.time - _startTime)/_waveLength;
      float yScale = Mathf.Cos((portion - 0.5f) * Mathf.PI * 2) * 0.5f + 0.5f;
      float xScale = _texAsp * (1.3f - yScale * 0.5f);
		transform.localScale = new Vector3(xScale, yScale, 1);

		//alpha adjustment
		if(transform.childCount == 1)	_child.renderer.material.SetFloat ("_Alpha", Mathf.Clamp01(transform.localScale.y * 2.5f));

		//movement
		transform.position += movDir * speed * Time.deltaTime;

		//check for inactivity
		if(portion > 1)	sleep();
		checkWithinLimits();
	}

	void LateUpdate(){
		//orientation
		if(_tarCam != null){
			Vector3 relPos = transform.position - _tarCam.transform.position;
			transform.rotation = Quaternion.LookRotation(new Vector3(0, relPos.y, relPos.z));	//change this?
		}	
	}

	//Checks if the wave is outside the active area
	private void checkWithinLimits(){
		Vector3 relPos = transform.position - _tarCam.transform.position;
		if(relPos.z < 0 || Mathf.Abs(relPos.x) > _witdhLimit)	sleep();
	}

	//Applies the player's displacement effect 
	private void playerDis(float zDisp){
		//movement
		//print (Vector3.forward);
		transform.position += -Vector3.forward * zDisp;
	}
	
	//Wakes up a wave object
	private void wake(){
		if(!_initiated)	sleep();
		else{
			if(transform.childCount == 1)	_child.renderer.material.SetFloat("_Alpha", 0);
			transform.localScale = new Vector3(_texAsp, 0, 1);
			gameObject.SetActive(true);
		}
	}
	
	//Sends a wave object to sleep
   private void sleep(){
      if(gameObject.activeSelf){
   		Sea.recicleWave(this, _section);
   		gameObject.SetActive(false);
      }
	}
}
