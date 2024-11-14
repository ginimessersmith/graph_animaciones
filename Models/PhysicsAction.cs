using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic3D.Models
{
    public class PhysicsAction : Action
    {
        Vector3 initialVelocity;
        Vector3 initialPosition;
        float launchAngle;
        float gravity = -9.81f;

        public PhysicsAction(IObject o,float duration, Vector3 velocity, float angle, Vector3 scaleFactor, Vector3 rotationAngle, Vector3 startPosition = default)
            : base(o,duration,velocity,angle,scaleFactor,rotationAngle)
        {
            initialVelocity = velocity;
            launchAngle = MathHelper.DegreesToRadians(angle);
            initialPosition = startPosition;
        }

        public override void Update(float time)
        {
            //base.Update(time,obj);
            
            if(CurrentTime <= Duration)
            {

                CurrentTime += time;
                float vx = initialVelocity.X * (float)Math.Cos(launchAngle);
                float vy = initialVelocity.Y * (float)Math.Sin(launchAngle) + (gravity * CurrentTime);

                Vector3 newPosition;
                newPosition.X = vx * CurrentTime;
                newPosition.Y = vy * CurrentTime;
                newPosition.Z = initialPosition.Z;

                CurrentPosition = newPosition;

                //Console.WriteLine("currentposition... " + CurrentPosition);

                ApplyTransformation(Obj);
            }
        }
    }
}
