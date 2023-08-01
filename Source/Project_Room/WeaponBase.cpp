// Fill out your copyright notice in the Description page of Project Settings.


#include "WeaponBase.h"
#include "CharacterBase.h"
#include "Kismet/GameplayStatics.h"

void AWeaponBase::Interact(ACharacterBase* interactor)
{
	Super::Interact(interactor);

	if (ACharacterBase* Character = Cast<ACharacterBase>(interactor))
	{
		Character->PickupWeapon(this);
	}
}

void AWeaponBase::ShootWeapon()
{
	if (ACharacterBase* Character = Cast<ACharacterBase>(GetOwner()))
	{
		FVector CameraLocation;
		FRotator CameraRotator;
		//Get character camera location and rotation
		Character->GetController()->GetPlayerViewPoint(CameraLocation, CameraRotator);

		FHitResult HitResult;
		FVector Start = CameraLocation;
		FVector End = Start + CameraRotator.Vector() * WeaponRange;

		//Debug line
		DrawDebugLine(GetWorld(), Start, End, FColor::Red, false, 1);

		if (GetWorld()->LineTraceSingleByChannel(HitResult, Start, End, ECollisionChannel::ECC_Visibility))
		{
			//Debug point
			DrawDebugPoint(GetWorld(), HitResult.ImpactPoint, 10, FColor::Red, false, 1);
			//Apply damage
			UGameplayStatics::ApplyDamage(HitResult.GetActor(), Damage, Character->GetController(), this, UDamageType::StaticClass());

			UE_LOG(LogTemp, Warning, TEXT("Weapon Shot"))
		}
	}

}
