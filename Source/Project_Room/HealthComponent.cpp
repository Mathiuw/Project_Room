// Fill out your copyright notice in the Description page of Project Settings.


#include "HealthComponent.h"

// Sets default values for this component's properties
UHealthComponent::UHealthComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = false;

	// ...
}


// Called when the game starts
void UHealthComponent::BeginPlay()
{
	Super::BeginPlay();
	
	Health = MaxHealth;

	GetOwner()->OnTakeAnyDamage.AddDynamic(this, &UHealthComponent::RemoveHealthOnTakeDamage);
}


// Called every frame
void UHealthComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

}

void UHealthComponent::AddHealth(int32 Value)
{
	//Add Health
	Health += Value;
	//Clamp the value
	Health = FMath::Clamp(Health, 0, MaxHealth);
}

void UHealthComponent::RemoveHealth(int32 Value)
{
	//Remove Health
	Health -= Value;
	//Clamp the value
	Health = FMath::Clamp(Health, 0, MaxHealth);
}

void UHealthComponent::RemoveHealthOnTakeDamage(AActor* DamagedActor, float Damage, const class UDamageType* DamageType, class AController* InstigatedBy, AActor* DamageCauser)
{
	RemoveHealth((int32)Damage);
}