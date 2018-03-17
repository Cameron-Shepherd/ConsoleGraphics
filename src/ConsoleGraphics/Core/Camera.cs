using System;

namespace ConsoleGraphics.Core
{
    public class Camera
    {
        public Camera(Point4D position, Vector4D direction, Vector4D up, float nearPlane, float farPlane, float viewAngle)
        {
            Position = position;
            Direction = direction;
            Up = up;
            NearPlane = nearPlane;
            FarPlane = farPlane;
            ViewAngle = (float)(viewAngle * Math.PI / 180.0f); ;
        }

        public Point4D Position { get; }
        public Vector4D Direction { get; }
        public Vector4D Up { get; }
        public float NearPlane { get; }
        public float FarPlane { get; }
        public float ViewAngle { get; }

        //public void calcWorldtoCameraMatrix()
        //{
        //    Matrix mat;
        //    Point3D r = -Point3D.Cross(direction, up);
        //    Point3D u = Point3D.Cross(r, direction);
        //    direction.Normalize();
        //    r.Normalize();
        //    u.Normalize();
        //    //float[,] vals = new float[,] { { r.X, r.Y, r.Z, Point3D.Dot(r,-position)}, { u.X, u.Y, u.Z, Point3D.Dot(u, -position)  }, { -direction.X, -direction.Y, -direction.Z, Point3D.Dot(direction, position) }, { 0, 0, 0, 1 } };
        //    float[,] vals = new float[,] { { r.X, u.X, -direction.X, 0 }, { r.Y, u.Y, -direction.Y, 0 }, { r.Z, u.Z, -direction.Z, 0 }, { 0, 0, 0, 1 } };
        //    Matrix M_R = new Matrix(vals, 4, 4);
        //    Matrix M_T = Matrix.Translation(-position.X, -position.Y, -position.Z);
        //    mat = Matrix.Multiply(M_R, M_T);
        //    //mat = Matrix.Multiply(M_T, M_R);
        //    worldToCameraMatrix = Matrix.Transpose(mat);
        //}
        //public Point3D ViewVector
        //{
        //    get
        //    {
        //        return new Point3D(direction.X, direction.Y, direction.Z);
        //    }
        //    set { direction = value; }
        //}
        //public Point3D UpVector
        //{
        //    get
        //    {
        //        return new Point3D(up.X, up.Y, up.Z);
        //    }
        //    set { up = value; }
        //}
        //public Matrix TranslateToOrigin()
        //{
        //    return Matrix.Translation(-position.X, -position.Y, -position.Z);
        //}
        //public void Translate(Point3D pt)
        //{
        //    position.X += pt.X;
        //    position.Y += pt.Y;
        //    position.Z += pt.Z;
        //    calcWorldtoCameraMatrix();
        //}
        //public Matrix getCameraViewMatrix()
        //{
        //    float[,] vals = new float[,] { { (1 / (float)Math.Tan(viewAngle / 2)), 0, 0, 0 }, { 0, (1 / (float)Math.Tan(viewAngle / 2)), 0, 0 }, { 0, 0, (f + n) / (f - n), -1 }, { 0, 0, (2 * f * n) / (f - n), 0 } };
        //    return new Matrix(vals,4,4);
        //}
        //public void Rotate(Point3D theta)
        //{
        //    direction.Rotate(theta);
        //    up.Rotate(theta);
        //    calcWorldtoCameraMatrix();
        //}

        //public void RotateAround(Point3D pt, Point3D amount)
        //{
        //    Point3D translate = new Point3D(0, 0, 0) - pt;
        //    this.position.Translate(translate);
        //    this.position.Rotate(amount);
        //    this.position.Translate(-translate);
        //    direction = position - pt;
        //    direction.Normalize();
        //    calcWorldtoCameraMatrix();
        //}
    }
}
