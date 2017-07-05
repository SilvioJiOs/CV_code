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

//This class handles a pooling of the island objects existing in the scene
public class Land : MonoBehaviour{
	
	public Camera cam;
	public BFParam BField;
	public List<Island> islandPrefab = new List<Island>();
	[Range(1,30)]
	public int numIslands = 15;
	[Range(0.01f,100)]
	public float spawnPeriod = 100;
	
	private static List<Island> _islandsOn = new List<Island>();
	private static List<Island> _islandsOff = new List<Island>();
	private Transform _islandHolder;
	private float _spawnAreaX;
	private float _lastSpawn;
   private float _seaHeight;
   private float _camDist;
	
	//Notifies the island 'anIsland' as a later reusable object
	public static void recicleIsland(Island anIsland){
		_islandsOn.Remove(anIsland);
		_islandsOff.Add(anIsland);
	}
	
	//-----------------------------------------------------------------------------------------------
	
	//Frees memory handling the pooling when this object is destroyed
	void OnDestroy(){
		Island anIsland;
		while(_islandsOn.Count>0){
			anIsland = _islandsOn[0];
			_islandsOn.Remove(anIsland);
			Destroy(anIsland.gameObject);
		}
		while(_islandsOff.Count>0){
			anIsland = _islandsOff[0];
			_islandsOff.Remove(anIsland);
			Destroy(anIsland.gameObject);
		}
	}
	
	// Use this for initialization
	//Reserves memory for island objects and sets the parameters for island spawning up
	void Start () {
      if(cam != null)   _camDist = Mathf.Abs (cam.transform.position.z - transform.position.z);
		transform.position = new Vector3(transform.position.x, transform.position.y, BField.transform.position.z + BField.depth * 0.495f);
      _seaHeight = BField.transform.position.y - BField.height * 0.5f;
		_islandHolder = transform.FindChild("Islands");
      _spawnAreaX = BField.width + Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(BField.transform.position.z + BField.depth * 0.5f - cam.transform.position.z) * cam.aspect;
		Island.init(cam, transform.position);
		for(int i=0; i<numIslands && islandPrefab.Count != 0; ++i){
			GameObject anIsland = GameObject.Instantiate(islandPrefab[i%islandPrefab.Count].gameObject) as GameObject;
			anIsland.SetActive(false);
			anIsland.transform.parent = _islandHolder;
			_islandsOff.Add(anIsland.GetComponent<Island>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		spawnIsland();
	}
	
	//Spawns an island every 'spawnPeriod' seconds
	private void spawnIsland(){
		float now = Time.time;
		if(_islandsOff.Count != 0 && now - _lastSpawn > spawnPeriod){
			Island anIsland = _islandsOff[0];
			_islandsOff.Remove(anIsland);
			_islandsOn.Add (anIsland);
         float random = (Random.value - 0.5f);
         float XPos = Mathf.Max(Mathf.Abs(random * _spawnAreaX), 3) * Mathf.Sign(random);
         float limitZ = (_spawnAreaX + anIsland.aspect().x) * _camDist / Mathf.Abs(XPos);
         float ZPos = Mathf.Max (Random.Range(Mathf.Max(_islandsOn.Count > 1 ? _islandsOn[_islandsOn.Count-2].ZDist() : 1000, 1000),3000), limitZ);
         anIsland.spawn(new Vector3(XPos, _camDist * _seaHeight / ZPos, ZPos));
			_lastSpawn = now;
		}
	}
}
