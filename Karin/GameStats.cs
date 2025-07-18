using Microsoft.Xna.Framework;

namespace Karin;

public class GameStats
{
    public float FPS { get; private set; }
    public float UPS { get; private set; }
    public float FUPS { get; private set; }

    int _drawCount;
    int _updateCount;
    int _fixedUpdateCount;
    double _elapsedTime;


    public void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        _updateCount++;
        if (_elapsedTime >= 1)
        {
            FPS = MathF.Round(_drawCount / (float)_elapsedTime);
            UPS = MathF.Round(_updateCount / (float)_elapsedTime);
            FUPS = MathF.Round(_fixedUpdateCount / (float)_elapsedTime);

            _fixedUpdateCount = 0;
            _updateCount = 0;
            _drawCount = 0;
            _elapsedTime = 0;
        }
    }

    public void FixedUpdate(GameTime gameTime)
    {
        _fixedUpdateCount++;
    }

    public void Draw(GameTime gameTime)
    {
        _drawCount++;
    }
}
