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

//This class handles the movement and tracking of the camera in the scene
public class SpectatorCamera : PauseObject{

	public Tracker player;
	public float trackTime = 3;
	public float reactionTime = 1;
	public float reactionDistance = 0.2f;
	public float ERROR = 0.001f;
	public Vector3 front = Vector3.forward, up = Vector3.up;

	private float _timer = 0;
	private bool _onTrack = false;
	private CameraBounds _camB;
   private Transform _targetPlayer;
   
   public delegate void void0void();
   public static event void0void CamUpdate;

   void Start(){
		_camB = gameObject.GetComponent<CameraBounds>();
		_targetPlayer = player.transform;
      restart();
   }
   
   public override void restart(){
      add(this);
      _paused = true;
   }

	//Sets the camera's target
	public void SetTarget(Transform t) {
		_targetPlayer = t;
	}
	
	//Sets tracking time
	public void SetTrackTime(float newTime){
		trackTime = newTime;
	}
	
	//Sets reactive tracking time
	public void SetReacTime(float newTime){
		reactionTime = newTime;
	}
	
	//Sets reactive tracking distance
	public void SetReacDist(float newDistance){
		reactionDistance = newDistance;
	}
	
	// Track target
	void LateUpdate() {
		//update camera position depending on target player
		if(_targetPlayer != null){
			float XDiff = _targetPlayer.transform.position.x-transform.position.x;
			float YDiff = _targetPlayer.transform.position.y-transform.position.y;
			float distance = XDiff * XDiff + YDiff * YDiff;
			float sqrDisLimit = reactionDistance * reactionDistance;
			_timer+=Time.deltaTime;
			if(!_onTrack){
				if(_timer > reactionTime || distance - sqrDisLimit > ERROR){
					_onTrack = true;
					_timer = 0;
				}
			}else if(distance > ERROR){
				float percentage;
				if(trackTime == 0)	percentage = 1;
				else 				percentage = (1-Mathf.Cos(Mathf.Min(_timer / trackTime, 1)*Mathf.PI))/2;
				float nextX = XDiff * percentage + transform.position.x;
				float nextY = YDiff * percentage + transform.position.y;
				if(_camB != null){
					nextX = _camB.insideX(nextX);
					nextY = _camB.insideY(nextY);
				}
				transform.position = new Vector3(nextX, nextY, transform.position.z);
			}else{
				_onTrack = false;
				_timer = 0;
			}
		}
	
      //update camera positioning depending functionalities
      if(CamUpdate != null)   CamUpdate();
	}
}