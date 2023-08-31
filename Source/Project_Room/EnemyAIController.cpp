// Fill out your copyright notice in the Description page of Project Settings.


#include "EnemyAIController.h"
#include "CharacterBase.h"
#include "Perception/AIPerceptionComponent.h"


void AEnemyAIController::BeginPlay()
{
	Super::BeginPlay();

	//Initiate Behavior Tree
	RunBehaviorTree(BehaviorTree);
}

void AEnemyAIController::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}
