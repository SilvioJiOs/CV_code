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

//This class handles the pooling and spawning of obstacles objects
public class ObsSpawner : PauseObject{
   
   public Camera cam;
   public BFParam BField;
   [Range(1,5)]
   public static float bottomMargin = 2;
   public List<Obstacle> obsPrefab = new List<Obstacle>();
   [Range(10,30)]
   public int numObs = 13;
   [Range(0.01f,10)]
   public float spawnPeriod = 1;
   public ObstacleSide obsSide = null;
   
   private static List<Obstacle> _obsOn = new List<Obstacle>();
   private static List<Obstacle> _obsOff = new List<Obstacle>();
   private Transform _obsHolder;
   private float _spawnWidth;
   private float _lastSpawn;
   
	//States that the obstacle 'anObs' is reusable for further spawning
   public static void recicleObs(Obstacle anObs){
      _obsOn.Remove(anObs);
      _obsOff.Add(anObs);
   }
   
   //-----------------------------------------------------------------------------------------------
   
	//Frees memory used for enemy pooling when the object is destroyed
   void OnDestroy(){
      Obstacle anObs;
      while(_obsOn.Count>0){
         anObs = _obsOn[0];
         _obsOn.Remove(anObs);
         Destroy(anObs.gameObject);
      }
      while(_obsOff.Count>0){
         anObs = _obsOff[0];
         _obsOff.Remove(anObs);
         Destroy(anObs.gameObject);
      }
   }
   
   // Use this for initialization
	//Initializes obstacle managing structures, initial parameters and battlefield parameters to be taken in further consideration
   void Start () {
		//Set obstacle's spawner position
      transform.position = new Vector3(transform.position.x, BField.transform.position.y - BField.height * 0.5f - bottomMargin, transform.position.z);
      _obsHolder = transform.FindChild("Obstacles");
      _spawnWidth = BField.width - PlayerObsDetector.wide() * 2.1f;
      Obstacle.init(cam);
		//Reserve memory for obstacle objects
      for(int i=0; i<numObs && obsPrefab.Count != 0; ++i){
         GameObject anObs = GameObject.Instantiate(obsPrefab[i%obsPrefab.Count].gameObject) as GameObject;
         anObs.SetActive(false);
         anObs.transform.parent = _obsHolder;
         _obsOff.Add(anObs.GetComponent<Obstacle>());
      }
      if(obsSide != null)
         obsSide.init((int)_spawnWidth, (int)BField.height);
      restart();
   }

   public override void restart(){
      add(this);
      _paused = true;
   }

	//Get next obstacle where to place the obstacle side detector
   public void getNewParent(){
      if(obsSide != null && _obsOn.Count > 1){
         obsSide.setParent(_obsOn[_obsOn.FindIndex(x => x.transform == obsSide.transform.parent)+1]);
      }
   }
   
   // Update is called once per frame
   void Update () {
      if(!_paused){
         spawnObs();
      }else{
         _lastSpawn += Time.deltaTime;
      }
   }
   
	//Spawns a new object into the scene if there are objects left to spawn every 'spawnPeriod' seconds
   private void spawnObs(){
      float now = Time.time;
      if(_obsOff.Count != 0 && now - _lastSpawn > spawnPeriod){
         Obstacle anObs = _obsOff[0];
         _obsOff.Remove(anObs);
         _obsOn.Add (anObs);
         float XPos = (Random.value - 0.5f) * (_spawnWidth - anObs.width);
         anObs.spawn(transform.position + Vector3.right * XPos);
         if(obsSide != null && _obsOn.Count == 1)  obsSide.setParent(anObs);
         _lastSpawn = now;
      }
   }
}
