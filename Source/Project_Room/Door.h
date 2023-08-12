// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InteractableBase.h"
#include "Door.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API ADoor : public AInteractableBase
{
	GENERATED_BODY()

public:

	ADoor();

	virtual void Interact(ACharacterBase* interactor) override;

private:

	//COMPONENTS

	//Door Frame
	UPROPERTY(VisibleAnywhere)
	UStaticMeshComponent* DoorFrameStaticMeshComponent;

	//Door
	UPROPERTY(VisibleAnywhere)
	UStaticMeshComponent* DoorStaticMeshComponent;

	//VARIABLES
	UPROPERTY(EditAnywhere)
	FRotator OpenRotation;

	UPROPERTY(EditAnywhere)
	FRotator ClosedRotation;

	UPROPERTY(VisibleAnywhere)
	bool IsOpen = false;

};
