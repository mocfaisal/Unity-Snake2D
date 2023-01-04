using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snakez;
    private LevelGrid levelGrid;

    private void Start()
    {
        Debug.Log("GameHandler.Start");

        /* int number = 0;

         FunctionPeriodic.Create(() =>
         {
             CMDebug.TextPopupMouse("Ding! " + number);
             number++;
         }, .3f);*/

        levelGrid = new LevelGrid(20, 20);
        //snake = new Snake();

        snakez.Setup(levelGrid);
        levelGrid.Setup(snakez);
    }

}
