// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CharacterBase.h"
#include "CharacterEnemy.generated.h"

class AWeaponBase;

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API ACharacterEnemy : public ACharacterBase
{
	GENERATED_BODY()
	
public:
	ACharacterEnemy();

protected:

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere, Category = "Weapon")
	TSubclassOf<AWeaponBase> SpawnWeaponClass;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	//Enemy try to pick up the weapon
	virtual void PickupWeapon(AWeaponBase* WeaponPicked) override;

	//Function to handle death
	virtual void Die() override;

};
