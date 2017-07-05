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

//This class handles any pause/resume/reset events that may occur during the application execution
public abstract class PauseObject : MonoBehaviour{

	//List of pause objects that must be handled during execution
   private static List<PauseObject> _pauseEle = new List<PauseObject>();

   protected bool _paused = true;
   protected bool _added = false;
	//Restarts the object (must be implemented by children class)
   public abstract void restart();

	//Pauses the object
   public void pause(){_paused = true;}
	//Resumes the object
   public void resume(){_paused = false;}

	//Add this object to the pause objects list, only if this object is not currently added
   protected void add(PauseObject other){
      if(!_added){
         _pauseEle.Add(other);
         _added = true;
      }
   }

	//Empties pause object list
   public static void clear(){
      _pauseEle.Clear();
   }

	//Pauses every object in the list
   public static void pauseAll(){
      for(int i=0; i<_pauseEle.Count; ++i){
         _pauseEle[i].pause();
      }
   }
   
	//Resumes every object in the list
   public static void resumeAll(){
      for(int i=0; i<_pauseEle.Count; ++i){
         _pauseEle[i].resume();
      }
   }
   
	//Restarts every object in the list
   public static void restartAll(){
      for(int i=0; i<_pauseEle.Count; ++i){
         _pauseEle[i].restart();
      }
   }
}

