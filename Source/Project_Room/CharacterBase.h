// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "CharacterBase.generated.h"

class AWeaponBase;
class UHealthComponent;

UCLASS()
class PROJECT_ROOM_API ACharacterBase : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	ACharacterBase();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	//Pawn Weapon
	UPROPERTY(VisibleAnywhere, Category = "Weapon")
	AWeaponBase* Weapon;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	//Shoot the weapon
	void PawnShoot();

	//Try to pick up the weapon
	virtual void PickupWeapon(AWeaponBase* WeaponPicked);

	//Drop the holding weapon
	virtual void DropWeapon();

	//Take damage override
	virtual float TakeDamage(float Damage, struct FDamageEvent const& DamageEvent, AController* EventInstigator, AActor* DamageCauser) override;

	//Function to handle death
	virtual void Die();

private:

	//VARIABLES
	UPROPERTY(VisibleAnywhere, Category = "Health")
	bool IsDead = false;

	//COMPONENTS
	UPROPERTY(VisibleAnywhere)
	UHealthComponent* HealthComponent;

};
