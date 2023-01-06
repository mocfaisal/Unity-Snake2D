using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using CodeMonkey;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left, Right, Up, Down
    }

    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    public float snakeSpeed = 25f;
    AudioSource audioObj;
    public AudioClip diedSound;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = snakeSpeed / 100;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        audioObj = GetComponent<AudioSource>();

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();

        //transform.localScale = new Vector3(2f, 2f, 0);
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            // add posisi snake
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);


            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            //Debug.Log(levelGrid.getFoodGridPosition());

            bool is_snake_eat_food = levelGrid.TrySnakeEatFood(gridPosition);

            if (is_snake_eat_food)
            {
                // snake eat food, then grow body size
                snakeBodySize++;
                createSnakeBodyPart();
            }



            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            /*  for (int i = 0; i < snakeMovePositionList.Count; i++)
              {
                  // add snake size body parts
                  Vector2Int snakeMovePosition = snakeMovePositionList[i];
                  World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
                  FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
              }*/

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            updateSnakeBodyPart();



        }
    }

    private void createSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void updateSnakeBodyPart()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            // set posisi body dengan head
            //Vector3 snakeBodyPosition = new Vector3(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            //Vector2Int snakeBodyPosition = new Vector2Int(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
            //snakeBodyPartList[i].SetSnakeMovePosition(snakeBodyPosition);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        // get angle object
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void died_snake()
    {
        // snake died condition
        Debug.Log("DIE");
        audioObj.PlayOneShot(diedSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            //Destroy(gameObject);
            died_snake();
        }
    }

    public Vector2Int GetGridPosition() { return gridPosition; }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        // FIXME ERROR iniii

        // return list posisi yang telah di pakai oleh snake : head + body
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };

        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.getGridPosition());
        }

        //gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyObj = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyObj.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;

            // scale snake body
            snakeBodyObj.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(2f, 2f, 0);


            // sorting posisi body
            //snakeBodyObj.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;

            transform = snakeBodyObj.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.getGridPosition().x, snakeMovePosition.getGridPosition().y);

            float angle;
            switch (snakeMovePosition.getDirection())
            {
                default:
                case Direction.Up:
                    angle = 0;
                    break;

                case Direction.Down:
                    angle = 180;
                    break;

                case Direction.Left:
                    angle = -90;
                    break;

                case Direction.Right:
                    angle = 90;
                    break;
            }

            // set rotasi
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

    }

    private class SnakeMovePosition
    {
        public Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(Vector2Int gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int getGridPosition()
        {
            return this.gridPosition;
        }

        public Direction getDirection() { return direction; }
    }
}
