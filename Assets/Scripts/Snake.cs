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

    public enum State
    {
        Alive,
        Dead
    }

    public GameHandler gameHandler;

    public State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    public float snakeSpeed = 25f; // mkain kecil makin cepat
    public ScoreWindow scoreWindow;

    AudioSource audioObj;
    private AudioClip diedSound;
    private AudioClip coinSound;
    private AudioClip completeSound;

    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private int curr_score;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }


    private void Start()
    {
        //gameHandler = new GameHandler();
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = snakeSpeed / 100;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        audioObj = GetComponent<AudioSource>();

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodySize = 0;
        state = State.Alive;
        //transform.localScale = new Vector3(2f, 2f, 0);

        this.diedSound = gameHandler.diedSound;
        this.coinSound = gameHandler.coinSound;
        this.completeSound = gameHandler.completeSound;

    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }

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
            SnakeMovePosition prevSnakePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                prevSnakePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(prevSnakePosition, gridPosition, gridMoveDirection);
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
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            //Debug.Log(levelGrid.getFoodGridPosition());

            bool is_snake_eat_food = levelGrid.TrySnakeEatFood(gridPosition);

            if (is_snake_eat_food)
            {
                // snake eat food, then grow body size
                //audioObj.PlayOneShot(coinSound);
                playSound("coin");
                snakeBodySize++;
                createSnakeBodyPart();
            }


            // jika list move posisi snake lebih besar dari sizeBody
            // maka remove index -1
            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            updateSnakeBodyPart();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.getGridPosition();
                // jika head kena posisi body
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    // game over
                    //CMDebug.TextPopup("Dead!", transform.position);

                    //died_snake();

                    curr_score = GameHandler.GetScore();

                    //Debug.Log(curr_score.ToString());

                    bool is_finish = scoreWindow.showScorePanel(false, curr_score);
                    if (is_finish)
                    {
                        died_snake();
                    }
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);




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

    public void died_snake()
    {
        // snake died condition
        Debug.Log("DIE");
        state = State.Dead;
        //audioObj.PlayOneShot(diedSound);
        playSound("died");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            //Destroy(gameObject);
            //died_snake();

            curr_score = GameHandler.GetScore();

            //Debug.Log(curr_score.ToString());

            bool is_finish = scoreWindow.showScorePanel(false, curr_score);
            if (is_finish)
            {
                died_snake();
            }
        }
    }

    public void playSound(string soundName)
    {
        switch (soundName)
        {
            case "died":
                audioObj.PlayOneShot(diedSound);
                break;

            case "win":
                audioObj.PlayOneShot(completeSound);
                break;
            case "coin":
                audioObj.PlayOneShot(coinSound);
                break;
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

    /*
    Handles Position from the snakes body
    */
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
                    //angle = 0;
                    // snake ke arah atas
                    switch (snakeMovePosition.getPrevDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left:
                            // sebelumnya snake ke arah kiri
                            angle = 0 + 45;
                            break;
                        case Direction.Right:
                            // sebelumnya snake ke arah kanan
                            angle = 0 - 45;
                            break;
                    }
                    break;

                case Direction.Down:
                    //angle = 180;
                    // snake ke arah bawah
                    switch (snakeMovePosition.getPrevDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left:
                            // sebelumnya snake ke arah kiri
                            angle = 180 + 45;
                            break;
                        case Direction.Right:
                            // sebelumnya snake ke arah kanan
                            angle = 180 - 45;
                            break;
                    }
                    break;

                case Direction.Left:
                    //angle = -90;
                    // snake ke arah kiri
                    switch (snakeMovePosition.getPrevDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down:
                            // sebelumnya snake ke arah bawah
                            angle = -45;
                            break;
                        case Direction.Up:
                            // sebelumnya snake ke arah atas
                            angle = 45;
                            break;
                    }
                    break;

                case Direction.Right:
                    //angle = 90;
                    // snake ke arah kanan
                    switch (snakeMovePosition.getPrevDirection())
                    {
                        default:
                            angle = 90;
                            break;
                        case Direction.Down:
                            // sebelumnya snake ke arah bawah
                            angle = 45;
                            break;
                        case Direction.Up:
                            // sebelumnya snake ke arah atas
                            angle = -45;
                            break;
                    }
                    break;
            }

            // set rotasi
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        public Vector2Int getGridPosition()
        {
            return this.snakeMovePosition.getGridPosition();
        }
    }

    /*
     Handles One Move Position from the snakes
     */
    private class SnakeMovePosition
    {
        private SnakeMovePosition prevSnakeMovePosition;
        public Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition prevSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.prevSnakeMovePosition = prevSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int getGridPosition()
        {
            return this.gridPosition;
        }

        public Direction getDirection() { return direction; }
        public Direction getPrevDirection()
        {
            if (this.prevSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return prevSnakeMovePosition.direction;
            }
        }
    }
}
