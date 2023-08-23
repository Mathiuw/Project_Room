// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InteractableBase.h"
#include "KeycardReader.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AKeycardReader : public AInteractableBase
{
	GENERATED_BODY()


public:

	//Set The Keycard Reader to True If Have Keycard
	virtual void Interact(ACharacterBase* interactor);

	bool GetRead() const;

private:

	UPROPERTY(VisibleAnywhere)
	bool IsRead = false;

};
