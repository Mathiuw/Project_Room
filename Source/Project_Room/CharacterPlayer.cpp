// Fill out your copyright notice in the Description page of Project Settings.


#include "CharacterPlayer.h"
#include "Camera/CameraComponent.h"
#include "InputMappingContext.h"
#include "EnhancedInputSubsystems.h"
#include "EnhancedInputComponent.h"

ACharacterPlayer::ACharacterPlayer()
{
	PrimaryActorTick.bCanEverTick = true;

	CameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
	CameraComponent->SetupAttachment(RootComponent);
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
	
	//Move input
	EnhancedInputComponent->BindAction(MoveInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::Move);

	//Look input
	EnhancedInputComponent->BindAction(LookInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::Look);

	//Interact input
	EnhancedInputComponent->BindAction(InteractInputAction, ETriggerEvent::Triggered, this, &ACharacterPlayer::InteractLineTrace);
}

void ACharacterPlayer::Move(const FInputActionValue& Value)
{
	FVector2D MoveVector = Value.Get<FVector2D>();

	//Add movement
	AddMovementInput(GetActorForwardVector(), MoveVector.X);
	AddMovementInput(GetActorRightVector(), MoveVector.Y);
}

void ACharacterPlayer::Look(const FInputActionValue& Value)
{
	FVector2D LookVector = Value.Get<FVector2D>();

	//Add camera movement
	AddControllerYawInput(LookVector.X);
	AddControllerPitchInput(LookVector.Y);
}

void ACharacterPlayer::InteractLineTrace(const FInputActionValue& Value)
{
	UE_LOG(LogTemp, Display, TEXT("Player tried to interact"));

	FHitResult HitResult;

	//Line trace to try to interact
	//if (GetWorld()->LineTraceSingleByChannel())
	//{

	//} 
}
