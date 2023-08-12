// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "InventoryItem.h"
#include "Keycard.generated.h"

/**
 * 
 */

UENUM(BlueprintType)
enum EKeycardColor 
{
	Red,
	Blue,
	Green,
	Yellow,
};

UCLASS()
class PROJECT_ROOM_API AKeycard : public AInventoryItem
{
	GENERATED_BODY()

	UPROPERTY(EditDefaultsOnly, Category = "Keycard Color")
	TEnumAsByte<EKeycardColor> KeycardColor;
};
