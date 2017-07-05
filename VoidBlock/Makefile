#   <<Copyright 2017 AlduinSG (Silvio Jimenez Osma)>>
#   
#   This file is part of VoidBlock.
#
#    VoidBlock is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    VoidBlock is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with VoidBlock.  If not, see <http://www.gnu.org/licenses/>.
#------------------------------------------------------------------------------

MAINTARGET = Test
TARGETS = dtExceptions dtGeometry dtPhysics
TRASH = *.o *.log
SOURCE = src/
BINARY = bin/
INCLUDE = include/
OBJECT = obj/
LOG = log/
#LIBRARIES=
CC = g++
CCFLAGS = -O2

INPUT = $<
INPUTS = $^
OUTPUT = $@

#------------------------------------------------------------------------------

#$(MYPROGRAM): $(SOURCE)
#
#	$(CC) -I$(MYINCLUDES) $(SOURCE) -o$(MYPROGRAM) -l$(MYLIBRARIES)


$(BINARY)$(MAINTARGET): $(TARGETS:%=$(OBJECT)%.o) $(OBJECT)$(MAINTARGET).o
	$(CC) $(INPUTS) -o $(OUTPUT)

$(OBJECT)$(MAINTARGET).o : $(SOURCE)$(MAINTARGET).cpp
	$(CC) -c $(CCFLAGS) -I $(INCLUDE) $(INPUT) -o $(OUTPUT)

%.o : ../$(INCLUDE)%.h ../$(SOURCE)%.cpp
	$(CC) -c $(CCFLAGS) -I $(INCLUDE) $(OUTPUT:$(OBJECT)%.o=$(SOURCE)%.cpp) -o $(OUTPUT)

#%.h :
#	rm -f $(OUTPUT:$(INCLUDE)%.h=$(OBJECT)%.o)

launch:
	$(BINARY)$(MAINTARGET)

clean: $(TRASH) modify

modify:
	find -name *~ -exec rm -f '{}' \;

$(TRASH):
	find -name $(OUTPUT) -exec rm -f '{}' \;
