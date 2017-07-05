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

//This class handles the wind line effect being represented on the obstacle side where the multiplier increases if the player goes trough it
public class WindSystem : MonoBehaviour {

   public Wind[] windPrefabs;
   public int windLines = 10;

	private float _width;
	private float _height;
	private Transform nextParent = null;

   private List<Wind> _lines = new List<Wind>();
	// Use this for initialization
	void Start () {
      for(int i=0; i<windLines; ++i){
         GameObject aWind = Instantiate(windPrefabs[i%windPrefabs.Length].gameObject) as GameObject;
         aWind.SetActive(false);
			aWind.transform.SetParent(transform);
         //aWind.transform.parent = transform;
         _lines.Add(aWind.GetComponent<Wind>());
      }
	}

	//Sets the next obstacle
	public void setNextParent(Transform otherParent){
		nextParent = otherParent;
	}

	//Changes parent to next obstacle
	public void changeParent(){
		transform.parent = nextParent;
	}

	//Spawning range for wind lines
	public void spawnRange(float width, float height){
		_width = width;
		_height = height;
	}

	//Spawn wind wall
   public void spawn(){
      foreach(Wind wind in _lines)  StartCoroutine(arbitrarySpawn(wind, Random.value * 2));
   }

	//Fade out all current wind lines
   public void fadeOut(){
      Wind.fadeOut();
   }

	//Is the fade out effect finished?
   public bool finishedFade(){
      return Wind.finishedFade();
   }

   public void setLocalPos(Vector3 local){
      transform.localPosition = local;
   }

	//Spawn wind lines within the wind wall
   private IEnumerator arbitrarySpawn(Wind wind, float time){
      yield return new WaitForSeconds(time);
		wind.spawn(_width, _height);
      yield return null;
   }
}
