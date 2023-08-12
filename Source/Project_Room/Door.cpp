// Fill out your copyright notice in the Description page of Project Settings.


#include "Door.h"

ADoor::ADoor()
{
	DoorFrameStaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Door Frame Static Mesh"));
	RootComponent = DoorFrameStaticMeshComponent;

	DoorStaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Door Static Mesh"));
	DoorStaticMeshComponent->SetupAttachment(DoorFrameStaticMeshComponent);
}

void ADoor::Interact(ACharacterBase* interactor)
{
	if (IsOpen)
	{
		DoorStaticMeshComponent->SetRelativeRotation(ClosedRotation);
		IsOpen = false;
	}
	else
	{
		DoorStaticMeshComponent->SetRelativeRotation(OpenRotation);
		IsOpen = true;
	}
}
