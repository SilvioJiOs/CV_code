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
// dtExceptions.cpp
//  
// @author:
// Silvio Jiménez Osma
//
//////////////////////////////////////////////////////////////////////

#include <dtExceptions.h>
#include <iostream>
#include <ctime>

bool dtExHandler::_initiated = false;

std::fstream dtExHandler::_output("./log/error.log", std::fstream::out | std::fstream::app);

/**
   *  @brief Initiates the information about the session
   */
void dtExHandler::init(){
	time_t rawtime;
	struct tm * timeinfo;
	char buffer [100];

	time ( &rawtime );
	timeinfo = localtime ( &rawtime );

	strftime (buffer,100,"%a %b %d %X %Z",timeinfo);

	if(!_initiated){
		_output << std::endl << "-------------------------START OF THE REPORT--------------------------" << std::endl;
		_output << "Current time: " << buffer << std::endl;
		_output << "Results:" << std::endl;
		_initiated = true;
	}
}

/**
   *  @brief Frees all the resources used to handle the exceptions
   */
void dtExHandler::free(){
	if(!_initiated){
		init();
		_output << "\n\tNo errors occurred.\n" << std::endl;
	}
	if(_output){
		_output << "*END OF THE REPORT*" << std::endl << std::endl << std::endl;
		_output.close();
	}
}

/**
   *  @brief Handles the exception prompting the corresponding message in the log file
   */
void dtExHandler::handle(const dtException &e){
	if(!_initiated) init();

	_output << "\tException occurred:\n";
	_output << "\t- Module: " << e.module();
	_output << ".\n\t- Object: " << e.object();
	_output << ".\n\t- Error message: \n\t\t" << e.error();
	_output << std::endl << std::endl;

}

