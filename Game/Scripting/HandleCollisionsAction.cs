using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;

// (This is part of my test for adding a delay while not needing "food")
// using System.Threading.Tasks;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (isGameOver == false)
            {
                HandleFoodCollisions(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the snake collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleFoodCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Snake snakeTwo = (Snake)cast.GetFirstActor("snakeTwo");
            Score score = (Score)cast.GetFirstActor("score");
            Food food = (Food)cast.GetFirstActor("food");

            // (just me trying to figure out a delay for the trail growth without using "food")
            // bool timer = true;
            // while (timer) {
            //     int points = food.GetPoints();
            //     await Task.Delay(10000);
            //     snake.GrowTail(points);
            //     snakeTwo.GrowTail(points);
            //     food.Reset();
            // }
            
            if (snake.GetHead().GetPosition().Equals(food.GetPosition()))
            {
                int points = food.GetPoints();
                snake.GrowTail(points);
                score.AddPoints(points);
                food.Reset();
            }
            if (snakeTwo.GetHead().GetPosition().Equals(food.GetPosition()))
            {
                int points = food.GetPoints();
                snakeTwo.GrowTail(points);
                score.AddPoints(points);
                food.Reset();
            }
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Actor head = snake.GetHead();
            List<Actor> segments = snake.GetSegments();
            List<Actor> body = snake.GetBody();
            Snake snakeTwo = (Snake)cast.GetFirstActor("snakeTwo");
            Actor headTwo = snakeTwo.GetHead();
            List<Actor> segmentsTwo = snakeTwo.GetSegments();
            List<Actor> bodyTwo = snakeTwo.GetBody();

            foreach (Actor segment in body)
            {
                if (segment.GetPosition().Equals(head.GetPosition()) || segment.GetPosition().Equals(headTwo.GetPosition()) || head.GetPosition().Equals(headTwo.GetPosition()))
                {
                    isGameOver = true;    
                }            
            }

            foreach (Actor segment in bodyTwo)
            {
                if (segment.GetPosition().Equals(headTwo.GetPosition()) || segment.GetPosition().Equals(head.GetPosition()) || headTwo.GetPosition().Equals(head.GetPosition()))
                {
                    isGameOver = true;
                }                
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Snake snake = (Snake)cast.GetFirstActor("snake");
                List<Actor> segments = snake.GetSegments();
                Snake snakeTwo = (Snake)cast.GetFirstActor("snakeTwo");
                List<Actor> segmentsTwo = snakeTwo.GetSegments();
                Food food = (Food)cast.GetFirstActor("food");

                // create a "game over" message
                int x = Constants.MAX_X / 2;
                int y = Constants.MAX_Y / 2;
                Point position = new Point(x, y);

                Actor message = new Actor();
                message.SetText("Game Over!");
                message.SetPosition(position);
                cast.AddActor("messages", message);

                // make everything white
                foreach (Actor segment in segments)
                {
                    segment.SetColor(Constants.WHITE);
                }

                food.SetColor(Constants.WHITE);
            }
        }

    }
}