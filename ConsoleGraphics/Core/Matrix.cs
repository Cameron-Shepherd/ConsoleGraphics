using System;
using System.Linq;

namespace ConsoleGraphics.Core
{
    public class Matrix
    {
        private float[,] matrix;//row,col
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(float[,] vals, int rows, int cols)
        {
            matrix = new float[rows, cols];
            this.Rows = rows;
            this.Cols = cols;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = vals[i, j];
                }
            }
        }
        public Matrix(Point3D a, Point3D b, Point3D c)
        {
            matrix = new float[,] { { a.X, b.X, c.X }, { a.Y, b.Y, c.Y }, { a.Z, b.Z, c.Z } };
            this.Rows = 3;
            this.Cols = 3;
        }

        public Matrix(Point3D a)
        {
            matrix = new float[,] { { a.X, a.Y, a.Z, 1 } };
            this.Rows = 1;
            this.Cols = 4;
        }
        public Matrix(Vector a)
        {
            matrix = new float[,] { { a.X, a.Y, a.Z, 1 } };
            this.Rows = 1;
            this.Cols = 4;
        }

        public Matrix(float[] vals)
        {
            matrix = new float[vals.Length, 1];
            vals.Select((f, i) => matrix[i, 0] = vals[i]);
        }

        public float this[int row, int col] => matrix[row, col];

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            float[,] temp = new float[a.Rows, b.Cols];
            if (a.Cols == b.Rows)
            {
                for (int i = 0; i < a.Rows; i++) // 4
                {
                    for (int j = 0; j < b.Cols; j++) //2
                    {
                        float sum = 0;
                        for (int col = 0; col < a.Cols; col++)
                        {
                            sum += (a.matrix[i, col] * b.matrix[col, j]);
                        }
                        temp[i, j] = sum;
                    }
                }
            }
            return new Matrix(temp, a.Rows, b.Cols);
        }

        public Matrix Multiply(Matrix a)
        {
            return Matrix.Multiply(this, a);
        }

        public static Matrix Transpose(Matrix a)
        {
            float[,] data = new float[a.Rows, a.Cols];
            if (a.Rows == a.Cols)
            {
                // copy the data
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Cols; j++)
                    {
                        data[i, j] = a.matrix[i, j];
                    }
                }
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = i; j < a.Cols; j++)
                    {
                        if (i != j)
                        {
                            swap(data, i, j, j, i);
                        }
                    }
                }
            }
            return new Matrix(data, a.Rows, a.Cols);
        }
        private static void swap(float[,] data, int row1, int col1, int row2, int col2)
        {
            float temp = data[row1, col1];
            data[row1, col1] = data[row2, col2];
            data[row2, col2] = temp;
        }
        public static Matrix RotationX(float x)
        {
            float[,] temp = new float[,] { { 1, 0, 0, 0 }, { 0, (float)Math.Cos(x), (float)-Math.Sin(x), 0 }, { 0, (float)Math.Sin(x), (float)Math.Cos(x), 0 }, { 0, 0, 0, 1 } };
            return new Matrix(temp, 4, 4);
        }
        public static Matrix RotationY(float x)
        {
            float[,] temp = new float[,] { { (float)Math.Cos(x), 0, (float)Math.Sin(x), 0 }, { 0, 1, 0, 0 }, { (float)-Math.Sin(x), 0, (float)Math.Cos(x), 0 }, { 0, 0, 0, 1 } };
            return new Matrix(temp, 4, 4);
        }
        public static Matrix RotationZ(float x)
        {
            float[,] temp = new float[,] { { (float)Math.Cos(x), (float)-Math.Sin(x), 0, 0 }, { (float)Math.Sin(x), (float)Math.Cos(x), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            return new Matrix(temp, 4, 4);
        }
        public static Matrix Translation(float x, float y, float z)
        {
            float[,] temp = new float[,] { { 1, 0, 0, x }, { 0, 1, 0, y }, { 0, 0, 1, z }, { 0, 0, 0, 1 } };
            return new Matrix(temp, 4, 4);
        }
        public void RotateX(float x)
        {
            matrix = (Matrix.Multiply(this, Matrix.RotationX(x))).matrix;
        }
        public void RotateY(float y)
        {
            matrix = (Matrix.Multiply(this, Matrix.RotationY(y))).matrix;
        }
        public void RotateZ(float z)
        {
            matrix = (Matrix.Multiply(this, Matrix.RotationZ(z))).matrix;
        }
        public void RotateXYZ(Matrix m, float x, float y, float z)
        {
            m.RotateX(x);
            m.RotateY(y);
            m.RotateZ(z);
        }
        public void UnRotateXYZ(Matrix m, float x, float y, float z)
        {
            m.RotateZ(-z);
            m.RotateY(-y);
            m.RotateX(-x);
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Multiply(a, b);
        }
        public static Matrix operator *(Point3D a, Matrix b)
        {
            Matrix temp = new Matrix(a);
            return Multiply(temp, b);
        }
        public static Matrix operator *(Matrix a, Point3D b)
        {
            float[,] vals = new float[,] { { b.X }, { b.Y }, { b.Z }, { 1} };
            Matrix temp = new Matrix(vals, 4, 1);
            return Multiply(a, temp);
        }
        public Point3D boxCoordsToPoint3D()
        {
            float w = matrix[0, 3];
            return new Point3D(matrix[0,0]/w, matrix[0, 1] / w,matrix[0, 2] / w);
        }
    }
}
