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
//This class manages the player's position within the field boundaries
public class CameraBounds : MonoBehaviour {

	public BFParam BField;
	public Transform target;
	[Range(0f,1f)]
	public float extraMargin = 0.85f;

	private float _XMIN, _XMAX, _YMIN, _YMAX;

	//Here the boundaries are set depending on the battlefield's position and parameters 
	void Start(){
		if(BField != null && target != null){
			float verLim = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * Mathf.Abs(transform.position.z - target.position.z) * extraMargin;
			float horLim = verLim * camera.aspect;
			_YMIN = BField.transform.position.y - BField.height * 0.5f + verLim;
			_YMAX = BField.transform.position.y + BField.height * 0.5f - verLim;
			_XMIN = BField.transform.position.x - BField.width * 0.5f + horLim;
			_XMAX = BField.transform.position.x + BField.width * 0.5f - horLim;
		}
	}

	//Checks within X boundary
	public float insideX(float value){
		return Mathf.Clamp(value, _XMIN, _XMAX);
	}

	//Checks within Y boundary
	public float insideY(float value){
		return Mathf.Clamp(value, _YMIN, _YMAX);
	}

	//Returns the X and Y boundaries' values
	public Vector4 limits(){
		return new Vector4(_XMIN, _XMAX, _YMIN, _YMAX);
	}
}
