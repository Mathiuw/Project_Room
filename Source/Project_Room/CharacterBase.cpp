// Fill out your copyright notice in the Description page of Project Settings.


#include "CharacterBase.h"
#include "WeaponBase.h"
#include "HealthComponent.h"

// Sets default values
ACharacterBase::ACharacterBase()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	//Creates health component for the character
	HealthComponent = CreateAbstractDefaultSubobject<UHealthComponent>(TEXT("Health"));

}

// Called when the game starts or when spawned
void ACharacterBase::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ACharacterBase::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void ACharacterBase::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

}

void ACharacterBase::CharacterShoot()
{
	if (Weapon)
	{
		Weapon->ShootWeapon();
	}
}

void ACharacterBase::CharacterReload()
{
	if (Weapon)
	{
		Weapon->ReloadWeapon();
	}
}

AWeaponBase* ACharacterBase::GetWeapon()
{
	if (Weapon)
	{
		return Weapon;
	}
	else return nullptr;
}

void ACharacterBase::PickupWeapon(AWeaponBase* WeaponPicked) 
{
}

void ACharacterBase::DropWeapon() 
{
}

float ACharacterBase::TakeDamage(float Damage, FDamageEvent const& DamageEvent, AController* EventInstigator, AActor* DamageCauser)
{
	Super::TakeDamage(Damage, DamageEvent, EventInstigator, DamageCauser);

	//Return if character is dead
	if (IsDead)
	{
		return Damage;
	}

	//Remove health from component
	HealthComponent->RemoveHealth(Damage);

	if (HealthComponent->GetHealth() <= 0.f)
	{
		//Character die
		Die();
	}

	UE_LOG(LogTemp, Warning, TEXT("Character took damage: %f life remaining"), HealthComponent->GetHealth())

	return Damage;
}

void ACharacterBase::Die()
{	
	IsDead = true;

	DetachFromControllerPendingDestroy();
}