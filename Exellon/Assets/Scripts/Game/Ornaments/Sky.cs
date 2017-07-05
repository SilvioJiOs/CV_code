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

//This class handles a pooling of the cloud objects existing in the scene
public class Sky : MonoBehaviour {
	
	public Camera cam;
	public BFParam BField;
//	[Range(0.1f,2)]
//	public float MINSPEED = 0.5f, MAXSPEED = 1;
//	[Range(5,10)]
//	public float MINWAVELEN = 6, MAXWAVELEN = 8;
	public List<Cloud> cloudPrefab = new List<Cloud>();
	[Range(1,30)]
	public int numClouds = 25;
//	[Range(0.1f,2)]
//	public float intPoFac = 0.2f;
//	[Range(5,20)]
//	public float MINWINDTIME = 5, MAXWINDTIME = 20;
	[Range(0.01f,100)]
	public float spawnPeriod = 10;
	
	private static List<Cloud> _cloudsOn = new List<Cloud>();
	private static List<Cloud> _cloudsOff = new List<Cloud>();
	private Transform _cloudHolder;
//	private Vector3 _newDir;
//	private float _newSpeed;
	private Vector2 _spawnArea = new Vector2();
	private float _lastSpawn;
   private float _camDist;
	
	//Notifies the cloud 'aCloud' as a later reusable object
	public static void recicleCloud(Cloud aCloud){
		_cloudsOn.Remove(aCloud);
		_cloudsOff.Add(aCloud);
	}
	
	//-----------------------------------------------------------------------------------------------
	
	//Frees memory handling the pooling when this object is destroyed
	void OnDestroy(){
		Cloud aCloud;
		while(_cloudsOn.Count>0){
			aCloud = _cloudsOn[0];
			_cloudsOn.Remove(aCloud);
			Destroy(aCloud.gameObject);
		}
		while(_cloudsOff.Count>0){
			aCloud = _cloudsOff[0];
			_cloudsOff.Remove(aCloud);
			Destroy(aCloud.gameObject);
		}
	}
	
	// Use this for initialization
	//Reserves memory for cloud objects and sets the parameters for cloud spawning up
	void Start () {
      if(cam != null)   _camDist = Mathf.Abs (cam.transform.position.z - transform.position.z);
		transform.position = new Vector3(transform.position.x, transform.position.y, BField.transform.position.z + BField.depth * 0.495f);
		_cloudHolder = transform.FindChild("Clouds");
		_spawnArea.y = (BField.height + Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(BField.transform.position.z + BField.depth * 0.5f - cam.transform.position.z))*0.5f;
		_spawnArea.x = BField.width + Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(BField.transform.position.z + BField.depth * 0.5f - cam.transform.position.z) * cam.aspect;
		Cloud.init(cam, transform.position);
		for(int i=0; i<numClouds && cloudPrefab.Count != 0; ++i){
			GameObject aCloud = GameObject.Instantiate(cloudPrefab[i%cloudPrefab.Count].gameObject) as GameObject;
			aCloud.SetActive(false);
			aCloud.transform.parent = _cloudHolder;
			_cloudsOff.Add(aCloud.GetComponent<Cloud>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		spawnCloud();
	}
	
	//Spawns a cloud every 'spawnPeriod' seconds
	private void spawnCloud(){
		float now = Time.time;
		if(_cloudsOff.Count != 0 && now - _lastSpawn > spawnPeriod){
			Cloud aCloud = _cloudsOff[0];
			_cloudsOff.Remove(aCloud);
			_cloudsOn.Add (aCloud);
         float random = (Random.value - 0.5f);
         float XPos = Mathf.Max(Mathf.Abs(random * _spawnArea.x), 3) * Mathf.Sign(random);
         float YPos = Mathf.Max((Random.value) * _spawnArea.y, 3);
         float limitZ = Mathf.Max ((_spawnArea.x + aCloud.aspect().x) * _camDist / Mathf.Abs(XPos), (_spawnArea.y + aCloud.aspect().y) * _camDist / YPos);
         float ZPos = Mathf.Max (Random.Range(Mathf.Max(_cloudsOn.Count > 1 ? _cloudsOn[_cloudsOn.Count-2].ZDist() : 500, 500),3000), limitZ);
			aCloud.spawn(new Vector3(XPos, YPos, ZPos));
			_lastSpawn = now;
		}
	}
}
