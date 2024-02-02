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