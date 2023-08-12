// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ItemBase.h"
#include "WeaponBase.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API AWeaponBase : public AItemBase
{
	GENERATED_BODY()

protected:

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:

	//Pickpup Weapon
	virtual void Interact(ACharacterBase* interactor)override;

	//Shoot Weapon
	virtual void ShootWeapon();

	//Reload Weapon
	virtual void ReloadWeapon();

	//Getter for Ammo
	UFUNCTION(BlueprintPure)
	float GetAmmo();

	//Getter for Max Ammo
	UFUNCTION(BlueprintPure)
	float GetMaxAmmo();

private:

	UPROPERTY(EditDefaultsOnly, Category = "Weapon")
	float Damage;

	UPROPERTY(VisibleAnywhere, Category = "Weapon")
	float Ammo;

	UPROPERTY(EditDefaultsOnly, Category = "Weapon")
	float MaxAmmo;

	UPROPERTY(EditDefaultsOnly, Category = "Weapon")
	float WeaponRange;

};
