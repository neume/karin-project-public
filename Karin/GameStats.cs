using Microsoft.Xna.Framework;

namespace Karin;

public class GameStats
{
    public float FPS { get; private set; }
    public float UPS { get; private set; }
    public float FUPS { get; private set; }
    public float TotalElapsedTime { get; private set; }

    int _drawCount;
    int _updateCount;
    int _fixedUpdateCount;
    double _elapsedTime;


    public void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        TotalElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
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

    public string TotalElapsedTimeFormatted()
    {
        int hours = (int)TotalElapsedTime / 3600;
        int minutes = (int)TotalElapsedTime / 60;
        int seconds = (int)TotalElapsedTime % 60;
        int milliseconds = (int)(TotalElapsedTime * 1000) % 1000;
        return $"{minutes}:{seconds:00}:{milliseconds:000}";
    }
}
