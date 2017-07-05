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
// dtPhysics.h
//
// @author:
// Silvio Jiménez Osma
//
//////////////////////////////////////////////////////////////////////

#ifndef DT_PHYSICS
#define DT_PHYSICS

#include <dtGeometry.h>
#include <dtExceptions.h>

class dtpShape;
class dtpPoint;
class dtpLine;
class dtpSegment;
class dtpPlane;
class dtpBox;
class dtpSphere;
class dtpCapsule;

//////////////////////////////////////////////////////////////////////
//
// dtpType enum
//
// @Description:
// Describes and represents the different types of physics objects.
//
//////////////////////////////////////////////////////////////////////

typedef enum{
	NONE = 0,
	STATIC_FIXED,
	STATIC_MOBILE,
	DYNAMIC
}dtpType;

//////////////////////////////////////////////////////////////////////
//
// dtpShape class
//
// @Description:
// Describes and represents the behavior of any physic shape. This is
// an abstract class and so there is no actual description of a real
// shape, but a description of the shape's functionality and behaviour.
//
//////////////////////////////////////////////////////////////////////

class dtpShape{
	protected:
		dtVector3f _position;

		//Constructors (not able in public scope)
		dtpShape() {}
		dtpShape(const dtpShape& shape);
		dtpShape& operator= (const dtpShape& shape);

	public:
		//Destructor
		virtual ~dtpShape(){}

		//Access and modify
		inline const dtVector3f& pos() const {return _position;}
		inline void pos(const dtVector3f& new_pos) {_position = new_pos;}

		//Methods
		inline void move(const dtVector3f& movVec) {_position += movVec;}
		virtual void rotate(const dtQuaternion4f& rotQuat) {}
		virtual const bool collide(const dtpShape& shape, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpPlane& plane, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpBox& box, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpSphere& sphere, dtpShape* & collS) const =0;
		virtual const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const =0;
};


