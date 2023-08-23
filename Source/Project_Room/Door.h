// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "DoorBase.h"
#include "Door.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API ADoor : public ADoorBase
{
	GENERATED_BODY()

public:

	//Function To Open Door
	virtual void Interact(ACharacterBase* interactor) override;

};
