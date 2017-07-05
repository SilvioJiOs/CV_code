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

//This class handles the general sound state and also the general pause state
public class GameSystem : MonoBehaviour {

	public static bool musicOn = true;
	public static bool soundOn = true;

	// Use this for initialization
	void Start () {
	   
	}

	//Turns music on/off
	public void changeMusic(){
		musicOn = !musicOn;
	}

	//Turns sound on/off
	public void changeSound(){
		soundOn = !soundOn;
	}

	//Pauses the application
   public void pause(){
      PauseObject.pauseAll();
   }

	//Restarts the application
   public void restart(){
      PauseObject.restartAll();
   }

	//Resumes the application
   public void resume(){
      PauseObject.resumeAll();
   }
}
