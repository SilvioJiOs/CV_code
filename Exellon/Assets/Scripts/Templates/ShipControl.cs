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

public class ShipControl : MonoBehaviour{

	public float rollForce = 135;
	public float pitchForce = 90;
	public float gasForce = 2.5f;
	public float brakeForce = 1000;
	public float brakeRotForce = 720;

	public float ROLL_MAX_SPEED_ABS = 1080;
   public float ROLL_MIN_SPEED_ABS = 0;
   public float PITCH_MAX_SPEED_ABS = 720;
   public float PITCH_MIN_SPEED_ABS = 0;
   public float GAS_MAX_SPEED_ABS = 100;
	public float GAS_MIN_SPEED_ABS = 5;
	
	private float rollSpeed;
	private float pitchSpeed;
	private float gasSpeed;

	private int _controlMask;
	private const int _ROLL_LEFT = 1;
	private const int _ROLL_RIGHT = 1<<1;
	private const int _PITCH_UP = 1<<2;
   private const int _PITCH_DOWN = 1<<3;
   private const int _GAS = 1<<4;
   private const int _BRAKE = 1<<5;

	// use this for initialization
	public void init () {
		_controlMask = 0;
		rollSpeed = ROLL_MIN_SPEED_ABS;
		pitchSpeed = PITCH_MIN_SPEED_ABS;
		gasSpeed = GAS_MIN_SPEED_ABS;
	}

	// update function
	public void update (Transform otherTransform, float deltaTime) {
		updateRoll(otherTransform, deltaTime);
		updatePitch(otherTransform, deltaTime);
		updateSpeed(otherTransform, deltaTime);
	}

	private void updateRoll(Transform otherTransform, float deltaTime){
		float newAngle = 0;
		float effectiveForce = 0;
		bool brake = false;
		if((_controlMask & _ROLL_LEFT) != 0){															//Rolling left
			effectiveForce = rollSpeed >= 0 ? Mathf.Abs(rollForce) : Mathf.Abs (brakeRotForce);
		}else if((_controlMask & _ROLL_RIGHT) != 0){													//Rolling right
			effectiveForce = rollSpeed <= 0 ? -Mathf.Abs(rollForce) : -Mathf.Abs (brakeRotForce);
		}else if(Mathf.Abs(rollSpeed) > Mathf.Abs (ROLL_MIN_SPEED_ABS))	brake = true;	//Brake rolling

		if(!brake && effectiveForce != 0){
			newAngle = (Mathf.Abs(rollSpeed) != Mathf.Abs (ROLL_MAX_SPEED_ABS) ? (effectiveForce * deltaTime / 2 + rollSpeed) : rollSpeed) * deltaTime;
			rollSpeed = Mathf.Max(-ROLL_MAX_SPEED_ABS, Mathf.Min(ROLL_MAX_SPEED_ABS, rollSpeed + effectiveForce * deltaTime));
		}else if(Mathf.Abs(rollSpeed) > Mathf.Abs(ROLL_MIN_SPEED_ABS)){
			effectiveForce = rollSpeed > 0 ? -Mathf.Abs (brakeRotForce) : Mathf.Abs (brakeRotForce);
			newAngle = (effectiveForce * deltaTime / 2 + rollSpeed) * deltaTime;
			float oldSpeed = rollSpeed;
			rollSpeed = Mathf.Max(-ROLL_MAX_SPEED_ABS, Mathf.Min(ROLL_MAX_SPEED_ABS, rollSpeed + effectiveForce * deltaTime));
			if(Mathf.Abs(rollSpeed) > Mathf.Abs(oldSpeed)){
				rollSpeed = ROLL_MIN_SPEED_ABS;
				newAngle = 0;
			}
		}

		if(newAngle != 0)	otherTransform.RotateAround(otherTransform.position, otherTransform.forward, newAngle);
	}
	
