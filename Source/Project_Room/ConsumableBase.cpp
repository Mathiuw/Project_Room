// Fill out your copyright notice in the Description page of Project Settings.


#include "ConsumableBase.h"
#include "CharacterBase.h"
#include "HealthComponent.h"
#include "CharacterPlayer.h"
#include "Inventory.h"

class ACharacterPlayer;

void AConsumableBase::Interact(ACharacterBase* interactor)
{
	Super::Interact(interactor);

	if (ACharacterPlayer* player = Cast<ACharacterPlayer>(interactor))
	{
		if (UInventory* inventoryComponent = player->GetComponentByClass<UInventory>())
		{
			inventoryComponent->PickupConsumable(this);
			UE_LOG(LogTemp, Warning, TEXT("Player Picked Up Consumable"));
		}
	}
}

void AConsumableBase::UseItem(ACharacterBase* user)
{	
	if (UHealthComponent* UserHealth = user->GetComponentByClass<UHealthComponent>())
	{
		UserHealth->AddHealth(HealthHealAmount);
	}
}

void AConsumableBase::AddAmount()
{
	Amount++;
}

void AConsumableBase::RemoveAmount()
{
	Amount--;
}
