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
	HealthComponent = CreateDefaultSubobject<UHealthComponent>(TEXT("Health"));

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

AWeaponBase* ACharacterBase::GetWeapon() const
{
	if (Weapon)
	{
		return Weapon;
	}
	else return nullptr;
}

float ACharacterBase::GetHealthPercent() const
{
	return (HealthComponent->GetHealth()/HealthComponent->GetMaxHealth());
}

bool ACharacterBase::GetIsDead()
{
	return IsDead;
}

void ACharacterBase::PickupWeapon(AWeaponBase* WeaponPicked) 
{

	if (Weapon)
	{
		//Already has Weapon
		UE_LOG(LogTemp, Warning, TEXT("Character Already Has A Weapon"))
		return;
	}

	if (WeaponPicked == nullptr)
	{
		//ERROR Finding Weapon to Pickup
		UE_LOG(LogTemp, Warning, TEXT("WeaponPicked is NULL"))
		return;
	}

	WeaponPicked->SetOwner(this);

	Weapon = WeaponPicked;

	Weapon->SetActorEnableCollision(false);

	//Enable physics
	if (UPrimitiveComponent* PrimitiveComponent = Weapon->GetComponentByClass<UPrimitiveComponent>())
	{
		PrimitiveComponent->SetSimulatePhysics(false);
	}

	//Attach Weapon
	Weapon->AttachToComponent(WeaponLocation, FAttachmentTransformRules::SnapToTargetNotIncludingScale);

	UE_LOG(LogTemp, Warning, TEXT("Character Picked up Weapon"))

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
		return Damage;
	}

	UE_LOG(LogTemp, Warning, TEXT("Character took damage: %f life remaining"), HealthComponent->GetHealth())

	return Damage;
}

void ACharacterBase::Die()
{	
	IsDead = true;

	DetachFromControllerPendingDestroy();
}