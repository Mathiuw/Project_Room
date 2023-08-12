// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InteractableBase.h"
#include "ItemBase.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AItemBase : public AInteractableBase
{
	GENERATED_BODY()
	
public:

	FString GetItemName() const;

private:

	UPROPERTY(EditDefaultsOnly, Category = "Item")
	FString ItemName;
};
