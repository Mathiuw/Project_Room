// Fill out your copyright notice in the Description page of Project Settings.


#include "InventoryItem.h"
#include "CharacterPlayer.h"
#include "Inventory.h"

void AInventoryItem::Interact(ACharacterBase* interactor)
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

void AInventoryItem::AddAmount()
{
	Amount++;
}

void AInventoryItem::RemoveAmount()
{
	Amount--;
}