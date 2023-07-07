using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
    public enum State
    {
        Alive,
        Dead,
    }

    public State state;
    public float gridMoveTimer;
    public float gridMoveTimerMax;
    public Vector2Int gridPosition;
    public Direction gridMoveDirection;
    public LevelGrid levelGrid;
    public int snakeBodySize;
    public List<SnakeMovePosition> snakeMovePositionList;
    public List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    { this.levelGrid = levelGrid; }

    private void Awake()
    {     
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovePositionList = new();
        snakeBodySize = 0;
        snakeBodyPartList = new();

        state = State.Alive;
    }

    private void Update()
    {
        if (state == State.Alive)
        {
            HandleInput();
            HandleGridMovement();
        }
        else state = State.Dead;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
                gridMoveDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
                gridMoveDirection = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
                gridMoveDirection = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
                gridMoveDirection = Direction.Left;
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            SoundManager.PlaySound(SoundManager.Sound.SnakeMove);

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            gridMoveDirectionVector = gridMoveDirection switch 
            {
                Direction.Right => new Vector2Int(1, 0),
                Direction.Left => new Vector2Int(-1, 0),
                Direction.Up => new Vector2Int(0,1),
                _ => new Vector2Int(0, -1),
            };
            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                snakeBodySize++;
                CreateSnakeBody();
            }

            if (snakeMovePositionList.Count >= snakeBodySize+1)
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

            UpdateSnakeBodyParts();
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    // Game Over                 
                    state = State.Dead;
                    Loader.Load(Loader.Scene.GameOver);
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);          
        }        
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeMovePositionList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) 
            angle += 360;
        return angle;
    }

    public List<Vector2Int> GetSnakePositionList()
    {
        List<Vector2Int> gridPositionList = new() { gridPosition };
        foreach (SnakeMovePosition bodyPart in snakeMovePositionList)
            gridPositionList.Add(bodyPart.GetGridPosition());
        return gridPositionList;
    }

    public class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            angle = snakeMovePosition.GetDirection() switch
            {
                Direction.Up => 0,
                Direction.Right => 90,
                Direction.Down => 180,
                _ => 270,              
            };
            transform.eulerAngles = new Vector3(0,0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    public class SnakeMovePosition
    {
        public Vector2Int gridPosition;
        public Direction direction;

        public SnakeMovePosition(Vector2Int gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection() 
        { 
            return direction; 
        }
    }
}