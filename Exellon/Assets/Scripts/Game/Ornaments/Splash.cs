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

//This class handles the animation of the splash effect whenever an enemy hits the water surface
public class Splash : MonoBehaviour {

   public float splashTime = 0.2f;
   public float minScale = 0.5f;
   public float maxScale = 1.2f;
   public float solidPerc = 0.7f;
	public AudioSource splashSound = null;

   private Transform _model = null;
   private Material _mat = null;
   private float _startTime;
   private bool _active;
   private float _scaleDif;
   private float _transDif;
   private Transform[] _everything;
   private static float _PHASE = Mathf.PI * 0.5f;
   private Transform _parent;

	//Sets initial animation values up
	void Start () {
      _parent = transform.parent;
      _everything = GetComponentsInChildren<Transform>();
      _active = false;
      _model = transform.FindChild("Model");
      if(_model != null){
         if(_model.childCount != 0 && _model.GetChild(0).renderer != null) _mat = _model.GetChild(0).renderer.material;
         if(_mat != null)  _mat.SetFloat("_Alpha", 0);
      }
      _scaleDif = maxScale - minScale;
      _transDif = 1 - solidPerc;
      setActive(false);
	}

	//Reset water splash animation
   public void launch(Vector3 spawnPoint, float waterLevel){
      transform.parent = null;
      spawnPoint.y = waterLevel;
      transform.position = spawnPoint;
      transform.rotation = Quaternion.identity;
      _startTime = Time.time;
      _active = true;
      setActive(true);
		if(GameSystem.soundOn && splashSound != null)	splashSound.Play();
   }

	// Update is called once per frame
	//Plays water splash animation
	void Update () {
	   if(_active){
         float perc = (Time.time - _startTime)/splashTime;
         _model.localScale = Vector3.one * (minScale + Mathf.Sin(perc * _PHASE) * _scaleDif);		//increase size with time
         float transPerc = (perc - solidPerc)/(_transDif);
         _mat.SetFloat("_Alpha", Mathf.Clamp(1 - transPerc, 0, 1));																	//decrease alpha with time
         _active = perc < 1;
         if(!_active){																																									//deactivate object when animation has finished
            transform.parent = _parent;
            setActive(false);
         }
      }
	}

	//Activates this object's transform tree
   private void setActive(bool value){
      if(_everything != null){
         foreach(Transform gob in _everything)
            gob.gameObject.SetActive(value);
      }
   }

	//Process player's displacement effect
   private void playerDis(float zDisp){
      transform.position += -Vector3.forward * zDisp;
   }

	//Register to player's displacement effect
   void OnDisable(){
      Tracker.ZMove -= playerDis;
   }

	//Dismiss from player's displacement effect
   void OnEnable(){
      Tracker.ZMove += playerDis;
   }
}
