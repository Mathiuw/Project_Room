// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InventoryItem.h"
#include "ConsumableBase.generated.h"

class ACharacterBase;

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AConsumableBase : public AInventoryItem
{
	GENERATED_BODY()
	
public:

	void UseItem(ACharacterBase* user);

private:

	//Health amount to be healed
	UPROPERTY(EditDefaultsOnly, Category = "Item")
	int32 HealthHealAmount;
};
