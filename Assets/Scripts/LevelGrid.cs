using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEditor;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObj;
    private int width;
    private int height;
    private Snake snake;
    private ScoreWindow scoreWindow;
    private int curr_score;

    public LevelGrid(int width_, int height_)
    {
        this.width = width_;
        this.height = height_;
        //Debug.Log("Levelgrid Construct");


        //FunctionPeriodic.Create(SpawnFood, 10f);
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;
        Debug.Log("Snake Class Setup");

        // set spawnfood disini karena jika di construct class snake blom ke intitialize
        SpawnFood();

    }
    public void setScoreWindow(ScoreWindow scoreWindow)
    {
        this.scoreWindow = scoreWindow;
    }

    private void SpawnFood()
    {
        // SpawnFood tidak di posisi diatas snake atau di body snake
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(5, width), Random.Range(5, height));
            //foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        //Debug.Log(foodGridPosition);
        //Debug.Log(snake.GetFullSnakeGridPositionList());
        //Debug.Log(snake.GetGridPosition());

        //foodGameObj = new GameObject("Food", typeof(GameObject));
        //foodGameObj.GetComponent<GameObject>() = GameAssets.i.foodObj;

        foodGameObj = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObj.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;

        // scale size food
        foodGameObj.transform.localScale = new Vector3(3f, 3f, 0);

        foodGameObj.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);

    }

    public Vector2Int getFoodGridPosition()
    {
        return foodGridPosition;
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        bool result = false;

        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObj);
            GameHandler.AddScore();
            curr_score = GameHandler.GetScore();
            //Debug.Log("Curr_score " + curr_score.ToString());


            bool is_finish = scoreWindow.showScorePanel(true, curr_score);

            if (is_finish)
            {
                // react requirement score
                snake.playSound("win");
                //snake.died_snake();
                snake.state = Snake.State.Dead;
                //Time.timeScale = 0;
            }
            else
            {
                SpawnFood();
            }

            //Debug.Log("Snake Ate Apple");
            result = true;
        }

        return result;
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        // reset posisi bila keluar dari area kotak
        // teleport kanan kiri
        if (gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }

        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 0;
        }

        // teleport atas bawah
        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }

        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }

        return gridPosition;
    }
}
