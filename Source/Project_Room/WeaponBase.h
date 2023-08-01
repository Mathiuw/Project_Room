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
	
public:

	virtual void Interact(ACharacterBase* interactor)override;

	virtual void ShootWeapon();

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
