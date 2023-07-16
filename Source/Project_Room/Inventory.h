// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "Inventory.generated.h"

class AConsumableBase;

UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class PROJECT_ROOM_API UInventory : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UInventory();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	void PickupConsumable(AConsumableBase* item);
		
	void DropConsumable();
	
private:	

	UPROPERTY(VisibleAnywhere, Category = "Inventory Settings")
	TArray<AConsumableBase*> Inventory;

	UPROPERTY(EditDefaultsOnly, Category = "Inventory Settings")
	int32 InventorySize = 5;

	UPROPERTY(EditDefaultsOnly, Category = "Inventory Settings")
	float DropForce = 75000;

	UPROPERTY(VisibleAnywhere, Category = "Inventory Settings")
	int32 Index;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputAction* DropConsumableInputAction;

};
