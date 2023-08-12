// Fill out your copyright notice in the Description page of Project Settings.


#include "WeaponBase.h"
#include "CharacterBase.h"
#include "Kismet/GameplayStatics.h"

void AWeaponBase::BeginPlay()
{
	Super::BeginPlay();

	Ammo = MaxAmmo;
}

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
	if (Ammo <= 0)
	{
		UE_LOG(LogTemp, Warning, TEXT("Weapon Out Of Ammo"))
		return;
	}

	if (APawn* Pawn = Cast<APawn>(GetOwner()))
	{
		FVector CameraLocation;
		FRotator CameraRotator;
		//Get character camera location and rotation
		Pawn->GetController()->GetPlayerViewPoint(CameraLocation, CameraRotator);

		FHitResult HitResult;
		FVector Start = CameraLocation;
		FVector End = Start + CameraRotator.Vector() * WeaponRange;

		//Debug line
		DrawDebugLine(GetWorld(), Start, End, FColor::Red, false, 1);

		Ammo--;

		FCollisionQueryParams CollisionQueryParams;
		CollisionQueryParams.AddIgnoredActor(Pawn);

		if (GetWorld()->LineTraceSingleByChannel(HitResult, Start, End, ECollisionChannel::ECC_GameTraceChannel3, CollisionQueryParams))
		{
			//Debug point
			DrawDebugPoint(GetWorld(), HitResult.ImpactPoint, 10, FColor::Red, false, 1);

			//Apply damage
			UGameplayStatics::ApplyDamage(HitResult.GetActor(), Damage, Pawn->GetController(), this, UDamageType::StaticClass());

			UE_LOG(LogTemp, Warning, TEXT("Weapon Shot"))
		}
	}

}

void AWeaponBase::ReloadWeapon()
{
	if (Ammo == MaxAmmo)
	{
		return;
	}

	Ammo = MaxAmmo;

	UE_LOG(LogTemp, Warning, TEXT("Weapon Reloaded"))
}

float AWeaponBase::GetAmmo()
{
	return Ammo;
}

float AWeaponBase::GetMaxAmmo()
{
	return MaxAmmo;
}
