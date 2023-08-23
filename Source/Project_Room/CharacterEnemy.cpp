// Fill out your copyright notice in the Description page of Project Settings.


#include "CharacterEnemy.h"
#include "WeaponBase.h"

ACharacterEnemy::ACharacterEnemy()
{
	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	WeaponLocation = CreateDefaultSubobject<USceneComponent>(TEXT("Weapon Location"));
	WeaponLocation->SetupAttachment(RootComponent);

}

void ACharacterEnemy::BeginPlay()
{
	Super::BeginPlay();

	if (SpawnWeaponClass)
	{
		AWeaponBase* SpawnedWeapon = GetWorld()->SpawnActor<AWeaponBase>(SpawnWeaponClass, GetActorTransform());

		PickupWeapon(SpawnedWeapon);
	}
}

void ACharacterEnemy::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void ACharacterEnemy::PickupWeapon(AWeaponBase* WeaponPicked)
{
	Super::PickupWeapon(WeaponPicked);
}

void ACharacterEnemy::Die()
{
	Super::Die();

	UE_LOG(LogTemp, Warning, TEXT("Enemy Died"))
}
