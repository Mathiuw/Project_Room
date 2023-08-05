// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CharacterBase.h"
#include "CharacterEnemy.generated.h"

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

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	//Function to handle death
	virtual void Die() override;

};
