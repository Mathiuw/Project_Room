// Fill out your copyright notice in the Description page of Project Settings.


#include "CharacterEnemy.h"

ACharacterEnemy::ACharacterEnemy()
{
	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

void ACharacterEnemy::BeginPlay()
{
	Super::BeginPlay();

}

void ACharacterEnemy::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void ACharacterEnemy::Die()
{
	Super::Die();

	UE_LOG(LogTemp, Warning, TEXT("Enemy Died"))
}
