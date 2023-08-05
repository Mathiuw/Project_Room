// Fill out your copyright notice in the Description page of Project Settings.


#include "EnemyAIController.h"

void AEnemyAIController::BeginPlay()
{
	Super::BeginPlay();

	//Initiate behavior tree
	RunBehaviorTree(BehaviorTree);
}

void AEnemyAIController::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);



}
