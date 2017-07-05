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

//This class implements the behaviour of an element as being much more far away than it really is
public abstract class FakeFarElement : MonoBehaviour {

	public float objectSize = 30;
   [Range(1,5)]
   public float fadeInTime = 2;

   private static float solidDistance = 5000;
   private Vector3 fakePosition = new Vector3(-100,100,10000);
   private static bool _initiated = false;
	private static Transform _cam = null;
   private static float _camDist;
   private float _texAsp = 0;
   private SpriteRenderer _child = null;
	private float _spawnTime;
   
	//Initializes the target camera and the plane position where the fake far elements are being placed
   public static void init(Camera tarCam, Vector3 fatherPos){
      if(!_initiated && tarCam != null){
         _cam = tarCam.transform;
         _camDist = Mathf.Abs (_cam.position.z - fatherPos.z);
         _initiated = true;
      }
   }

	//Sets up parameters in order to further handle this objects size, position and alpha values
	void Awake(){
      if(transform.childCount == 1){
         _child = transform.GetChild(0).GetComponent<SpriteRenderer>();
         if(_texAsp == 0){
            Sprite tex = _child.sprite;
            _texAsp = tex != null ? tex.texture.width/tex.texture.height : 1;
            float reScale = tex.pixelsPerUnit / tex.texture.height;
            _child.transform.localScale = Vector3.one * reScale;
         }
      }
	}

	//Handles fade in effect and the fake size and position of the object in order to simulate it being far far away from the camera
	private void updateView(){
		if(_initiated){
         float timeDiff = Time.time - _spawnTime;
         float alphaEnter = timeDiff/fadeInTime;
			//Calculates the alpha of the object depending on fade in effect time and fake distance to camera
         float alphaDist = (10000 - fakePosition.z)/solidDistance;
         if(alphaEnter <= 1 || alphaDist <=1 && _child != null){
            _child.renderer.material.SetFloat("_Alpha", Mathf.Min(1, Mathf.Min(alphaEnter, alphaDist)));
         }
			//Calculates real position and size depending of fake initial size and fake current position
			Vector3 relPos = fakePosition - _cam.position;
			float factor = _camDist / relPos.z;
         float zScaleDis = (10000 - relPos.z) / 10000;
         _child.sortingOrder = -(int)fakePosition.z;
			transform.position = new Vector3(_cam.position.x + relPos.x * factor, _cam.position.y + relPos.y * factor, transform.parent.position.z - zScaleDis * 3);
			transform.localScale = Vector3.one * factor * objectSize;
         checkWithinLimits();
		}
	}
	
	//Register to player's displacement effect and camera repositioning correction
	void OnEnable(){
		Tracker.ZMove += playerDis;
      SpectatorCamera.CamUpdate += updateView;
	}
	
	//Dismiss from player's displacement effect and camera repositioning correction
	void OnDisable(){
		Tracker.ZMove -= playerDis;
      SpectatorCamera.CamUpdate -= updateView;
	}
	
	//Process player's displacement effect
	private void playerDis(float zDisp){
		//movement
		fakePosition += -Vector3.forward * zDisp;
	}
	
	//Checks within active area limits
	private void checkWithinLimits(){
		if(fakePosition.z < transform.parent.position.z)	sleep();
      //if(fakePosition.z < _cam.position.z)   sleep();
	}

	//Wakes a fake far element up
	private void wake(){
      if(!_initiated) sleep();
      else{
         if(transform.childCount == 1)   _child.renderer.material.SetFloat("_Alpha", 0);
         gameObject.SetActive(true);
      }
   }

	//Spawns a fake far element within the scene at a position 'pos'
   public void spawn(Vector3 pos){
      fakePosition = (Vector3.forward + new Vector3(pos.x, pos.y, 0) / _camDist) * pos.z;
      _spawnTime = Time.time;
      wake();
   }

	//Returns the fake distance to camera
   public float ZDist(){return fakePosition.z;}

	//Returns the aspect ratio of the object's image
   public Vector2 aspect(){return new Vector2(_texAsp, 1) * objectSize;}

	//Sends the object to sleep (must be implemented by children class)
   protected abstract void sleep();
}
