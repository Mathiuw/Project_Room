// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CharacterBase.h"
#include "InputActionValue.h"
#include "CharacterPlayer.generated.h"

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
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

private:
	//FUNCTIONS
	void Move(const FInputActionValue& Value);

	void Look(const FInputActionValue& Value);

	void InteractLineTrace(const FInputActionValue& Value);

	//INPUTS
	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputMappingContext* PlayerInputMappingContext;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputAction* MoveInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputAction* LookInputAction;

	UPROPERTY(EditDefaultsOnly, Category = "Input")
	class UInputAction* InteractInputAction;

	//COMPONENTS
	UPROPERTY(VisibleAnywhere)
	class UCameraComponent* CameraComponent;

};
