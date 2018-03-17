using System;
using System.Collections.Generic;
using ConsoleGraphics.Core;

namespace ConsoleGraphics
{
    public class PhysicsObject
    {
        private Mesh mesh;
        private Point3D velocity;
        private Point3D angularVelocity;
        private int mass;
        private float radius;

        public static Point3D grav = new Point3D(0, -0.5f, 0);

        private float floor = 600.0f;
        public static Point3D gravity = new Point3D(0, 0, 0);

        public PhysicsObject() { }
        public PhysicsObject(Mesh m, int mass) {
            this.mesh = m;
            this.velocity = new Point3D();
            this.angularVelocity = new Point3D();
            this.mass = mass;

            velocity.X = velocity.Z = velocity.Z = 0;
            angularVelocity.X = angularVelocity.Y = angularVelocity.Z = 0;

            radius = 0.7f*averageRadius();
        }

        private float averageRadius() {
            float sum = 0;
            foreach (Point3D p in mesh.getVerticies()) {
                sum += Math.Abs((p - MassCenter).Magnitude);
            }
            return (sum / mesh.VertexCount);
        }

        public Point3D MassCenter { get { return mesh.Center; } }
        public Point3D Velocity { get { return velocity; } set { velocity = value; } }
        public Point3D AngularVelocity { get { return angularVelocity; } set { angularVelocity = value; } }
        public float Radius { get { return radius; } }

		public void applyGravity(){
            if (gravity.Y != 0)
            {
                if (Math.Abs(MassCenter.Y + (Math.Sign(gravity.Y)) *radius) >= floor)
                {
                    velocity = -velocity / 1.5f;
                    angularVelocity = angularVelocity / 1.5f;
                }
                else
                {
                    velocity += gravity;
                }
            }
		}

        public void applyVelocity()
        {
            if (Math.Abs(MassCenter.X + Math.Sign(velocity.X) * radius) >= floor) {
                velocity.X *= -1;
                angularVelocity.X += velocity.X / 100;
            }
            if (Math.Abs(MassCenter.Y + Math.Sign(velocity.Y) * radius) >= floor) {
                velocity.Y *= -1;
                angularVelocity.Y += velocity.Y / 100;
            }
            if (Math.Abs(MassCenter.Z + Math.Sign(velocity.Z) * radius) + 600.0f >= floor){
                velocity.Z *= -1;
                angularVelocity.Z += velocity.Z/100;
            }
            mesh.Translate(velocity);
            mesh.Rotate(angularVelocity);
        }      

        public static void interact(ref List<PhysicsObject> collider)
        {
            float m0 = collider[0].mass;
            float m1 = collider[1].mass;
            float total_M = m0 + m1;
            Point3D Vdiff = collider[0].velocity - collider[1].velocity;
            Point3D CMdiff = collider[0].MassCenter - collider[1].MassCenter;
            float common = 2 * (Point3D.Dot(Vdiff, CMdiff)) / (Point3D.Dot(CMdiff, CMdiff) * (total_M));
            collider[0].velocity -= (CMdiff) * (common * m1);
            collider[1].velocity -= (-CMdiff) * (common * m0);

            m0 *= collider[0].Radius * collider[0].Radius;
            m1 *= collider[1].Radius * collider[1].Radius;
            total_M = m0 + m1;
            Vdiff = collider[0].AngularVelocity - collider[1].AngularVelocity;
            //Point3D CMdiff = collider[0].MassCenter - collider[1].MassCenter;
            CMdiff = Point3D.Cross(collider[0].AngularVelocity, collider[1].AngularVelocity);
            common = 2 * (Point3D.Dot(Vdiff, CMdiff)) / (total_M);
            collider[0].AngularVelocity = (CMdiff) * (common * m1);
            collider[1].AngularVelocity = (-CMdiff) * (common * m0);

            collider[0].applyVelocity();
            collider[1].applyVelocity();
        }

        public Mesh getMesh() {
            return this.mesh;
        }
    }
}
