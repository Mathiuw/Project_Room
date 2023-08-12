// Fill out your copyright notice in the Description page of Project Settings.


#include "ConsumableBase.h"
#include "HealthComponent.h"
#include "CharacterBase.h"

void AConsumableBase::UseItem(ACharacterBase* user)
{	
	if (UHealthComponent* UserHealth = user->GetComponentByClass<UHealthComponent>())
	{
		UserHealth->AddHealth(HealthHealAmount);
	}
}