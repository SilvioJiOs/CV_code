/**
<<Copyright 2017 AlduinSG (Silvio Jimenez Osma)>>

This file is part of VoidBlock.

 VoidBlock is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 VoidBlock is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with VoidBlock.  If not, see <http://www.gnu.org/licenses/>.
**/

//////////////////////////////////////////////////////////////////////
//
// dtGeometry.cpp
//  
// @author:
// Silvio Jiménez Osma
//
//////////////////////////////////////////////////////////////////////

#include <dtGeometry.h>

/**
   *  @brief Calculates the addition of two vectors and stores it in the first one
   *  @param sum dtVector3f to add
   *  @return dtVector3f result of the addition
   */
dtVector3f& dtVector3f::operator+= (const dtVector3f& sum){
	_x += sum._x;
	_y += sum._y;
	_z += sum._z;
	return *this;
}

/**
   *  @brief Calculates the subtraction of two vectors and stores it in the first one
   *  @param sub dtVector3f to subtract
   *  @return dtVector3f result of the subtraction
   */
dtVector3f& dtVector3f::operator-= (const dtVector3f& sub){
	_x -= sub._x;
	_y -= sub._y;
	_z -= sub._z;
	return *this;
}

/**
   *  @brief Calculates the product of one vector by a constant and stores it in the quoted vector
   *  @param value float value to multiply
   *  @return dtVector3f result of the product
   */
dtVector3f& dtVector3f::operator*= (const float value){
	if (value !=0 ){
		if(value != 1){
			_x *= value;
			_y *= value;
			_z *= value;
		}
	}else{
		_x = _y = _z = 0;
	}

	return *this;
}

/**
   *  @brief Checks for the value not beeing zero and calculates the division of one vector by a constant value and stores it in the quoted vector
   *  @param value float value to divide
   */
void dtVector3f::divisionCheck(const float value){
	if (value == 0)
		throw dtVecDivZero();
	else if (value != 1){
			_x /= value;
			_y /= value;
			_z /= value;
	} 
}

/**
   *  @brief Calculates the division of one vector by a constant value and stores it in the quoted vector
   *  @param value float value to divide
   *  @return dtVector3f result of the division
   */
dtVector3f& dtVector3f::operator/= (const float value){
	try{
		divisionCheck(value);
	}catch (dtException &e){
		dtExHandler::handle(e);
	}

	return *this;
}

/**
   *  @brief Calculates the cross product of two vectors
   *  @param pro dtVector3f to cross product by
   *  @return dtVector3f result of the addition
   */
const dtVector3f dtVector3f::operator* (const dtVector3f& pro) const{
	dtVector3f result(0,0,0);
	if(!iszero() && !pro.iszero() && !(*this==pro)){
 		result._x = _y * pro._z - _z * pro._y;
 		result._y = _z * pro._x - _x * pro._z;
 		result._z = _x * pro._y - _y * pro._x;
	}
	return result;
}

/**
   *  @brief Checks the previous length of the dtVector3f not being zero and resizes the vector length to one
   *  @return dtVector3f result of the normalization
   */
void dtVector3f::normalizeCheck(){
	float length = len();
	if(length==0) throw dtNormZeroVec();
	if(length!=1) *this/=length;
}

/**
   *  @brief Resizes the vector length to one
   *  @return dtVector3f result of the normalization
   */
const dtVector3f& dtVector3f::normalize(){
	try{
		normalizeCheck();
	}catch (dtException &e){
		dtExHandler::handle(e);
	}

	return *this;
}

/**
   *  @brief Conjugates two quaternions and stores the result in the first one
   *  @param con dtQuaternion4f to conjugate with
   *  @return dtQuaternion4f result of the conjugation
   */
dtQuaternion4f& dtQuaternion4f::operator*= (const dtQuaternion4f& con){
	float old_s(_s);
	_s = _s*con._s - dot(_vec,con._vec);
	_vec = old_s*con._vec + con._s*_vec + _vec*con._vec;
	return *this;
}
