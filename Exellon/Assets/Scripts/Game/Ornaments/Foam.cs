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

//This class manages the foam effect of the obstacles base
public class Foam : MonoBehaviour {

	public float rotSpeed = 15;
	public float wavePeriod = 4;
   public bool halfPhase = false;

	private Transform _model;
	private Material _mat;
	private float _phase;
	private float _waveSpeed;
   private static float _MIN_ALPHA = 0.7f;
	private static float _AMPLITUDE = 0.1f;
   private static float _CENTER_VALUE = 1;
	private static float _MIN_VALUE = _CENTER_VALUE - _AMPLITUDE;
	// Use this for initialization
	//Sets the foam's animation parameters up
	void Start () {
		_phase = Random.value * Mathf.PI * 2;
		_model = transform.FindChild ("Model");
		if (transform.childCount != 0 && transform.GetChild (0).childCount != 0)
			_mat = transform.GetChild (0).GetChild (0).renderer.material;
		//_MIN_VALUE = 1 - _AMPLITUDE;
		_waveSpeed = Mathf.PI * 2 / wavePeriod;
	}
	
	// Update is called once per frame
	void Update () {
		float now = Time.time;
		float dTime = Time.deltaTime;
		//Sinus function which controls foam size
		float value = _CENTER_VALUE + _AMPLITUDE * Mathf.Sin (_phase + now * _waveSpeed + (halfPhase ? Mathf.PI : 0));
		_model.localScale = new Vector3 (value, 1, value);
		//Rotates the foam around its up axis
		_model.Rotate (Vector3.up * rotSpeed * dTime);
		//Animates alpha value depending on sinus current phase
		_mat.SetFloat ("_Alpha", 1 - Mathf.Min(_MIN_ALPHA, (value - _MIN_VALUE) / (2 * _AMPLITUDE)));
	}
}
