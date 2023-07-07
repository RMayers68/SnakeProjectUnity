using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;      
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }

    public void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(1, width-1), Random.Range(1, height-1));
        } while (snake.GetSnakePositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            Score.AddScore();
            SpawnFood();
            return true;
        }
        return false;
    }

    public Vector2Int ValidateGridPosition( Vector2Int gridPosition)
    {
        if (gridPosition.x <= 0)
            gridPosition.x = width - 1;
        else if (gridPosition.x >= width)
            gridPosition.x = 1;
        if (gridPosition.y <= 0)
            gridPosition.y = height - 1;
        else if (gridPosition.y >= height)
            gridPosition.y = 1;
        return gridPosition;
    }
}