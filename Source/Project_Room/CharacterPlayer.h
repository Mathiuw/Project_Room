// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CharacterBase.h"
#include "InputActionValue.h"
#include "CharacterPlayer.generated.h"

class UInputAction;

/**
 * 
 */
UCLASS()
class PROJECT_ROOM_API ACharacterPlayer : public ACharacterBase
{
	GENERATED_BODY()

public:
	//Player constructor
	ACharacterPlayer();

protected:
	//Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	//Called every frame
	virtual void Tick(float DeltaTime) override;

	//Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	//Return the interact radius
	float GetInteractDistance() const;

	//Try to pick up the weapon
	virtual void PickupWeapon(AWeaponBase* WeaponPicked) override;

	//Drop the holding weapon
	virtual void DropWeapon() override;

	//Function to handle death
	virtual void Die() override;

private:
	//VARIABLES
	UPROPERTY(EditDefaultsOnly, Category = "Interact")
	float InteractDistance = 750;

	//FUNCTIONS
	void Move(const FInputActionValue& Value);

	void Look(const FInputActionValue& Value);

	void InteractLineTrace(const FInputActionValue& Value);

	//INPUTS
	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputMappingContext* PlayerInputMappingContext;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* MoveInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* LookInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* InteractInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* ShootWeaponInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* ReloadWeaponInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	UInputAction* DropWeaponInputAction;

	//COMPONENTS
	UPROPERTY(VisibleAnywhere)
	class UCameraComponent* CameraComponent;

};