	private void updatePitch(Transform otherTransform, float deltaTime){
		float newAngle = 0;
		float effectiveForce = 0;
		bool brake = false;
		if((_controlMask & _PITCH_DOWN) != 0){															//Pitching down
			effectiveForce = pitchSpeed >= 0 ? Mathf.Abs(pitchForce) : Mathf.Abs (brakeRotForce);
		}else if((_controlMask & _PITCH_UP) != 0){													//Pitching up
			effectiveForce = pitchSpeed <= 0 ? -Mathf.Abs(pitchForce) : -Mathf.Abs (brakeRotForce);
		}else if(Mathf.Abs(pitchSpeed) > Mathf.Abs (PITCH_MIN_SPEED_ABS))	brake = true;	//Brake pitching
		
		if(!brake && effectiveForce != 0){
			newAngle = (Mathf.Abs(pitchSpeed) != Mathf.Abs (PITCH_MAX_SPEED_ABS) ? (effectiveForce * deltaTime / 2 + pitchSpeed) : pitchSpeed) * deltaTime;
			pitchSpeed = Mathf.Max(-PITCH_MAX_SPEED_ABS, Mathf.Min(PITCH_MAX_SPEED_ABS, pitchSpeed + effectiveForce * deltaTime));
		}else if(Mathf.Abs(pitchSpeed) > Mathf.Abs(PITCH_MIN_SPEED_ABS)){
			effectiveForce = pitchSpeed > 0 ? -Mathf.Abs (brakeRotForce) : Mathf.Abs (brakeRotForce);
			newAngle = (effectiveForce * deltaTime / 2 + pitchSpeed) * deltaTime;
			float oldSpeed = pitchSpeed;
			pitchSpeed = Mathf.Max(-PITCH_MAX_SPEED_ABS, Mathf.Min(PITCH_MAX_SPEED_ABS, pitchSpeed + effectiveForce * deltaTime));
			if(Mathf.Abs(pitchSpeed) > Mathf.Abs(oldSpeed)){
				pitchSpeed = PITCH_MIN_SPEED_ABS;
				newAngle = 0;
			}
		}
		
		if(newAngle != 0)	otherTransform.RotateAround(otherTransform.position, otherTransform.right, newAngle);
	}
	
	private void updateSpeed(Transform otherTransform, float deltaTime){
		float newDistance = 0;
		float effectiveForce = 0;
		if((_controlMask & _GAS) != 0 && gasSpeed < GAS_MAX_SPEED_ABS){						//Accelerate
			effectiveForce = gasForce;
		}else if((_controlMask & _BRAKE) != 0 && gasSpeed < GAS_MIN_SPEED_ABS){				//Brake
			effectiveForce = brakeForce;
		}

		newDistance = (effectiveForce * deltaTime / 2 + gasSpeed) * deltaTime;
		if(effectiveForce != 0)
			gasSpeed = Mathf.Max(GAS_MIN_SPEED_ABS, Mathf.Min(GAS_MAX_SPEED_ABS, gasSpeed + effectiveForce * deltaTime));
		
		if(newDistance != 0)	otherTransform.Translate(otherTransform.forward * newDistance, Space.World);
	}

	public void goPitchUp(){
		stopPitch();	_controlMask |= _PITCH_UP;
	}

	public void goPitchDown(){
		stopPitch();	_controlMask |= _PITCH_DOWN;
	}

	public void goRollLeft(){
		stopRoll();		_controlMask |= _ROLL_LEFT;
	}

	public void goRollRight(){
		stopRoll();		_controlMask |= _ROLL_RIGHT;
   }
   
   public void goGas(){
      stopBrake();	_controlMask |= _GAS;
   }
   
   public void goBrake(){
      stopGas();		_controlMask |= _BRAKE;
   }

	public void stopPitch(){
		_controlMask &= (~_PITCH_UP) & (~_PITCH_DOWN);
	}

	public void stopRoll(){
		_controlMask &= (~_ROLL_LEFT) & (~_ROLL_RIGHT);
	}
   
   public void stopGas(){
      _controlMask &= ~_GAS;
   }
   
   public void stopBrake(){
      _controlMask &= ~_BRAKE;
   }

	public Vector3 shipSpeed(){
		return new Vector3(pitchSpeed, rollSpeed, gasSpeed);
	}
}
