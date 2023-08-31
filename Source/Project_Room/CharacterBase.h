// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GenericTeamAgentInterface.h"
#include "GameFramework/Character.h"
#include "CharacterBase.generated.h"

class AWeaponBase;
class UHealthComponent;

UCLASS()
class PROJECT_ROOM_API ACharacterBase : public ACharacter, public IGenericTeamAgentInterface
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

	//COMPONENTS
	UPROPERTY(VisibleAnywhere)
	USceneComponent* WeaponLocation;

	UPROPERTY(VisibleAnywhere)
	UHealthComponent* HealthComponent;

	//VARIABLES
	UPROPERTY(VisibleAnywhere, Category = "Health")
	bool IsDead = false;



	//Affiliation System
	FGenericTeamId TeamID;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	//Shoot the weapon
	UFUNCTION(BlueprintCallable)
	void CharacterShoot();

	//Reload the weapon
	void CharacterReload();

	//Getter for Weapon if Not Null
	UFUNCTION(BlueprintPure)
	AWeaponBase* GetWeapon() const;

	//Getter For The Character Health Percent
	UFUNCTION(BlueprintPure)
	float GetHealthPercent() const;

	//Getter For If The Character Is Dead
	UFUNCTION(BlueprintPure)
	bool GetIsDead();

	//Try to pick up the weapon
	virtual void PickupWeapon(AWeaponBase* WeaponPicked);

	//Drop the holding weapon
	virtual void DropWeapon();

	//Take Damage Override
	virtual float TakeDamage(float Damage, struct FDamageEvent const& DamageEvent, AController* EventInstigator, AActor* DamageCauser) override;

	//Function to handle death
	virtual void Die();

	//Team ID
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "AI")
	int32 ID = 0;

	//Getters for the team ID
	virtual FGenericTeamId GetGenericTeamId() const override { return TeamID; }
};
