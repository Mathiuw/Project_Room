// Fill out your copyright notice in the Description page of Project Settings.


#include "Inventory.h"
#include "ConsumableBase.h"
#include "CharacterPlayer.h"
#include "EnhancedInputComponent.h"

// Sets default values for this component's properties
UInventory::UInventory()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = false;

}

// Called when the game starts
void UInventory::BeginPlay()
{
	Super::BeginPlay();

	//Inventory input
	if (ACharacterPlayer* player = Cast<ACharacterPlayer>(GetOwner()))
	{
		if (UEnhancedInputComponent* EnhancedComponent = Cast<UEnhancedInputComponent>(player->InputComponent))
		{
			//Drop consumable input bind
			EnhancedComponent->BindAction(DropConsumableInputAction, ETriggerEvent::Triggered, this, &UInventory::DropConsumable);
		}
	}
}

// Called every frame
void UInventory::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

}

void UInventory::PickupConsumable(AConsumableBase* item)
{
	Inventory.Add(item);
	item->Destroy();
}

void UInventory::DropConsumable()
{
	if (ACharacterPlayer* player = Cast<ACharacterPlayer>(GetOwner()))
	{	
		if (Inventory.Num() == 0)
		{
			UE_LOG(LogTemp, Warning, TEXT("Dont have items"))
			return;
		}
		
		AConsumableBase* ItemToBeRemoved = Inventory[Index];

		FVector CameraLocation;
		FRotator CameraRotator;
		//Get player camera location and rotation
		player->GetController()->GetPlayerViewPoint(CameraLocation, CameraRotator);

		FVector SpawnLocation = CameraLocation + CameraRotator.Vector() * 100;

		//Spawn Parameters
		FActorSpawnParameters ActorSpawnParameters = FActorSpawnParameters();
		//Collision handling override
		ActorSpawnParameters.SpawnCollisionHandlingOverride = ESpawnActorCollisionHandlingMethod::AdjustIfPossibleButAlwaysSpawn;

		//Get consumable from array and spawns into the world
		AActor* SpawnedActor = GetWorld()->SpawnActor(ItemToBeRemoved->GetClass(), &SpawnLocation, &CameraRotator, ActorSpawnParameters);

		if (UPrimitiveComponent* PrimitveComponent = SpawnedActor->GetComponentByClass<UPrimitiveComponent>())
		{
			//Enable physics
			PrimitveComponent->SetSimulatePhysics(true);
			//Add impulse to the spawned consumable
			PrimitveComponent->AddImpulse(SpawnedActor->GetActorForwardVector() * DropForce);
		}

		//Remove the consumable from the Inventory array
		Inventory.RemoveAt(Index);
		UE_LOG(LogTemp, Warning, TEXT("Dropped Consumable"))
	}
}