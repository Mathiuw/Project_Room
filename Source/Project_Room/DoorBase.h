// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InteractableBase.h"
#include "DoorBase.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API ADoorBase : public AInteractableBase
{
	GENERATED_BODY()
	
public:

	ADoorBase();

	void DoorInteract();

protected:

	//COMPONENTS
	UPROPERTY(VisibleAnywhere)
	UStaticMeshComponent* DoorFrameStaticMeshComponent;

	UPROPERTY(VisibleAnywhere)
	UStaticMeshComponent* DoorStaticMeshComponent;

	//VARIABLES
	UPROPERTY(EditAnywhere, Category = "Door")
	FVector OpenLocation;

	UPROPERTY(EditAnywhere, Category = "Door")
	FRotator OpenRotation;

	UPROPERTY(EditAnywhere, Category = "Door")
	FVector ClosedLocation;

	UPROPERTY(EditAnywhere, Category = "Door")
	FRotator ClosedRotation;

	UPROPERTY(VisibleAnywhere, Category = "Door")
	bool IsOpen = false;

};
