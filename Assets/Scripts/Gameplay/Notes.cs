/*private int CheckEnemyType(int enemyIndex) //Method to check and return which enemytype is selected
        {
            if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                    .enemyType == EnemyType.Enemy_ak47)
            {
                y = 0;
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Enemy_m16)
            {
                y = 1;
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Enemy_Pistol)
            {
                y = 2;
//                
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Interactable)
            {
                y = 3;
            }

            return y;
        }
    }*/
    
    
    
    
/*private void SpawnEnemies()
{
    for (int j = 0;
         j < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel.Count;
         j++)
    {
//                Debug.Log("selected variant enemy Index is:" + j);
        Debug.Log(missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
            .enemyType);
        var g = Instantiate(enemyVariantsPrefab[CheckEnemyType(j)]);
        enemiesInLevel.Add(g.transform);
    }
}*/


/*var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];
          int j;
          var enemiesInLevelCount=selectedWave.enemiesInLevel.Count;

          for (j = 0; j < enemiesInLevelCount; j++)
          {
              var enemyType = selectedWave.enemiesInLevel[j].enemyType;
              Debug.Log(enemyType);

              var variantIndex = CheckEnemyType(j);
              var enemyPrefab = enemyVariantsPrefab[variantIndex];

              enemiesInLevel.Add(Instantiate(enemyPrefab).transform);
          }*/








/*private void SpawnEnemies()
{
           
    for (int j = 0;
         j < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel.Count;
         j++)
    {
//                Debug.Log("selected variant enemy Index is:" + j);
        /*Debug.Log(missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
            .enemyType);#1#
        var enemy = Instantiate(enemyVariantsPrefab[CheckEnemyType(j)],
            missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
                .enemyPosition);
        enemiesInLevel.Add(enemy.transform);
                
        for (int i = 0; i < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j].enemyWaypoints.Length; i++)
        {
                  
            enemiesInLevel[j].gameObject.GetComponent<StateController>().patrolWayPoints.Add(
                missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
                    .enemyWaypoints[i]);

        }
            
               
                
    }
}*/





/*private void SpawnEnemies()
{
    var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];

    for (int j = 0; j < selectedWave.enemiesInLevel.Count; j++)
    {
        var enemyData = selectedWave.enemiesInLevel[j];
        var enemyTypeIndex = CheckEnemyType(j);

        var enemy = Instantiate(enemyVariantsPrefab[enemyTypeIndex], enemyData.enemyPosition);
        enemiesInLevel.Add(enemy.transform);

        if (selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_ak47||selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_m16||selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_Pistol)
        {
            var stateController = enemy.GetComponent<StateController>();
                
            if (stateController != null)
            {
                if (enemyData.enemyWaypoints!=null)
                {
                    stateController.patrolWayPoints.AddRange(enemyData.enemyWaypoints);
                    Debug.Log("Enemies With Waypoints Spawned");
                }

                       
                  
            }
        }
        else
        {
            Debug.Log("Interactable Spawned");
        }
               
    }
}*/