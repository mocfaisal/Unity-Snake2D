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
            SpawnFood();
            //Debug.Log("Snake Ate Apple");
            result = true;
        }

        return result;
    }
}
