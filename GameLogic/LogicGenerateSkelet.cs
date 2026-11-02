using Microsoft.Xna.Framework;
namespace SGame.GameLogic
{
    public class LogicGenerateSkelet
    {
        private int _currentState = 1;
        private readonly Vector2[] FirtsRoute =
        {
            Vector2.Zero
        };

        private readonly Vector2[] Secondoute =
        {
            new Vector2(100, 100)
        };
        private readonly Vector2[] ThirdRoute =
        {
            new Vector2(350, 350)
        };
        public Vector2[] GenerateSkeletRoute()
        {
            Vector2[] route;
            if (_currentState == 0)
                route = FirtsRoute;
            else if (_currentState == 1) 
                route = Secondoute;
            else 
                route = ThirdRoute;

            _currentState++;
            if (_currentState >= 3) _currentState = 0; 

            return route;

        }
    }
}
