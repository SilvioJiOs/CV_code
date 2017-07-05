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
// dtGeometry.h
//  
// @author:
// Silvio Jiménez Osma
//
//////////////////////////////////////////////////////////////////////

#ifndef DT_GEOMETRY
#define DT_GEOMETRY

#include <iostream>
#include <cmath>
#include <dtExceptions.h>

//////////////////////////////////////////////////////////////////////
//
// dtVector3f class
//
// @Description:
// Describes and represents a vector of three float components
//
//////////////////////////////////////////////////////////////////////

class dtVector3f{
	private:
		float _x,_y,_z;

		//Exception check methods
		void divisionCheck (const float value);
		void normalizeCheck();

	public:
		//Constructors
		dtVector3f() {}
		dtVector3f(const float x, const float y, const float z) : _x(x), _y(y), _z(z) {}
		//dtVector3f & operator= (const dtVector3f copy);

		//Access and modify
		inline const float x() const {return _x;}
		inline const float y() const {return _y;}
		inline const float z() const {return _z;}
		inline void x(const float new_x) {_x = new_x;}
		inline void y(const float new_y) {_y = new_y;}
		inline void z(const float new_z) {_z = new_z;}

		//Operators
		dtVector3f& operator+= (const dtVector3f& sum);
		dtVector3f& operator-= (const dtVector3f& sub);
		inline dtVector3f& operator*= (const dtVector3f& pro) {return *this = *this * pro;}
		dtVector3f& operator*= (const float value);
		dtVector3f& operator/= (const float value);
		inline const dtVector3f operator+ (const dtVector3f& sum) const {return dtVector3f(*this) += sum;}
		inline const dtVector3f operator- (const dtVector3f& sub) const {return dtVector3f(*this) -= sub;}
		inline const dtVector3f operator- () const {return dtVector3f(-_x,-_y,-_z);}
		const dtVector3f operator* (const dtVector3f& pro) const;
		inline const dtVector3f operator* (const float value) const {return dtVector3f(*this) *= value;}
		inline const dtVector3f operator/ (const float value) const {return dtVector3f(*this) /= value;}
		inline const bool operator== (const dtVector3f& comp) const {return _x == comp._x && _y == comp._y && _z == comp._z;}
		inline const bool operator!= (const dtVector3f& comp) const {return _x != comp._x || _y != comp._y || _z != comp._z;}
		inline friend const dtVector3f operator* (const float value, const dtVector3f& pro) {return dtVector3f(pro) *= value;}
		inline friend std::ostream& operator<< (std::ostream& os, const dtVector3f& out) {
						return os << "[" << out._x << "," << out._y << "," << out._z << "]";}

		//Methods
		const dtVector3f& normalize();
		inline const dtVector3f unitary() const {return dtVector3f(*this).normalize();}
		inline const bool iszero() const {return _x == 0 && _y == 0 && _z == 0;}
		inline const float len() const {return iszero() ? 0 : sqrt(_x*_x+_y*_y+_z*_z);}
		inline const float sqlen() const {return iszero() ? 0 : _x*_x+_y*_y+_z*_z;}
		static inline const dtVector3f abs(const dtVector3f& vector) {return dtVector3f(fabs(vector._x),fabs(vector._y),fabs(vector._z));}
		static inline const float dot (const dtVector3f& factA, const dtVector3f& factB) {
						return factA._x*factB._x + factA._y*factB._y + factA._z*factB._z;}
};

//////////////////////////////////////////////////////////////////////
//
// dtQuaternion4f class
//
// @Description:
// Describes and represents a quaternion with one float and one
// dtVector3f components.
//
//////////////////////////////////////////////////////////////////////

class dtQuaternion4f{			//Rotation into quaternion -> dtQuaternion4f(cos(alfa/2)*Vec.len(),sin(alfa/2)*Vec)
	private:							// 								or dtQuaternion4f(cos(alfa/2),sin(alfa/2)*Vec.unitary())
		float _s;
		dtVector3f _vec;

	public:
		//Constructors
		dtQuaternion4f(const float s, const dtVector3f& vec) : _s(s), _vec(vec) {}

		//Access and modify
		inline const float s() const {return _s;}
		inline const dtVector3f& vec() const {return _vec;}
		inline void s(const float new_s) {_s = new_s;}
		inline void vec(const dtVector3f& new_vec) {_vec = new_vec;}

		//Operators
		dtQuaternion4f& operator*= (const dtQuaternion4f& con);
		inline const bool operator== (const dtQuaternion4f& comp) const {return _s == comp._s && _vec == comp._vec;}
		inline const bool operator!= (const dtQuaternion4f& comp) const {return _s != comp._s || _vec != comp._vec;}
		inline const dtQuaternion4f operator- () const {return dtQuaternion4f(_s, -_vec);}
		inline const dtQuaternion4f operator* (const dtQuaternion4f& con) const {return dtQuaternion4f(*this) *= con;}
		inline const dtVector3f operator* (const dtVector3f& point) const {
						return (_s*_s - dot(_vec,_vec))*point + 2*(_s*_vec*point + dot(_vec,point)*_vec);}

		//Methods
		inline friend std::ostream& operator<<(std::ostream& os, const dtQuaternion4f& output) {
						return os << "[" << output._s << "|" << output._vec << "]";}
	
};

/**
   *  @brief Calculates the effective quaternion of a rotation by a vector of angle radians
   *  @param rotVec dtVector3f roation vector
	*  @param angle float radians value
   *  @return dtQuaternion4f quaternion that represents the rotation specified
   */
inline const dtQuaternion4f rotVecToQuat (const dtVector3f& rotVec, const float angle) {
				return dtQuaternion4f(cos(angle*0.5f),sin(angle*0.5f)*rotVec.unitary());};

#endif
