using Graphic3D.Utils;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Graphic3D.Models
{
    public class Action
    {

        public IObject Obj;
        public float Duration;
        public bool IsCompleted { get; private set; }
        
        public Vector3 PositionOffset { get; set; }
        public Vector3 RotationAngle { get; set; }
        public Vector3 ScaleFactor { get; set; }

        private Vector3 _speed;
        public float CurrentTime { get;  set; }
        public Vector3 CurrentPosition { get;  set; }

        Vector3 initialVelocity;
        float launchAngle;
        float gravity = -9.81f;
        Vector3 finalPosition;
        Vector3 startPosition;
        private float initialHeight;
        private float initialVerticalVelocity;
        float initialHorizontalVelocity;

        public Action(IObject obj, float duration, Vector3 velocity, float angle, Vector3 startPosition = default, Vector3 finalPosition = default, Vector3 positionOffset = default, Vector3 rotationAngle = default, Vector3 scaleFactor = default)
        {
            Obj = obj;
            Duration = duration;
            IsCompleted = false;
            PositionOffset = positionOffset;
            RotationAngle = rotationAngle;
            ScaleFactor = scaleFactor;
            CurrentTime = 0f;
            _speed = new Vector3(positionOffset.X/Duration/60, positionOffset.Y / Duration/60, positionOffset.Z / Duration/60);
            CurrentPosition = new Vector3(0f,0f,0f);
            initialVelocity = velocity;
            launchAngle = MathHelper.DegreesToRadians(angle);
            this.finalPosition = finalPosition;
            this.startPosition = startPosition;
            
            initialHorizontalVelocity = (finalPosition.X - startPosition.X) / duration;
            initialVerticalVelocity = (finalPosition.Y - startPosition.Y + 0.5f * gravity * duration * duration) / duration;

        }

        public virtual void Update(float time)
        {
            if (IsCompleted)
            {
                return;
            }

            CurrentTime += time;
            if (CurrentTime > Duration)
            {
                Finish();
            }
            else
            {
                //CalculatePosition();
               
                CurrentPosition = CalculateCurve();
                ApplyTransformation(Obj);
                
                //Console.WriteLine("current: " + _currentTime );
            }
 
        }

        private Vector3 CalculateCurve()
        {
           
            float ratio = CurrentTime / Duration;

            // Vx
            float x = startPosition.X + (finalPosition.X - startPosition.X) * ratio;

            // Vy
            float v0y = (finalPosition.Y - startPosition.Y + 0.5f * 9.81f * Duration * Duration) / Duration; 
            float y =startPosition.Y + initialHeight + v0y * CurrentTime - 0.5f * 9.81f * CurrentTime * CurrentTime;

            
            return new Vector3(x, y, startPosition.Z);

            
        }

        private Vector3 CalculateBezierCurve(Vector3 p0, Vector3 p3, float tTotal, float t)
        {
            float tNormalized = t / tTotal;
            float u = 1 - tNormalized;
            float tt = tNormalized * tNormalized;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * tNormalized;

            Vector3 p = uuu * p0;
            p += 3 * uu * tNormalized * p0;
            p += 3 * u * tt * p3;
            p += ttt * p3;

            return p;
        }

        
        private void CalculatePosition()
        {
            float vx = initialVelocity.X * (float)Math.Cos(launchAngle);
            float vy = initialVelocity.Y * (float)Math.Sin(launchAngle) + (gravity * CurrentTime);

            Vector3 newPosition;
            newPosition.X = vx * CurrentTime;
            newPosition.Y = vy * CurrentTime;
            newPosition.Z = CurrentPosition.Z;

            CurrentPosition = newPosition;

        }

        public void ApplyTransformation(IObject o)
        {
            if (o != null)
            {
                
                o.Translate(new Vertex(),CurrentPosition.X, CurrentPosition.Y, CurrentPosition.Z);
                o.Rotate(o.Center,RotationAngle.X, RotationAngle.Y, RotationAngle.Z);
                o.Scale(ScaleFactor.X, ScaleFactor.Y, ScaleFactor.Z);
            }
        }

        public void Reset()
        {
            CurrentPosition = new Vector3(0f,0f,0f);
            IsCompleted = false;
            CurrentTime = 0f;
        }
        protected void Finish()
        {
            IsCompleted = true;
        }
    }
}
