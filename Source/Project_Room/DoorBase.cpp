// Fill out your copyright notice in the Description page of Project Settings.


#include "DoorBase.h"

ADoorBase::ADoorBase()
{
	DoorFrameStaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Door Frame Static Mesh"));
	RootComponent = DoorFrameStaticMeshComponent;

	DoorStaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Door Static Mesh"));
	DoorStaticMeshComponent->SetupAttachment(DoorFrameStaticMeshComponent);
}

void ADoorBase::DoorInteract()
{
	if (IsOpen)
	{
		DoorStaticMeshComponent->SetRelativeLocation(OpenLocation);
		DoorStaticMeshComponent->SetRelativeRotation(ClosedRotation);
	}
	else
	{
		DoorStaticMeshComponent->SetRelativeLocation(OpenLocation);
		DoorStaticMeshComponent->SetRelativeRotation(OpenRotation);
	}

	IsOpen = !IsOpen;
}
