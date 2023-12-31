// Fill out your copyright notice in the Description page of Project Settings.


#include "CharacterPlayer.h"
#include "Camera/CameraComponent.h"
#include "InputMappingContext.h"
#include "EnhancedInputSubsystems.h"
#include "EnhancedInputComponent.h"
#include "InteractableBase.h"
#include "Inventory.h"
#include "WeaponBase.h"

ACharacterPlayer::ACharacterPlayer()
{
	PrimaryActorTick.bCanEverTick = true;

	CameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
	CameraComponent->SetupAttachment(RootComponent);

	WeaponLocation = CreateDefaultSubobject<USceneComponent>(TEXT("Weapon Location"));
	WeaponLocation->SetupAttachment(CameraComponent);

}

void ACharacterPlayer::BeginPlay()
{
	Super::BeginPlay();

	//Get player controller
	APlayerController* playerController = Cast<APlayerController>(GetController());

	//Get enhanced input local player subsystem
	UEnhancedInputLocalPlayerSubsystem* EnhancedInputLocalPlayerSubsystem = 
		ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(playerController->GetLocalPlayer());

	if (playerController && EnhancedInputLocalPlayerSubsystem)
	{
		//Adds the mapping context to the character
		EnhancedInputLocalPlayerSubsystem->AddMappingContext(PlayerInputMappingContext, 0);
	}
}

void ACharacterPlayer::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void ACharacterPlayer::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	//Get the input component to bind the actions
	UEnhancedInputComponent* EnhancedInputComponent = Cast<UEnhancedInputComponent>(PlayerInputComponent);
	
	if (EnhancedInputComponent)
	{
		//Move input
		EnhancedInputComponent->BindAction(MoveInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::Move);
		//Look input
		EnhancedInputComponent->BindAction(LookInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::Look);
		//Interact input
		EnhancedInputComponent->BindAction(InteractInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::InteractLineTrace);
		//Shoot Weapon
		EnhancedInputComponent->BindAction(ShootWeaponInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::CharacterShoot);
		//Reload Weapon
		EnhancedInputComponent->BindAction(ReloadWeaponInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::CharacterReload);
		//Drop Weapon
		EnhancedInputComponent->BindAction(DropWeaponInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::DropWeapon);
	}
}

//Returns the max distance to interact
float ACharacterPlayer::GetInteractDistance() const
{
	return InteractDistance;
}

//Pickup the weapon
void ACharacterPlayer::PickupWeapon(AWeaponBase* WeaponPicked)
{
	Super::PickupWeapon(WeaponPicked);

}

//Drop the weapon
void ACharacterPlayer::DropWeapon()
{
	Super::DropWeapon();

	if (Weapon)
	{
		FVector CameraLocation;
		FRotator CameraRotation;

		//Get camera location and rotation
		GetController()->GetPlayerViewPoint(CameraLocation, CameraRotation);

		//Detach weapon from player
		Weapon->DetachFromActor(FDetachmentTransformRules::KeepWorldTransform);

		//Enable collision
		Weapon->SetActorEnableCollision(true);

		//Enable physics and add impulse
		if (UPrimitiveComponent* PrimitiveComponent = Weapon->GetComponentByClass<UPrimitiveComponent>())
		{
			PrimitiveComponent->SetSimulatePhysics(true);

			PrimitiveComponent->AddImpulse(CameraRotation.Vector() * 10000);
		}

		Weapon->SetActorLocation(CameraLocation);

		Weapon = nullptr;

		UE_LOG(LogTemp, Warning, TEXT("Player Dropped Weapon"))
	}
}

void ACharacterPlayer::Die()
{
	Super::Die();

	UE_LOG(LogTemp, Warning, TEXT("Player Died"))
}

//Move player logic
void ACharacterPlayer::Move(const FInputActionValue& Value)
{
	FVector2D MoveVector = Value.Get<FVector2D>();

	//Add Movement
	AddMovementInput(GetActorForwardVector(), MoveVector.X);
	AddMovementInput(GetActorRightVector(), MoveVector.Y);
}

//Look player camera logic
void ACharacterPlayer::Look(const FInputActionValue& Value)
{
	FVector2D LookVector = Value.Get<FVector2D>();

	//Add Camera Movement
	AddControllerYawInput(LookVector.X);
	AddControllerPitchInput(LookVector.Y);
}

//Line trace to interact with the AInteractable class
void ACharacterPlayer::InteractLineTrace(const FInputActionValue& Value)
{
	FHitResult HitResult;
	FVector Start = CameraComponent->GetComponentLocation();
	FVector End = CameraComponent->GetComponentLocation() + (CameraComponent->GetForwardVector() * InteractDistance);

	//Debug Line
	DrawDebugLine(GetWorld(), Start, End, FColor::Red, false, 1);
	//LineTrace
	if (GetWorld()->LineTraceSingleByChannel(HitResult, Start, End, ECollisionChannel::ECC_Visibility))
	{	
		if (AInteractableBase* interactableActor = Cast<AInteractableBase>(HitResult.GetActor()))
		{
			//Interact with interactable
			interactableActor->Interact(this);
		}

		//Debug Point
		DrawDebugPoint(GetWorld(), HitResult.ImpactPoint, 10, FColor::Red, false, 1);
	} 

	UE_LOG(LogTemp, Display, TEXT("Player tried to interact"));
}