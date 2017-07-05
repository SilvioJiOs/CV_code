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
// dtExceptions.h
//  
// @author:
// Silvio Jiménez Osma
//
//////////////////////////////////////////////////////////////////////

#ifndef DT_EXCEPTIONS
#define DT_EXCEPTIONS

#include <fstream>


//////////////////////////////////////////////////////////////////////
//
// dtException class
//
// @Description:
// Describes and represents the information related with an exception
//
//////////////////////////////////////////////////////////////////////

class dtException{

	public:

		//Access
		virtual const char* module() const =0;
		virtual const char* object() const =0;
		virtual const char* error() const =0;
};

//////////////////////////////////////////////////////////////////////
//
// dtExcGeo class
//
// @Description:
// Describes and represents the information related with an exception
// of the Geometry module
//
//////////////////////////////////////////////////////////////////////

class dtExcGeo : public dtException{

	public:

		//Access
		virtual inline const char* module() const {return "Geometry";}
};

//////////////////////////////////////////////////////////////////////
//
// dtExcGeoVec class
//
// @Description:
// Describes and represents the information related with an exception
// of the Geometry module about an dtVector3f object
//
//////////////////////////////////////////////////////////////////////

class dtExcGeoVec : public dtExcGeo{

	public:

		//Access
		virtual inline const char* object() const {return "dtVector3f";}
};

//////////////////////////////////////////////////////////////////////
//
// dtVecDivZero class
//
// @Description:
// Exception occurred when attempting to divide a dtVector3f object by
// scalar 0
//
//////////////////////////////////////////////////////////////////////

class dtVecDivZero : public dtExcGeoVec{

	public:

		//Access
		virtual inline const char* module() const {return dtExcGeo::module();}
		virtual inline const char* object() const {return dtExcGeoVec::object();}
		virtual inline const char* error() const {return "Attempt to divide by zero.";}
};

//////////////////////////////////////////////////////////////////////
//
// dtNormZeroVec class
//
// @Description:
// Exception occurred when attempting to normalize a dtVector3f object
// which length is 0
//
//////////////////////////////////////////////////////////////////////

class dtNormZeroVec : public dtExcGeoVec{

	public:

		//Access
		virtual inline const char* module() const {return dtExcGeo::module();}
		virtual inline const char* object() const {return dtExcGeoVec::object();}
		virtual inline const char* error() const {return "Attempt to normalize a zero length vector.";}
};

//////////////////////////////////////////////////////////////////////
//
// dtExcPhys class
//
// @Description:
// Describes and represents the information related with an exception
// of the Physics module
//
//////////////////////////////////////////////////////////////////////

class dtExcPhys : public dtException{

	public:

		//Access
		virtual inline const char* module() const {return "Physics";}
};

//////////////////////////////////////////////////////////////////////
//
// dtExcPhysShape class
//
// @Description:
// Describes and represents the information related with an exception
// of the Physics module about a dtpShape object
//
//////////////////////////////////////////////////////////////////////

class dtExcPhysShape : public dtExcPhys{

	public:

		//Access
		virtual inline const char* object() const {return "dtpShape";}
};

//////////////////////////////////////////////////////////////////////
//
// dtWrongShape class
//
// @Description:
// Describes and represents the information related with an exception
// of the Physics module about a dtpShape object
//
//////////////////////////////////////////////////////////////////////

class dtWrongShape : public dtExcPhysShape{

	public:

		//Access
		virtual inline const char* error() const {return "The shape pointer is invalid or not initiated.";}
};

//////////////////////////////////////////////////////////////////////
//
// dtExcExc class
//
// @Description:
// Describes and represents the information related with an exception
// of the Exceptions module
//
//////////////////////////////////////////////////////////////////////

class dtExcExc : public dtException{

	public:

		//Access
		virtual inline const char* module() const {return "Exceptions";}
};

//////////////////////////////////////////////////////////////////////
//
// dtExHandler class
//
// @Description:
// Class that represents an interface to handle the exceptions that may occur
//
//////////////////////////////////////////////////////////////////////

class dtExHandler{
	private:
		static std::fstream _output;
		static bool _initiated;
		static void init();

	public:
		static void free();
		static void handle(const dtException &e);
};
#endif

