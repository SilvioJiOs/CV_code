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

[RequireComponent(typeof(Camera))]
//This class manages the repositioning of the player's target
public class TargetMove : MonoBehaviour {

	public Camera cam;
	public BFParam BField;

	private float _ZDistance;
	private Vector2 _XRange, _YRange;
   private float _clickDown = 0, _clickUp = 0;
   private float _Margin;

	//Sets the limit range parameters up
	void Start(){
      _Margin = Mathf.Min(Screen.width, Screen.height) / 100;
      _Margin *= _Margin;
		_ZDistance = Mathf.Abs(transform.position.z - cam.transform.position.z);
		if(BField != null){
			_XRange = new Vector2();
			_YRange = new Vector2();
			Vector3 BFpos = BField.transform.position;
			_XRange.x = BFpos.x - BField.width * 0.5f;
			_XRange.y = BFpos.x + BField.width * 0.5f;
			_YRange.x = BFpos.y - BField.height * 0.5f;
			_YRange.y = BFpos.y + BField.height * 0.5f;
		}
   }
   
	//Register to camera movement correction
   void OnEnable(){
      SpectatorCamera.CamUpdate += updateTarPos;
   }
   
	//Dismiss from camera movement correction
   void OnDisable(){
      SpectatorCamera.CamUpdate -= updateTarPos;
   }

	//Updates target's position when touch or mouse events detected (depends on the platform where the application is being run)
	private void updateTarPos () {
		if(Application.platform != RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer){
         if(Input.GetMouseButton(0))      _clickDown = Time.time;
         if(Input.GetMouseButtonDown(0))  _clickUp = Time.time;
         if(Input.GetMouseButtonUp(0))    _clickDown = _clickUp;
         if(_clickDown - _clickUp > 0.3f){
				Vector3 finalPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _ZDistance));
				if(BField != null){
					finalPos.x = Mathf.Clamp(finalPos.x, _XRange.x, _XRange.y);
					finalPos.y = Mathf.Clamp(finalPos.y, _YRange.x, _YRange.y);
				}
				transform.position = finalPos;
			}
		}else if(Application.platform == RuntimePlatform.Android){
         _clickDown = Time.time;
			if(Input.touchCount != 0){
				Touch inTouch = Input.GetTouch(0);
            if(_clickDown - _clickUp > 0.2f/*inTouch.deltaPosition.sqrMagnitude > _Margin/*inTouch.phase == TouchPhase.Moved*/ /*|| inTouch.phase == TouchPhase.Stationary*/){
					Vector3 finalPos = cam.ScreenToWorldPoint(new Vector3(inTouch.position.x, inTouch.position.y, _ZDistance));
					if(BField != null){
						finalPos.x = Mathf.Clamp(finalPos.x, _XRange.x, _XRange.y);
						finalPos.y = Mathf.Clamp(finalPos.y, _YRange.x, _YRange.y);
					}
					transform.position = finalPos;
				}
			}else if(Input.touchCount == 0){
            _clickUp = _clickDown;
         }
		}
	}
}
