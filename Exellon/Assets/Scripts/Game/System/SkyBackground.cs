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

//This class manages the positioning of the scene background in order to achieve the effect of infinite space (horizon line)
public class SkyBackground : MonoBehaviour {

	public CameraBounds camBou;
	[Range(100,100000)]
	public float fakeFarDis = 10000;
	[Range(1,1.5f)]
	public float extraMargin = 1.1f;

	private Camera _cam;
	private float _extraWidth, _extraHeight;
	private Vector2 _camMargin;
	private Vector3 _refPos;
	private bool _initiated;

	void Start(){
		_initiated = false;
   }
   
	//Register to camera movement correction
   void OnEnable(){
      SpectatorCamera.CamUpdate += updateSkyPos;
   }
   
	//Dismiss from camera movement correction
   void OnDisable(){
      SpectatorCamera.CamUpdate -= updateSkyPos;
   }

	//Late initiation
	//Sets perspective initial parameters up in order to correctly fake the effect of being at a far far away position
	private void init () {
		if(!_initiated && camBou != null){
			_cam = camBou.camera;
			_refPos = camBou.BField.transform.position;
			float distance = Mathf.Abs(_cam.transform.position.z - transform.position.z);
			float height = 2 * Mathf.Tan(_cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * distance;
			float width = height * _cam.aspect;
			Vector4 limits = camBou.limits();
			_camMargin = new Vector2(Mathf.Abs(limits.y - limits.x), Mathf.Abs (limits.w - limits.z));
			float ZDistance = Mathf.Abs(_cam.transform.position.z - transform.position.z);
			_extraHeight = (_camMargin.y / fakeFarDis) * ZDistance;
			_extraWidth = (_camMargin.x /fakeFarDis) * ZDistance;
			transform.localScale = new Vector3(_extraWidth + width, _extraHeight + height, 1);
			_initiated = true;
		}
	}
	
	// Update is called once per frame
	//Calculates new relative position depending on new camera position
	private void updateSkyPos () {
		if(_initiated && _cam != null){
			Vector3 localCam = _cam.transform.position - _refPos;
			Vector3 newLocPos = new Vector3();
			newLocPos.x = (localCam.x / _camMargin.x) * _extraWidth * Mathf.Sign(-localCam.x);
			newLocPos.y = (localCam.y / _camMargin.y) * _extraHeight * Mathf.Sign(-localCam.y);
			transform.localPosition = new Vector3(newLocPos.x, newLocPos.y, transform.localPosition.z);
		}else 	init();
	}
}
