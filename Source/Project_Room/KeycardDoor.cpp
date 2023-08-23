// Fill out your copyright notice in the Description page of Project Settings.


#include "KeycardDoor.h"
#include "KeycardReader.h"

void AKeycardDoor::BeginPlay()
{
	//Set all the readers owner to this
	for (AKeycardReader* Reader : KeycardReaders)
	{
		Reader->SetOwner(this);
	}

}

bool AKeycardDoor::CheckIfAllRead()
{
	//Iterate All Readers to Check If They're Are Read
	for (AKeycardReader* Reader : KeycardReaders)
	{
		if (!Reader->GetRead())
		{
			//Reader Aren't Read
			return false;
		}
	}

	//Open the Door
	DoorInteract();
	return true;
}
