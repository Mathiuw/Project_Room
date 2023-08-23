// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "DoorBase.h"
#include "KeycardDoor.generated.h"

class AKeycardReader;

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AKeycardDoor : public ADoorBase
{
	GENERATED_BODY()

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere, Category = "Keycard Readers")
	TArray<AKeycardReader*> KeycardReaders;

public:

	//Check If All Keycard Readers Are Read
	bool CheckIfAllRead();

};
