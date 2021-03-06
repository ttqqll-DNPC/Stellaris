﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Stellaris.Graphics
{
    public class VertexInfo
    {
        public Vertex[] vertex;
        public short[] index;
        public VertexInfo(Vertex[] vertex, short[] index)
        {
            this.vertex = vertex;
            this.index = index;
        }
        public VertexInfo(Vertex[] vertex)
        {
            this.vertex = vertex;
            index = new short[vertex.Length];
            for (short i = 0; i < vertex.Length; i++)
            {
                index[i] = i;
            }
        }
        public VertexInfo TransformPosition(Matrix matrix, Vector2 center = default)
        {
            Vertex[] vertices = new Vertex[vertex.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertex[i].ChangePosition(Vector2.Transform(vertex[i].Position - center, matrix) + center);
            }
            return new VertexInfo(vertices, index);
        }
        public VertexInfo TransformPosition(Func<int, Vector2, Vector2> positionFunction)
        {
            Vertex[] vertices = new Vertex[vertex.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertex[i].ChangePosition(positionFunction(i, vertex[i].Position));
            }
            return new VertexInfo(vertices, index);
        }
        public VertexInfo TransformColor(Func<int, Color, Color> colorFunction)
        {
            Vertex[] vertices = new Vertex[vertex.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertex[i].ChangeColor(colorFunction(i, vertex[i].Color));
            }
            return new VertexInfo(vertices, index);
        }
        public VertexInfo TransformCoord(Func<int, Vector2, Vector2> coordFunction)
        {
            Vertex[] vertices = new Vertex[vertex.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertex[i].ChangeCoord(coordFunction(i, vertex[i].TextureCoordinate));
            }
            return new VertexInfo(vertices, index);
        }
        public VertexInfo Transform(Func<int, Vertex, Vertex> vertexFunction)
        {
            Vertex[] vertices = new Vertex[vertex.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertexFunction(i, vertex[i]);
            }
            return new VertexInfo(vertices, index);
        }
    }
    public struct Vertex : IVertexType
    {
        public Vector2 Position;
        public Color Color;
        public Vector2 TextureCoordinate;
        static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
        public Vertex(Vector2 position)
        {
            Position = position;
            Color = Color.White;
            TextureCoordinate = Vector2.Zero;
        }
        public Vertex(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            TextureCoordinate = Vector2.Zero;
        }
        public Vertex(Vector2 position, Color color, Vector2 textureCoord)
        {
            Position = position;
            Color = color;
            TextureCoordinate = textureCoord;
        }
        public Vertex(Vector2 position, Vector2 textureCoord)
        {
            Position = position;
            Color = Color.White;
            TextureCoordinate = textureCoord;
        }
        public static Vertex operator +(Vertex left, Vertex right)
        {
            return new Vertex(left.Position + right.Position, (left.Color.ToVector4() + right.Color.ToVector4()).ToColor(), left.TextureCoordinate + right.TextureCoordinate);
        }
        public static Vertex operator *(Vertex left, float right)
        {
            return new Vertex(left.Position * right, (left.Color.ToVector4() * right).ToColor(), left.TextureCoordinate * right);
        }
        public static Vertex operator /(Vertex left, float right)
        {
            return new Vertex(left.Position / right, (left.Color.ToVector4() / right).ToColor(), left.TextureCoordinate / right);
        }
        public Vertex AddPosition(Vector2 position)
        {
            return new Vertex(Position + position, Color, TextureCoordinate);
        }
        public Vertex ChangePosition(Vector2 newPosition)
        {
            return new Vertex(newPosition, Color, TextureCoordinate);
        }
        public Vertex ChangeColor(Color newColor)
        {
            return new Vertex(Position, newColor, TextureCoordinate);
        }
        public Vertex AddCrood(Vector2 coord)
        {
            return new Vertex(Position, Color, TextureCoordinate + coord);
        }
        public Vertex ChangeCoord(Vector2 newCoord)
        {
            return new Vertex(Position, Color, newCoord);
        }
        public Vertex ChangeCoord(float newCoordX, float newCoordY)
        {
            return new Vertex(Position, Color, new Vector2(newCoordX, newCoordY));
        }
        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
