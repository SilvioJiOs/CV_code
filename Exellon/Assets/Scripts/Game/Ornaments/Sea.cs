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
using System.Collections.Generic;

//This class manages the sea wave effects
public class Sea : MonoBehaviour {

	public Camera cam;
   public BFParam BField;
   [Range(1,5)]
   public float bottomMargin = 3;
	[Range(0.1f,2)]
	public float MINSPEED = 0.5f, MAXSPEED = 1;
	[Range(5,10)]
	public float MINWAVELEN = 6, MAXWAVELEN = 8;
	public Wave wavePrefab;
	[Range(10,30)]
	public int numWaves = 13;
	[Range(0.1f,2)]
	public float intPoFac = 0.2f;
	[Range(5,20)]
	public float MINWINDTIME = 5, MAXWINDTIME = 20;
	[Range(0.01f,10)]
	public float spawnPeriod = 1;

	//This two lists handles a pool of waves in order to avoid bad memory performance
	private static List<Wave> _wavesOn = new List<Wave>();
	private static List<Wave> _wavesOff = new List<Wave>();
	private static int[] _sections;
	private static int _secI;
	private Transform _waveHolder;
	private Vector3 _newDir;
	private float _newSpeed;
	private Vector2 _spawnArea = new Vector2();
	private float _lastSpawn;

	//States that the wave 'aWave' is ready to be reused later and that its area section is free to spawn a new wave
	public static void recicleWave(Wave aWave, int section){
		_wavesOn.Remove(aWave);
		_wavesOff.Add(aWave);
		if(section >= 0)	_sections[++_secI] = section;
	}

	//-----------------------------------------------------------------------------------------------

	//Clears the memory the lists hold on wave objects when this manager is destroyed
	void OnDestroy(){
		Wave aWave;
		while(_wavesOn.Count>0){
			aWave = _wavesOn[0];
			_wavesOn.Remove(aWave);
			Destroy(aWave.gameObject);
		}
		while(_wavesOff.Count>0){
			aWave = _wavesOff[0];
			_wavesOff.Remove(aWave);
			Destroy(aWave.gameObject);
		}
	}

	// Use this for initialization
	//Sets up parameters of sea position and spawning area, and wave objects are created and stored in lists
	void Start () {
      transform.position = new Vector3(transform.position.x, BField.transform.position.y - BField.height * 0.5f - bottomMargin, transform.position.z);
		_sections = new int[numWaves/2];
		for(int i=0; i<_sections.Length; ++i)	_sections[i] = i;
		_secI = _sections.Length - 1;
		_waveHolder = transform.FindChild("Waves");
		_spawnArea.y = BField.depth * 0.5f;
		float depth = (Mathf.Abs(cam.transform.position.z - BField.transform.position.z) + _spawnArea.y);
		_spawnArea.x = (BField.width * 0.5f + depth * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * cam.aspect) / _sections.Length;
		Wave.init(cam, _spawnArea.x * 0.5f * _sections.Length);
		for(int i=0; i<numWaves; ++i){
			GameObject aWave = GameObject.Instantiate(wavePrefab.gameObject) as GameObject;
			aWave.SetActive(false);
			aWave.transform.parent = _waveHolder;
			_wavesOff.Add(aWave.GetComponent<Wave>());
		}
		Invoke("changeParameters",0);
	}
	
	// Update is called once per frame
	void Update () {
		interNewParams();
		spawnWave();
	}

	//Interpolates change of wind and wave parameters
	private void interNewParams(){
		if(Wave.speed != _newSpeed)	Wave.speed = Mathf.Lerp(Wave.speed, _newSpeed, Time.deltaTime * intPoFac);
		if(Wave.movDir != _newDir)		Wave.movDir = Vector3.Lerp(Wave.movDir, _newDir, Time.deltaTime * intPoFac);
	}

	//Changes general parameters, such as wave speed and wind direction
	private void changeParameters(){
		_newSpeed = Random.Range(MINSPEED, MAXSPEED);
		Vector3 dir = Random.onUnitSphere;
		_newDir = new Vector3(dir.x, 0, -Mathf.Abs(dir.z * 0.5f)).normalized;
		Invoke("changeParameters",Random.Range(MINWINDTIME, MAXWINDTIME));
	}

	//Spawns a new wave in a cleared area section every 'spawnedPeriod' seconds and updates the lists state in
	//order to manage the wave objects previously created during initialization
	private void spawnWave(){
		float now = Time.time;
		if(_wavesOff.Count != 0 && now - _lastSpawn > spawnPeriod){
			Wave aWave = _wavesOff[0];
			_wavesOff.Remove(aWave);
			_wavesOn.Add (aWave);
			float XPos;
			int section;
			if(_secI < 0){
				XPos = (Random.value - 0.5f) * _spawnArea.x * _sections.Length;
				section = -1;
			}else{
				section = Random.Range(0, _secI);
				if(section != _secI){
					int aux = _sections[_secI];
					_sections[_secI] = _sections[section];
					_sections[section] = aux;
				}
				section = _sections[_secI];
				_secI--;
				XPos = (Random.value - 0.5f + section - _sections.Length/2) * _spawnArea.x;
			}
			aWave.spawn(transform.position + new Vector3(XPos, 0, (Random.value * 0.5f + 0.5f) * _spawnArea.y), Random.Range(MINWAVELEN, MAXWAVELEN), section);
			_lastSpawn = now;
		}
	}
}
