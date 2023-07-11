// Copyright Epic Games, Inc. All Rights Reserved.

#include "Project_RoomGameMode.h"
#include "Project_RoomCharacter.h"
#include "UObject/ConstructorHelpers.h"

AProject_RoomGameMode::AProject_RoomGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}