//////////////////////////////////////////////////////////////////////
//
// dtpPoint class
//
// @Description:
// Describes and represents a point. It is represented by its position
// vector.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpPoint : public dtpShape{
	public:
		//Constructors
		dtpPoint() {}
		dtpPoint(const dtVector3f& pos) {_position = pos;}
		dtpPoint(const dtpPoint& point) {_position = point._position;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpLine class
//
// @Description:
// Describes and represents a line. It is represented as a point
// position vector and a direction vector.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpLine : public dtpShape{
	protected:
		dtVector3f _direction;

	public:
		//Constructors
		dtpLine() {}
		dtpLine(const dtVector3f& pos, const dtVector3f& dir) : _direction(dir.unitary()) {_position = pos;}
		dtpLine(const dtpLine& line) : _direction(line._direction.unitary()) {_position = line._position;}

		//Access and modify
		inline const dtVector3f& dir() const {return _direction;}
		inline void dir(const dtVector3f& new_dir) {_direction = new_dir;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {_direction = rotQuat * _direction;}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpSegment class
//
// @Description:
// Describes and represents a segment. It is represented as a point
// position vector, a direction vector and a length value.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpSegment : public dtpLine{
	private:
		float _length;

	public:
		//Constructors
		dtpSegment() {}
		dtpSegment(const dtVector3f& pos, const dtVector3f& dir, const float length) : _length(length) {_position = pos; _direction = dir.unitary();}
		dtpSegment(const dtpSegment& segment) : _length(segment._length) {_position = segment._position; _direction = segment._direction.unitary();}

		//Access and modify
		inline const float len() const {return _length;}
		inline void len(const float new_len) {_length = new_len;}

		//Methods
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpPlane class
//
// @Description:
// Describes and represents a plane. It is represented as a point
// position vector and a normal vector.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpPlane : public dtpShape{
	private:
		dtVector3f _normal;

	public:
		//Constructors
		dtpPlane();
		dtpPlane(const dtVector3f& pos, const dtVector3f& norm) : _normal(norm.unitary()) {_position = pos;}
		dtpPlane(const dtpPlane& plane) : _normal(plane._normal.unitary()) {_position = plane._position;}

		//Access and modify
		inline const dtVector3f& norm() const {return _normal;}
		inline void norm(const dtVector3f& new_norm) {_normal = new_norm;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {_normal = rotQuat * _normal;}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpBox class
//
// @Description:
// Describes and represents a box. It is represented as a point
// position vector which is the center of the box, three unitary
// vectors that represent the dimension directions (normally they will
// be orthonormal to each other), and one more vector that represents
// the length of each box's dimension.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpBox : public dtpShape{
	private:
		dtVector3f _frVec,_sdVec,_upVec;
		dtVector3f _dimensions;

	public:
		//Constructors
		dtpBox();
		dtpBox(const dtVector3f& position, const dtVector3f& frVec, const dtVector3f& sdVec, const dtVector3f& upVec, const dtVector3f& dimensions) :
				_frVec(frVec.unitary()), _sdVec(sdVec.unitary()), _upVec(upVec.unitary()), _dimensions(abs(dimensions)) {_position = position;}
		dtpBox(const dtpBox& box) :
				_frVec(box._frVec.unitary()), _sdVec(box._sdVec.unitary()), _upVec(box._upVec.unitary()), _dimensions(box._dimensions) {_position = box._position;}

		//Access and modify
		inline const dtVector3f& frontV() const {return _frVec;}
		inline const dtVector3f& sideV() const {return _sdVec;}
		inline const dtVector3f& upV() const {return _upVec;}
		inline const dtVector3f& dimensions() const {return _dimensions;}
		inline void frontV(const dtVector3f& new_frVec) {_frVec = new_frVec;}
		inline void sideV(const dtVector3f& new_sdVec) {_sdVec = new_sdVec;}
		inline void upV(const dtVector3f& new_upVec) {_upVec = new_upVec;}
		inline void dimensions(const dtVector3f& new_dims) {_dimensions = new_dims;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {
			_frVec = rotQuat * _frVec;	_sdVec = rotQuat * _sdVec;	_upVec = rotQuat * _upVec;}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpSphere class
//
// @Description:
// Describes and represents a sphere. It is represented as a point
// position vector that is the center and a float value for the radius.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpSphere : public dtpShape{
	private:
		float _radius;

	public:
		//Constructors
		dtpSphere();
		dtpSphere(const dtVector3f& pos, const float rad) : _radius(rad) {_position = pos;}
		dtpSphere(const dtpSphere& sphere) : _radius(sphere._radius) {_position = sphere._position;}

		//Access and modify
		inline const float rad() const {return _radius;}
		inline void rad(const float new_rad) {_radius = new_rad;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual inline const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

//////////////////////////////////////////////////////////////////////
//
// dtpCapsule class
//
// @Description:
// Describes and represents a capsule. It is represented as a point
// position vector which is the center of the capsule, a vector
// relative to the capsule's center that places the semisphere's
// center (one by adding this vector to the position point and the
// other substracting it), and a float value representing the radius.
// @Extends: dtpShape
//
//////////////////////////////////////////////////////////////////////

class dtpCapsule : public dtpShape{
	private:
		dtVector3f _cenVec;
		float _radius;

	public:
		//Constructors
		dtpCapsule();
		dtpCapsule(const dtVector3f& pos, const dtVector3f& cenVec, const float rad) : _cenVec(cenVec), _radius(rad) {_position = pos;}
		dtpCapsule(const dtpCapsule& capsule) : _cenVec(capsule._cenVec), _radius(capsule._radius) {_position = capsule._position;}

		//Access and modify
		inline const float rad() const {return _radius;}
		inline const dtVector3f& cenVec() const {return _cenVec;}
		inline void rad(const float new_rad) {_radius = new_rad;}
		inline void cenVec(const dtVector3f& new_cenVec) {_cenVec = new_cenVec;}

		//Methods
		inline void rotate(const dtQuaternion4f& rotQuat) {_cenVec = rotQuat * _cenVec;}
		virtual inline const bool collide(const dtpShape& shape, dtpShape* & collS) const {return shape.collide(*this,collS);}
		virtual const bool collide(const dtpPoint& point, dtpShape* & collS) const;
		virtual const bool collide(const dtpLine& line, dtpShape* & collS) const;
		virtual const bool collide(const dtpSegment& segment, dtpShape* & collS) const;
		virtual const bool collide(const dtpPlane& plane, dtpShape* & collS) const;
		virtual const bool collide(const dtpBox& box, dtpShape* & collS) const;
		virtual const bool collide(const dtpSphere& sphere, dtpShape* & collS) const;
		virtual const bool collide(const dtpCapsule& capsule, dtpShape* & collS) const;
};

/**
   *  @brief Checks for a valid dtpShape
   *  @param shape dtpShape pointer to be checked
   */
template <class H>
void validShape(const H& shape){
	if(dynamic_cast<const dtpShape*>(&shape)==0) throw dtWrongShape();
}

/**
   *  @brief Checks if two physic shapes collide or not
   *  @param shapeA dtpShape first shape of the pair to check
	*  @param shapeB dtpShape second shape of the pair to check
	*  @param collP dtVector3f vector value of the point of collision
   *  @return bool true if there is a collision between the two shapes, false otherwise
   */
template <class T, class S>
const bool Collision(const T& shapeA, const S& shapeB, dtpShape* & collS){
	try{
		validShape(shapeA);
		validShape(shapeB);
	}catch (dtException &e){
		dtExHandler::handle(e);
		return false;
	}
	return shapeA.collide(shapeB,collS);
};


//--------------------------------INLINE FUNCTIONS---------------------------------------

const bool dtpPoint::collide(const dtpLine& line, dtpShape* & collS) const {return line.collide(*this,collS);}
const bool dtpPoint::collide(const dtpSegment& segment, dtpShape* & collS) const {return segment.collide(*this,collS);}
const bool dtpPoint::collide(const dtpPlane& plane, dtpShape* & collS) const {return plane.collide(*this,collS);}
const bool dtpPoint::collide(const dtpBox& box, dtpShape* & collS) const {return box.collide(*this,collS);}
const bool dtpPoint::collide(const dtpSphere& sphere, dtpShape* & collS) const {return sphere.collide(*this,collS);}
const bool dtpPoint::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

const bool dtpLine::collide(const dtpSegment& segment, dtpShape* & collS) const {return segment.collide(*this,collS);}
const bool dtpLine::collide(const dtpPlane& plane, dtpShape* & collS) const {return plane.collide(*this,collS);}
const bool dtpLine::collide(const dtpBox& box, dtpShape* & collS) const {return box.collide(*this,collS);}
const bool dtpLine::collide(const dtpSphere& sphere, dtpShape* & collS) const {return sphere.collide(*this,collS);}
const bool dtpLine::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

const bool dtpSegment::collide(const dtpPlane& plane, dtpShape* & collS) const {return plane.collide(*this,collS);}
const bool dtpSegment::collide(const dtpBox& box, dtpShape* & collS) const {return box.collide(*this,collS);}
const bool dtpSegment::collide(const dtpSphere& sphere, dtpShape* & collS) const {return sphere.collide(*this,collS);}
const bool dtpSegment::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

const bool dtpPlane::collide(const dtpBox& box, dtpShape* & collS) const {return box.collide(*this,collS);}
const bool dtpPlane::collide(const dtpSphere& sphere, dtpShape* & collS) const {return sphere.collide(*this,collS);}
const bool dtpPlane::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

const bool dtpBox::collide(const dtpSphere& sphere, dtpShape* & collS) const {return sphere.collide(*this,collS);}
const bool dtpBox::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

const bool dtpSphere::collide(const dtpCapsule& capsule, dtpShape* & collS) const {return capsule.collide(*this,collS);}

#endif
