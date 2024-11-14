using System.Threading;
using System;
using Graphic3D.Models;
using System.Diagnostics;

namespace Graphic3D
{
    public class AnimationController
    {
        private Animation _animation;
        private bool _isPlaying;
        private Thread _animationThread;

        public AnimationController(Animation animation)
        {
            _animation = animation;
        }

        public void Start()
        {
            if (_animationThread == null || !_animationThread.IsAlive)
            {
                _isPlaying = true;
                _animation.Reset(); // resetear cada vez que se inicie la animacion
                _animationThread = new Thread(RunAnimation);
                _animationThread.Start();
            }
        }

        
        private void RunAnimation()
        {
            Stopwatch stopwatch = new Stopwatch();// Crea un cronómetro para medir los intervalos de tiemp
            stopwatch.Start(); 
            long lastTime = stopwatch.ElapsedMilliseconds;

            while (_isPlaying && !_animation.IsCompleted())
            {
                long currentTime = stopwatch.ElapsedMilliseconds;  // Obtiene el tiempo actual en milisegundos
                float deltaTime = (currentTime - lastTime) / 1000f; 
                lastTime = currentTime;

                _animation.Update(deltaTime);

                // Calcula el tiempo de sueño necesario para mantener 60 FPS (milisegundos por frame)
                //int sleepTime = Math.Max(0, (int)(1000 / 60 - deltaTime * 1000));
                //Thread.Sleep(sleepTime);
            }
        }

        public void Stop()
        {
            _isPlaying = false;
            _animationThread?.Join();
            _animation.Reset();
        }

        public void Pause()
        {
            _isPlaying = !_isPlaying;
            if (_isPlaying && (_animationThread == null || !_animationThread.IsAlive))
            {
                _animationThread = new Thread(RunAnimation);
                _animationThread.Start();
            }
        }
    }
}