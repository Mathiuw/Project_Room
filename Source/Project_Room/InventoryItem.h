// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ItemBase.h"
#include "InventoryItem.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AInventoryItem : public AItemBase
{
	GENERATED_BODY()
	
public:

	virtual void Interact(ACharacterBase* interactor) override;

	void AddAmount();

	void RemoveAmount();

protected:

	UPROPERTY(VisibleAnywhere, Category = "Item")
	int32 Amount = 1;

	UPROPERTY(EditDefaultsOnly, Category = "Item")
	int32 MaxAmount;
};
