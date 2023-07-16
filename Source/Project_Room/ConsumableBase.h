// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ItemBase.h"
#include "ConsumableBase.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AConsumableBase : public AItemBase
{
	GENERATED_BODY()
	
public:

	virtual void Interact(ACharacterBase* interactor) override;

	void UseItem(ACharacterBase* user);

	void AddAmount();

	void RemoveAmount();

private:
	
	//Amount of the item
	UPROPERTY(VisibleAnywhere, Category = "Item Settings")
	int32 Amount = 1;

	//Max amount if the item
	UPROPERTY(EditDefaultsOnly, Category = "Item Settings")
	int32 MaxAmount = 1;

	//Health amount to be healed
	UPROPERTY(EditDefaultsOnly, Category = "Item Settings")
	int32 HealthHealAmount;
};
