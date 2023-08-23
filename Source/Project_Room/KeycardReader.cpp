// Fill out your copyright notice in the Description page of Project Settings.


#include "KeycardReader.h"
#include "KeycardDoor.h"

void AKeycardReader::Interact(ACharacterBase* interactor)
{
	if (IsRead)
	{
		UE_LOG(LogTemp, Warning, TEXT("Keycard Reader Already Read"))
		return;
	}

	IsRead = true;

	if (AKeycardDoor* KeycardDoor = Cast<AKeycardDoor>(GetOwner()))
	{
		KeycardDoor->CheckIfAllRead();
	}
}

bool AKeycardReader::GetRead() const
{
	return IsRead;
}
